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

            if (!string.Equals(
                context.Request.HttpMethod,
                "POST",
                StringComparison.OrdinalIgnoreCase))
            {
                WriteJson(
                    context,

                    new ChatbotResponse
                    {
                        Success = false,

                        Message =
                            "Phương thức không được hỗ trợ."
                    },

                    405);

                return;
            }

            try
            {
                if (!CheckRateLimit(context))
                {
                    WriteJson(
                        context,

                        new ChatbotResponse
                        {
                            Success = false,

                            Message =
                                "Bạn đang gửi hơi nhanh. "
                                + "Vui lòng chờ khoảng "
                                + "1 phút rồi thử lại nhé."
                        },

                        429);

                    return;
                }

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
                    JsonConvert.DeserializeObject
                    <ChatbotRequest>(body);

                string question =
                    request == null
                        ? null
                        : request.Message;

                if (string.IsNullOrWhiteSpace(
                    question))
                {
                    WriteJson(
                        context,

                        new ChatbotResponse
                        {
                            Success = false,

                            Message =
                                "Bạn hãy nhập câu hỏi nhé 😊"
                        },

                        400);

                    return;
                }

                question =
                    question.Trim();

                if (question.Length > 1000)
                {
                    question =
                        question.Substring(
                            0,
                            1000);
                }

                int? maKhachHang =
                    GetCustomerId(
                        context);

                List<ChatMessage> history =
                    GetHistory(
                        context);

                ChatbotCatalogService
                    catalogService =
                        new ChatbotCatalogService();

                string catalogContext =
                    catalogService
                        .BuildCatalogContext(
                            question,
                            maKhachHang);

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

                AddHistory(
                    context,
                    "user",
                    question);

                AddHistory(
                    context,
                    "assistant",
                    answer);

                WriteJson(
                    context,

                    new ChatbotResponse
                    {
                        Success = true,

                        Message = answer
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
                        Success = false,

                        Message =
                            GetSafeErrorMessage(
                                ex)
                    },

                    500);
            }
        }

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

            if (int.TryParse(
                Convert.ToString(value),
                out id)
                && id > 0)
            {
                return id;
            }

            return null;
        }

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

        private static void AddHistory(
            HttpContext context,
            string role,
            string content)
        {
            List<ChatMessage> history =
                GetHistory(context);

            history.Add(
                new ChatMessage
                {
                    Role = role,
                    Content = content
                });

            while (history.Count > 12)
            {
                history.RemoveAt(0);
            }
        }

        private static bool CheckRateLimit(
            HttpContext context)
        {
            RateLimitState state =
                context.Session[
                    RateSessionKey]
                as RateLimitState;

            DateTime now =
                DateTime.UtcNow;

            if (state == null ||
                now - state.StartedAtUtc
                    >= TimeSpan.FromMinutes(1))
            {
                state =
                    new RateLimitState
                    {
                        StartedAtUtc = now,
                        Count = 0
                    };

                context.Session[
                    RateSessionKey] =
                    state;
            }

            state.Count++;

            return state.Count
                <= MaxRequestsPerMinute;
        }

        private static string
            GetSafeErrorMessage(
                Exception ex)
        {
            string text =
                ex == null
                    ? string.Empty
                    : ex.ToString()
                        .ToLowerInvariant();

            if (
                text.Contains(
                    "không thể kết nối tới ollama")
                || text.Contains(
                    "unable to connect")
                || text.Contains(
                    "connection refused")
                || text.Contains(
                    "no connection could be made"))
            {
                return
                    "Không thể kết nối tới Ollama. "
                    + "Bạn hãy kiểm tra Ollama đang chạy trên máy và thử lại nhé.";
            }

            if (
                text.Contains("model")
                && text.Contains("not found"))
            {
                return
                    "Model AI chưa được cài đặt trong Ollama. "
                    + "Hãy kiểm tra lại model trong Web.config.";
            }

            if (
                text.Contains("timeout")
                || text.Contains("timed out")
                || text.Contains("quá lâu"))
            {
                return
                    "AI đang phản hồi chậm. "
                    + "Bạn vui lòng thử lại sau ít giây nhé.";
            }

            return
                "Xin lỗi, chatbot đang gặp sự cố tạm thời. "
                + "Bạn vui lòng thử lại sau nhé.";
        }

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

        private class ChatbotRequest
        {
            public string Message
            {
                get;
                set;
            }
        }

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