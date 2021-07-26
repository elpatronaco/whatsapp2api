using System;

namespace whatsapp2api.Models.Message
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public bool AmISender { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }
    }
}