using System;

namespace whatsapp2api.Models.Message
{
    public class MessageCreate
    {
        public Guid RecipientId { get; set; }
        public string Content { get; set; }
    }
}