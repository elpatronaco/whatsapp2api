using System;
using whatsapp2api.Models.User;

namespace whatsapp2api.Models.Message
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public UserModel? Sender { get; set; }
        public UserModel? Recipient { get; set; }
        public string Content { get; set; } = "";
        public DateTime SentDate { get; set; }
    }
}