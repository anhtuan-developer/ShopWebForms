using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace web_ban_hang2.Services
{
    /// <summary>
    /// AI Chatbot sử dụng Ollama + Qwen3.
    ///
    /// Luồng:
    /// ChatbotHandler
    ///      ↓
    /// ChatbotCatalogService
    ///      ↓
    /// AIChatbotService
    ///      ↓
    /// Ollama /api/chat
    ///      ↓
    /// Qwen3
    /// </summary>
    public class AIChatbotService
    {
        private readonly string ollamaUrl;
        private readonly string model;

        // =========================================================
        // HTTP CLIENT
        // =========================================================

        private static readonly HttpClient client =
            CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            HttpClient httpClient =
                new HttpClient();

            /*
             * Qwen3 trên máy local có thể mất 15-30 giây.
             *
             * 60 giây là đủ rộng nhưng không để request treo
             * quá lâu.
             */
            httpClient.Timeout =
                TimeSpan.FromSeconds(60);

            return httpClient;
        }

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public AIChatbotService()
        {
            ollamaUrl =
                ConfigurationManager.AppSettings[
                    "OllamaUrl"];

            if (
                string.IsNullOrWhiteSpace(
                    ollamaUrl))
            {
                ollamaUrl =
                    "http://localhost:11434/api/chat";
            }

            model =
                ConfigurationManager.AppSettings[
                    "OllamaModel"];

            if (
                string.IsNullOrWhiteSpace(
                    model))
            {
                model =
                    "qwen3:4b";
            }
        }

        // =========================================================
        // ASK ASYNC
        // =========================================================

        public async Task<string> AskAsync(
            string question,
            string catalogContext,
            IList<ChatMessage> history)
        {
            if (
                string.IsNullOrWhiteSpace(
                    question))
            {
                return
                    "Bạn hãy nhập câu hỏi để mình hỗ trợ nhé 😊";
            }

            question =
                question.Trim();

            if (question.Length > 800)
            {
                question =
                    question.Substring(
                        0,
                        800);
            }

            JObject payload =
                BuildChatPayload(
                    question,
                    catalogContext,
                    history);

            string json =
                payload.ToString();

            using (
                StringContent content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"))
            {
                HttpResponseMessage response;

                try
                {
                    response =
                        await client
                            .PostAsync(
                                ollamaUrl,
                                content)
                            .ConfigureAwait(false);
                }
                catch (TaskCanceledException ex)
                {
                    throw new InvalidOperationException(
                        "OLLAMA_TIMEOUT",
                        ex);
                }
                catch (HttpRequestException ex)
                {
                    throw new InvalidOperationException(
                        "OLLAMA_CONNECTION",
                        ex);
                }

                using (response)
                {
                    string responseBody =
                        await response.Content
                            .ReadAsStringAsync()
                            .ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException(
                            GetOllamaErrorMessage(
                                responseBody,
                                response.StatusCode));
                    }

                    JObject result;

                    try
                    {
                        result =
                            JObject.Parse(
                                responseBody);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            "OLLAMA_INVALID_JSON",
                            ex);
                    }

                    // =================================================
                    // LẤY message.content
                    // =================================================

                    string answer =
                        ExtractAnswer(
                            result);

                    // =================================================
                    // CLEAN
                    // =================================================

                    answer =
                        CleanModelAnswer(
                            answer);

                    if (
                        string.IsNullOrWhiteSpace(
                            answer))
                    {
                        /*
                         * AI không trả nội dung.
                         * Nếu có catalogContext thì cố gắng trả lời
                         * trực tiếp từ dữ liệu SHOP.
                         */
                        return
                            BuildVietnameseFallback(
                                question,
                                catalogContext);
                    }

                    // =================================================
                    // KIỂM TRA CÂU TRẢ LỜI CÓ PHẢI META/ENGLISH
                    // =================================================

                    if (
                        LooksLikeInvalidAIAnswer(
                            answer))
                    {
                        /*
                         * KHÔNG gọi Qwen3 lần 2.
                         *
                         * Dùng dữ liệu SHOP thật đã được
                         * ChatbotCatalogService truy vấn.
                         */
                        return
                            BuildVietnameseFallback(
                                question,
                                catalogContext);
                    }

                    return answer.Trim();
                }
            }
        }

        // =========================================================
        // EXTRACT ANSWER
        // =========================================================

        private static string ExtractAnswer(
            JObject result)
        {
            if (result == null)
            {
                return string.Empty;
            }

            /*
             * Ollama /api/chat:
             *
             * {
             *   "message": {
             *       "role": "assistant",
             *       "content": "..."
             *   }
             * }
             */

            JToken message =
                result["message"];

            if (message != null)
            {
                JToken content =
                    message["content"];

                if (content != null)
                {
                    return
                        content.ToString();
                }
            }

            /*
             * Fallback nếu Ollama trả response
             * theo kiểu /api/generate.
             */
            JToken response =
                result["response"];

            if (response != null)
            {
                return
                    response.ToString();
            }

            return string.Empty;
        }

        // =========================================================
        // BUILD PAYLOAD
        // =========================================================

        private JObject BuildChatPayload(
            string question,
            string catalogContext,
            IList<ChatMessage> history)
        {
            JArray messages =
                new JArray();

            // =====================================================
            // SYSTEM
            // =====================================================

            messages.Add(
                new JObject
                {
                    ["role"] =
                        "system",

                    ["content"] =
                        "Bạn là trợ lý bán hàng "
                        + "của SHOP 5 ANH EM. "

                        + "Hãy trả lời khách hàng "
                        + "hoàn toàn bằng tiếng Việt. "

                        + "Không được dịch câu hỏi. "

                        + "Không được mô tả quá trình suy nghĩ. "

                        + "Không được viết các câu như "
                        + "'Okay', "
                        + "'The user is asking', "
                        + "'First, I need', "
                        + "'Let me check'. "

                        + "Không được nói về prompt, "
                        + "model, AI hoặc hệ thống nội bộ. "

                        + "Chỉ sử dụng dữ liệu SHOP được cung cấp. "

                        + "Không tự bịa tên sản phẩm, "
                        + "giá, tồn kho hoặc đơn hàng. "

                        + "Nếu dữ liệu không có câu trả lời, "
                        + "hãy nói rõ chưa có dữ liệu. "

                        + "Trả lời trực tiếp, "
                        + "ngắn gọn và tự nhiên."
                });

            // =====================================================
            // HISTORY
            // =====================================================

            if (
                history != null &&
                history.Count > 0)
            {
                /*
                 * Chỉ lấy 2 lượt gần nhất.
                 *
                 * Giảm context để Qwen3 phản hồi nhanh.
                 */
                int start =
                    Math.Max(
                        0,
                        history.Count - 2);

                for (
                    int i = start;
                    i < history.Count;
                    i++)
                {
                    ChatMessage item =
                        history[i];

                    if (
                        item == null ||
                        string.IsNullOrWhiteSpace(
                            item.Content))
                    {
                        continue;
                    }

                    string role =
                        string.Equals(
                            item.Role,
                            "assistant",
                            StringComparison.OrdinalIgnoreCase)
                            ? "assistant"
                            : "user";

                    string text =
                        item.Content.Trim();

                    if (text.Length > 350)
                    {
                        text =
                            text.Substring(
                                0,
                                350)
                            + "...";
                    }

                    messages.Add(
                        new JObject
                        {
                            ["role"] =
                                role,

                            ["content"] =
                                text
                        });
                }
            }

            // =====================================================
            // USER
            // =====================================================

            StringBuilder userMessage =
                new StringBuilder();

            userMessage.AppendLine(
                "DỮ LIỆU SHOP THỰC TẾ:");

            if (
                string.IsNullOrWhiteSpace(
                    catalogContext))
            {
                userMessage.AppendLine(
                    "Không có dữ liệu SHOP "
                    + "liên quan được cung cấp.");
            }
            else
            {
                /*
                 * Giới hạn context để tránh prompt quá dài.
                 */
                string safeContext =
                    catalogContext;

                if (safeContext.Length > 9000)
                {
                    safeContext =
                        safeContext.Substring(
                            0,
                            9000)
                        + "\r\n...";
                }

                userMessage.AppendLine(
                    safeContext);
            }

            userMessage.AppendLine();

            userMessage.AppendLine(
                "CÂU HỎI CỦA KHÁCH:");

            userMessage.AppendLine(
                question);

            userMessage.AppendLine();

            userMessage.AppendLine(
                "Hãy trả lời trực tiếp "
                + "câu hỏi của khách bằng tiếng Việt.");

            messages.Add(
                new JObject
                {
                    ["role"] =
                        "user",

                    ["content"] =
                        userMessage.ToString()
                });

            // =====================================================
            // OLLAMA
            // =====================================================

            return
                new JObject
                {
                    ["model"] =
                        model,

                    ["messages"] =
                        messages,

                    ["stream"] =
                        false,

                    /*
                     * Qwen3:
                     * yêu cầu không hiển thị reasoning.
                     */
                    ["think"] =
                        false,

                    ["keep_alive"] =
                        "10m",

                    ["options"] =
                        new JObject
                        {
                            ["temperature"] =
                                0.1,

                            /*
                             * 120 token đủ cho câu trả lời
                             * bán hàng thông thường.
                             */
                            ["num_predict"] =
                                120,

                            ["num_ctx"] =
                                1536,

                            ["top_k"] =
                                10,

                            ["top_p"] =
                                0.8,

                            ["repeat_penalty"] =
                                1.05
                        }
                };
        }

        // =========================================================
        // KIỂM TRA AI RESPONSE
        // =========================================================

        private static bool LooksLikeInvalidAIAnswer(
            string text)
        {
            if (
                string.IsNullOrWhiteSpace(
                    text))
            {
                return true;
            }

            string normalized =
                text.ToLowerInvariant();

            normalized =
                Regex.Replace(
                    normalized,
                    @"\s+",
                    " ");

            // =====================================================
            // META / REASONING
            // =====================================================

            string[] invalidPatterns =
            {
                "the user is asking",
                "the user asks",
                "the user wants",
                "let me check",
                "let me see",
                "first, i need",
                "okay,",
                "okay so",
                "sure,",
                "i need to",
                "i should",
                "i think",
                "i will",
                "we need to",
                "from the name",
                "i don't have specific",
                "i do not have specific",
                "the question is",
                "the original sentence",
                "translate the question",
                "translation of",
                "in vietnamese",
                "in english"
            };

            foreach (
                string pattern
                in invalidPatterns)
            {
                if (
                    normalized.Contains(
                        pattern))
                {
                    return true;
                }
            }

            // =====================================================
            // ENGLISH SCORE
            // =====================================================

            string[] words =
                Regex.Split(
                    normalized,
                    @"[^a-zA-ZÀ-ỹ]+");

            string[] englishWords =
            {
                "the",
                "this",
                "that",
                "with",
                "from",
                "under",
                "over",
                "price",
                "prices",
                "recommend",
                "recommended",
                "recommendation",
                "available",
                "availability",
                "because",
                "however",
                "between",
                "performance",
                "battery",
                "display",
                "processor",
                "memory",
                "storage",
                "customer",
                "order",
                "delivery",
                "choose",
                "choice"
            };

            int englishScore =
                0;

            int vietnameseScore =
                0;

            foreach (
                string word
                in words)
            {
                if (
                    string.IsNullOrWhiteSpace(
                        word))
                {
                    continue;
                }

                if (
                    Array.IndexOf(
                        englishWords,
                        word) >= 0)
                {
                    englishScore++;
                }

                if (
                    word == "bạn" ||
                    word == "mình" ||
                    word == "shop" ||
                    word == "sản" ||
                    word == "phẩm" ||
                    word == "giá" ||
                    word == "hàng" ||
                    word == "đang" ||
                    word == "còn" ||
                    word == "đơn")
                {
                    vietnameseScore++;
                }
            }

            return
                englishScore >= 3 &&
                englishScore >
                    vietnameseScore + 1;
        }

        // =========================================================
        // FALLBACK TIẾNG VIỆT
        // =========================================================

        private static string BuildVietnameseFallback(
            string question,
            string catalogContext)
        {
            if (
                string.IsNullOrWhiteSpace(
                    catalogContext))
            {
                return
                    "Mình chưa có đủ dữ liệu để "
                    + "trả lời chính xác câu hỏi này. "
                    + "Bạn cho mình biết thêm thông tin nhé.";
            }

            string q =
                RemoveVietnameseDiacritics(
                    question)
                .ToLowerInvariant();

            // =====================================================
            // ĐƠN HÀNG
            // =====================================================

            if (
                q.Contains("don hang") ||
                q.Contains("don cua toi") ||
                q.Contains("dat hang"))
            {
                string orders =
                    ExtractOrderLines(
                        catalogContext);

                if (!string.IsNullOrWhiteSpace(
                    orders))
                {
                    return
                        "Đây là thông tin các đơn hàng "
                        + "gần đây của bạn:\n"
                        + orders;
                }

                if (
                    catalogContext.Contains(
                        "Khách hàng chưa có đơn hàng"))
                {
                    return
                        "Hiện tại bạn chưa có "
                        + "đơn hàng nào.";
                }

                if (
                    catalogContext.Contains(
                        "Chưa đăng nhập"))
                {
                    return
                        "Bạn cần đăng nhập để mình "
                        + "kiểm tra đơn hàng cá nhân.";
                }
            }

            // =====================================================
            // SẢN PHẨM
            // =====================================================

            string[] productLines =
                ExtractProductLines(
                    catalogContext);

            if (productLines.Length > 0)
            {
                StringBuilder result =
                    new StringBuilder();

                if (
                    q.Contains("con hang") ||
                    q.Contains("ton kho"))
                {
                    result.Append(
                        "Hiện shop đang có các "
                        + "sản phẩm còn hàng:\n");
                }
                else if (
                    q.Contains("gia") ||
                    q.Contains("bao nhieu") ||
                    q.Contains("trieu"))
                {
                    result.Append(
                        "Mình tìm thấy các sản phẩm "
                        + "phù hợp trong dữ liệu shop:\n");
                }
                else
                {
                    result.Append(
                        "Mình tìm thấy các sản phẩm "
                        + "phù hợp:\n");
                }

                int max =
                    Math.Min(
                        productLines.Length,
                        6);

                for (
                    int i = 0;
                    i < max;
                    i++)
                {
                    result.Append(
                        productLines[i]);

                    result.AppendLine();
                }

                return result.ToString().Trim();
            }

            // =====================================================
            // KHÔNG CÓ SẢN PHẨM
            // =====================================================

            if (
                catalogContext.Contains(
                    "Không tìm thấy sản phẩm"))
            {
                return
                    "Mình chưa tìm thấy sản phẩm "
                    + "đang bán và còn hàng phù hợp "
                    + "với yêu cầu của bạn. "
                    + "Bạn thử thay đổi từ khóa "
                    + "hoặc mức giá nhé.";
            }

            return
                "Mình chưa có đủ dữ liệu để "
                + "trả lời chính xác câu hỏi này. "
                + "Bạn cho mình biết thêm tên "
                + "hoặc loại sản phẩm nhé.";
        }

        // =========================================================
        // EXTRACT PRODUCT
        // =========================================================

        private static string[] ExtractProductLines(
            string catalogContext)
        {
            if (
                string.IsNullOrWhiteSpace(
                    catalogContext))
            {
                return new string[0];
            }

            List<string> products =
                new List<string>();

            string[] lines =
                catalogContext.Split(
                    new[]
                    {
                        '\r',
                        '\n'
                    },
                    StringSplitOptions.RemoveEmptyEntries);

            foreach (
                string rawLine
                in lines)
            {
                string line =
                    rawLine.Trim();

                if (
                    !line.StartsWith(
                        "- MaSP=",
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                /*
                 * Format hiện tại:
                 *
                 * - MaSP=1; Tên=...;
                 * Danh mục=...; Giá=... VNĐ;
                 * Tồn kho=...
                 */

                string ten =
                    ExtractField(
                        line,
                        "Tên=",
                        "; Danh mục=");

                string category =
                    ExtractField(
                        line,
                        "Danh mục=",
                        "; Giá=");

                string price =
                    ExtractField(
                        line,
                        "Giá=",
                        "; Tồn kho=");

                string stock =
                    ExtractField(
                        line,
                        "Tồn kho=",
                        "; Mô tả=");

                if (
                    string.IsNullOrWhiteSpace(
                        ten))
                {
                    continue;
                }

                StringBuilder item =
                    new StringBuilder();

                item.Append(
                    "• ");

                item.Append(
                    ten.Trim());

                if (
                    !string.IsNullOrWhiteSpace(
                        category))
                {
                    item.Append(
                        " – ");

                    item.Append(
                        category.Trim());
                }

                if (
                    !string.IsNullOrWhiteSpace(
                        price))
                {
                    item.Append(
                        " – ");

                    item.Append(
                        price.Trim());
                }

                if (
                    !string.IsNullOrWhiteSpace(
                        stock))
                {
                    item.Append(
                        " – Còn ");

                    item.Append(
                        stock.Trim());

                    item.Append(
                        " sản phẩm");
                }

                products.Add(
                    item.ToString());
            }

            return products.ToArray();
        }

        // =========================================================
        // EXTRACT ORDERS
        // =========================================================

        private static string ExtractOrderLines(
            string catalogContext)
        {
            if (
                string.IsNullOrWhiteSpace(
                    catalogContext))
            {
                return string.Empty;
            }

            StringBuilder result =
                new StringBuilder();

            string[] lines =
                catalogContext.Split(
                    new[]
                    {
                        '\r',
                        '\n'
                    },
                    StringSplitOptions.RemoveEmptyEntries);

            foreach (
                string rawLine
                in lines)
            {
                string line =
                    rawLine.Trim();

                if (
                    !line.StartsWith(
                        "- Đơn #",
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string order =
                    line.Substring(2).Trim();

                result.Append(
                    "• ");

                result.AppendLine(
                    order);
            }

            return result.ToString().Trim();
        }

        // =========================================================
        // EXTRACT FIELD
        // =========================================================

        private static string ExtractField(
            string text,
            string start,
            string end)
        {
            int startIndex =
                text.IndexOf(
                    start,
                    StringComparison.OrdinalIgnoreCase);

            if (startIndex < 0)
            {
                return string.Empty;
            }

            startIndex +=
                start.Length;

            int endIndex =
                text.IndexOf(
                    end,
                    startIndex,
                    StringComparison.OrdinalIgnoreCase);

            if (endIndex < 0)
            {
                return
                    text.Substring(
                        startIndex).Trim();
            }

            return
                text.Substring(
                    startIndex,
                    endIndex - startIndex)
                .Trim();
        }

        // =========================================================
        // CLEAN
        // =========================================================

        private static string CleanModelAnswer(
            string answer)
        {
            if (
                string.IsNullOrWhiteSpace(
                    answer))
            {
                return string.Empty;
            }

            // Xóa <think>...</think>
            answer =
                Regex.Replace(
                    answer,
                    @"<think>.*?</think>",
                    string.Empty,
                    RegexOptions.IgnoreCase |
                    RegexOptions.Singleline);

            // Xóa phần <think> chưa đóng
            answer =
                Regex.Replace(
                    answer,
                    @"<think>.*",
                    string.Empty,
                    RegexOptions.IgnoreCase |
                    RegexOptions.Singleline);

            answer =
                answer.Replace(
                    "</think>",
                    "");

            // Xóa các marker thường gặp
            answer =
                Regex.Replace(
                    answer,
                    @"^\s*Thinking\s*:\s*",
                    "",
                    RegexOptions.IgnoreCase);

            answer =
                Regex.Replace(
                    answer,
                    @"^\s*Answer\s*:\s*",
                    "",
                    RegexOptions.IgnoreCase);

            answer =
                Regex.Replace(
                    answer,
                    @"^\s*Trả lời\s*:\s*",
                    "",
                    RegexOptions.IgnoreCase);

            answer =
                Regex.Replace(
                    answer,
                    @"\n{3,}",
                    "\n\n");

            return answer.Trim();
        }

        // =========================================================
        // REMOVE VIETNAMESE DIACRITICS
        // =========================================================

        private static string RemoveVietnameseDiacritics(
            string text)
        {
            if (
                string.IsNullOrWhiteSpace(
                    text))
            {
                return string.Empty;
            }

            string normalized =
                text.Normalize(
                    System.Text.NormalizationForm.FormD);

            StringBuilder result =
                new StringBuilder();

            foreach (
                char c
                in normalized)
            {
                System.Globalization.UnicodeCategory category =
                    System.Globalization.CharUnicodeInfo
                        .GetUnicodeCategory(c);

                if (
                    category !=
                    System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    result.Append(c);
                }
            }

            return
                result.ToString()
                    .Normalize(
                        System.Text.NormalizationForm.FormC)
                    .Replace(
                        'đ',
                        'd')
                    .Replace(
                        'Đ',
                        'D');
        }

        // =========================================================
        // OLLAMA ERROR
        // =========================================================

        private static string GetOllamaErrorMessage(
            string responseBody,
            HttpStatusCode statusCode)
        {
            try
            {
                if (
                    !string.IsNullOrWhiteSpace(
                        responseBody))
                {
                    JObject obj =
                        JObject.Parse(
                            responseBody);

                    if (
                        obj["error"] != null)
                    {
                        return
                            "OLLAMA_ERROR: "
                            + obj["error"].ToString();
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi parse.
            }

            return
                "OLLAMA_HTTP_"
                + (int)statusCode;
        }
    }
}