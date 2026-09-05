using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using web_ban_hang2.Services;

namespace web_ban_hang2
{
    public class ChatbotHandler
        : IHttpHandler,
          IRequiresSessionState
    {
        private const string HistorySessionKey =
            "AIChatbotHistory";

        private const string RateSessionKey =
            "AIChatbotRate";

        private const int MaxRequestsPerMinute =
            15;

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(
            HttpContext context)
        {
            context.Response.ContentType =
                "application/json";

            context.Response.ContentEncoding =
                Encoding.UTF8;

            context.Response.Cache
                .SetCacheability(
                    HttpCacheability.NoCache);

            context.Response.Cache
                .SetNoStore();

            /*
             * =====================================================
             * CHỈ CHẤP NHẬN POST
             * =====================================================
             */
            if (
                !string.Equals(
                    context.Request.HttpMethod,
                    "POST",
                    StringComparison.OrdinalIgnoreCase))
            {
                WriteJson(
                    context,

                    new ChatbotResponse
                    {
                        Success =
                            false,

                        Message =
                            "Phương thức không được hỗ trợ."
                    },

                    405);

                return;
            }

            try
            {
                /*
                 * =================================================
                 * RATE LIMIT
                 * =================================================
                 */
                if (!CheckRateLimit(context))
                {
                    WriteJson(
                        context,

                        new ChatbotResponse
                        {
                            Success =
                                false,

                            Message =
                                "Bạn đang gửi hơi nhanh. "
                                + "Vui lòng chờ khoảng 1 phút nhé."
                        },

                        429);

                    return;
                }

                /*
                 * =================================================
                 * ĐỌC REQUEST JSON
                 * =================================================
                 */
                string body;

                using (
                    var reader =
                        new System.IO.StreamReader(
                            context.Request.InputStream))
                {
                    body =
                        reader.ReadToEnd();
                }

                ChatbotRequest request =
                    JsonConvert
                        .DeserializeObject
                        <ChatbotRequest>(
                            body);

                string question =
                    request == null
                        ? null
                        : request.Message;

                if (
                    string.IsNullOrWhiteSpace(
                        question))
                {
                    WriteJson(
                        context,

                        new ChatbotResponse
                        {
                            Success =
                                false,

                            Message =
                                "Bạn hãy nhập câu hỏi nhé 😊"
                        },

                        400);

                    return;
                }

                question =
                    question.Trim();

                /*
                 * Không cho gửi câu hỏi quá dài.
                 */
                if (question.Length > 800)
                {
                    question =
                        question.Substring(
                            0,
                            800);
                }

                /*
                 * =================================================
                 * TRẢ LỜI NHANH
                 * =================================================
                 *
                 * Các câu đơn giản không cần gọi AI.
                 */
                string quickAnswer =
                    GetQuickAnswer(
                        question);

                if (
                    !string.IsNullOrWhiteSpace(
                        quickAnswer))
                {
                    AddHistory(
                        context,
                        "user",
                        question);

                    AddHistory(
                        context,
                        "assistant",
                        quickAnswer);

                    WriteJson(
                        context,

                        new ChatbotResponse
                        {
                            Success =
                                true,

                            Message =
                                quickAnswer
                        },

                        200);

                    return;
                }

                /*
                 * =================================================
                 * CUSTOMER SESSION
                 * =================================================
                 */
                int? maKhachHang =
                    GetCustomerId(
                        context);

                /*
                 * Lấy lịch sử hiện tại.
                 */
                List<ChatMessage> history =
                    GetHistory(
                        context);

                /*
                 * =================================================
                 * DATABASE
                 * =================================================
                 */
                ChatbotCatalogService
                    catalogService =
                        new ChatbotCatalogService();

                string catalogContext =
                    catalogService
                        .BuildCatalogContext(
                            question,
                            maKhachHang,
                            history);

                /*
                 * =================================================
                 * AI
                 * =================================================
                 */
                AIChatbotService aiService =
                    new AIChatbotService();

                string answer =
                    aiService
                        .AskAsync(
                            question,
                            catalogContext,
                            history)
                        .GetAwaiter()
                        .GetResult();

                /*
                 * =================================================
                 * LƯU HISTORY
                 * =================================================
                 */
                AddHistory(
                    context,
                    "user",
                    question);

                AddHistory(
                    context,
                    "assistant",
                    answer);

                /*
                 * =================================================
                 * RESPONSE
                 * =================================================
                 */
                WriteJson(
                    context,

                    new ChatbotResponse
                    {
                        Success =
                            true,

                        Message =
                            answer
                    },

                    200);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace
                    .TraceError(
                        "AI chatbot error: "
                        + ex);

                WriteJson(
                    context,

                    new ChatbotResponse
                    {
                        Success =
                            false,

                        Message =
                            GetSafeErrorMessage(
                                ex)
                    },

                    500);
            }
        }

        /*
         * =========================================================
         * TRẢ LỜI NHANH
         * =========================================================
         */
        private static string GetQuickAnswer(
            string question)
        {
            string q =
                question
                    .Trim()
                    .ToLowerInvariant();

            /*
             * Xin chào
             */
            if (
                q == "xin chào" ||
                q == "xin chao" ||
                q == "chào" ||
                q == "chao" ||
                q == "hello" ||
                q == "hi")
            {
                return
                    "Xin chào 👋 "
                    + "Mình là trợ lý AI của SHOP 5 ANH EM. "
                    + "Bạn có thể hỏi mình về sản phẩm, "
                    + "giá, tồn kho, tư vấn mua hàng "
                    + "hoặc đơn hàng nhé.";
            }

            /*
             * Cảm ơn
             */
            if (
                q == "cảm ơn" ||
                q == "cam on" ||
                q.Contains("cảm ơn bạn") ||
                q.Contains("cam on ban"))
            {
                return
                    "Không có gì 😊 "
                    + "Mình luôn sẵn sàng hỗ trợ bạn!";
            }

            /*
             * Hỏi chatbot là ai
             */
            if (
                q == "bạn là ai" ||
                q == "ban la ai")
            {
                return
                    "Mình là trợ lý bán hàng AI "
                    + "của SHOP 5 ANH EM. "
                    + "Mình có thể giúp bạn tìm sản phẩm, "
                    + "tư vấn theo ngân sách và so sánh sản phẩm.";
            }

            /*
             * Tạm biệt
             */
            if (
                q == "tạm biệt" ||
                q == "tam biet" ||
                q == "bye")
            {
                return
                    "Tạm biệt 👋 "
                    + "Khi cần tư vấn sản phẩm, "
                    + "bạn cứ quay lại nhé!";
            }

            return null;
        }

        /*
         * =========================================================
         * LẤY ID KHÁCH HÀNG
         * =========================================================
         */
        private static int? GetCustomerId(
            HttpContext context)
        {
            object value =
                context.Session["UserId"];

            if (value == null)
            {
                return null;
            }

            int id;

            if (
                int.TryParse(
                    Convert.ToString(value),
                    out id)
                &&
                id > 0)
            {
                return id;
            }

            return null;
        }

        /*
         * =========================================================
         * HISTORY
         * =========================================================
         */
        private static List<ChatMessage>
            GetHistory(
                HttpContext context)
        {
            List<ChatMessage> history =
                context.Session[
                    HistorySessionKey]
                as List<ChatMessage>;

            if (history == null)
            {
                history =
                    new List<ChatMessage>();

                context.Session[
                    HistorySessionKey] =
                    history;
            }

            return history;
        }

        /*
         * =========================================================
         * THÊM HISTORY
         * =========================================================
         *
         * Chỉ giữ 6 message gần nhất:
         *
         * user
         * assistant
         * user
         * assistant
         * user
         * assistant
         */
        private static void AddHistory(
            HttpContext context,
            string role,
            string content)
        {
            List<ChatMessage> history =
                GetHistory(
                    context);

            history.Add(
                new ChatMessage
                {
                    Role =
                        role,

                    Content =
                        content
                });

            while (
                history.Count > 6)
            {
                history.RemoveAt(0);
            }
        }

        /*
         * =========================================================
         * RATE LIMIT
         * =========================================================
         */
        private static bool CheckRateLimit(
            HttpContext context)
        {
            RateLimitState state =
                context.Session[
                    RateSessionKey]
                as RateLimitState;

            DateTime now =
                DateTime.UtcNow;

            if (
                state == null ||
                now - state.StartedAtUtc
                    >= TimeSpan.FromMinutes(1))
            {
                state =
                    new RateLimitState
                    {
                        StartedAtUtc =
                            now,

                        Count =
                            0
                    };

                context.Session[
                    RateSessionKey] =
                    state;
            }

            state.Count++;

            return
                state.Count
                <= MaxRequestsPerMinute;
        }

        /*
         * =========================================================
         * XỬ LÝ LỖI AN TOÀN
         * =========================================================
         */
        private static string
            GetSafeErrorMessage(
                Exception ex)
        {
            string text =
                ex == null
                    ? string.Empty
                    : ex.ToString()
                        .ToLowerInvariant();

            /*
             * TIMEOUT
             */
            if (
                text.Contains(
                    "ollama_timeout") ||
                text.Contains(
                    "timeout") ||
                text.Contains(
                    "timed out"))
            {
                if (
                    text.Contains(
                        "ollama_timeout"))
                {
                    return
                        "AI đang xử lý hơi lâu. "
                        + "Bạn vui lòng thử lại sau vài giây nhé.";
                }
            }

            /*
             * CONNECTION
             */
            if (
                text.Contains(
                    "ollama_connection") ||
                text.Contains(
                    "connection refused") ||
                text.Contains(
                    "unable to connect"))
            {
                return
                    "Không thể kết nối tới Ollama. "
                    + "Bạn hãy kiểm tra Ollama đang chạy.";
            }

            /*
             * MODEL
             */
            if (
                text.Contains("model") &&
                text.Contains("not found"))
            {
                return
                    "Model Qwen3 chưa được cài đặt "
                    + "hoặc tên model trong Web.config không đúng.";
            }

            /*
             * DATABASE
             */
            if (
                text.Contains("sql") ||
                text.Contains("invalid object") ||
                text.Contains("invalid column") ||
                text.Contains("cannot open database"))
            {
                return
                    "Chatbot không đọc được dữ liệu sản phẩm "
                    + "từ cơ sở dữ liệu. "
                    + "Bạn hãy kiểm tra SQL Server.";
            }

            return
                "Xin lỗi, chatbot đang gặp sự cố tạm thời. "
                + "Bạn vui lòng thử lại nhé.";
        }

        /*
         * =========================================================
         * GHI JSON
         * =========================================================
         */
        private static void WriteJson(
            HttpContext context,
            object value,
            int statusCode)
        {
            context.Response.StatusCode =
                statusCode;

            context.Response.Write(
                JsonConvert.SerializeObject(
                    value));
        }

        /*
         * =========================================================
         * CLASS RATE LIMIT
         * =========================================================
         */
        private class RateLimitState
        {
            public DateTime StartedAtUtc
            {
                get;
                set;
            }

            public int Count
            {
                get;
                set;
            }
        }

        /*
         * =========================================================
         * REQUEST
         * =========================================================
         */
        private class ChatbotRequest
        {
            public string Message
            {
                get;
                set;
            }
        }

        /*
         * =========================================================
         * RESPONSE
         * =========================================================
         */
        private class ChatbotResponse
        {
            [JsonProperty("success")]
            public bool Success
            {
                get;
                set;
            }

            [JsonProperty("message")]
            public string Message
            {
                get;
                set;
            }
        }
    }
}