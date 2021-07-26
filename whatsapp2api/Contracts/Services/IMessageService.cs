using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using whatsapp2api.Models.Chat;
using whatsapp2api.Models.Message;

namespace whatsapp2api.Contracts.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageModel>> GetMessagesFromSenderToRecipient(Guid senderId, Guid recipientId);
        Task<IEnumerable<OpenChat>> GetOpenChats(Guid senderId);
        Task<MessageModel?> NewMessage(string connectionId, MessageCreate owner);
    }
}