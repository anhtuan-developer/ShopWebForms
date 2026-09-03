using System;

namespace web_ban_hang2.Services
{
    [Serializable]
    public class ChatMessage
    {
        public string Role { get; set; }

        public string Content { get; set; }
    }
}