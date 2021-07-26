using whatsapp2api.Models.Message;
using whatsapp2api.Models.User;

namespace whatsapp2api.Models.Chat
{
    public class OpenChat
    {
        public UserModel Recipient { get; set; }
        public MessageModel? LastMessage { get; set; }
    }
}