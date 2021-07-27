using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using whatsapp2api.Contracts.Repositories;
using whatsapp2api.Contracts.Services;
using whatsapp2api.Models.Chat;
using whatsapp2api.Models.Message;
using whatsapp2api.Models.User;

namespace whatsapp2api.Services
{
    public class UserComparer : IEqualityComparer<UserModel>
    {
        public bool Equals(UserModel? x, UserModel? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return x.GetType() == y.GetType() && x.Id.Equals(y.Id);
        }

        public int GetHashCode(UserModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserService _userService;

        public MessageService(IMessageRepository messageRepository, IUserService userService)
        {
            _messageRepository = messageRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<MessageModel>> GetMessagesFromSenderToRecipient(Guid senderId, Guid recipientId)
        {
            var messages =
                await _messageRepository.GetByCondition(
                    x =>
                        x.SenderId == senderId && x.RecipientId == recipientId ||
                        x.SenderId == recipientId && x.RecipientId == senderId);

            return messages.Select(x => x.ToDto(senderId)).ToList();
        }

        public async Task<IEnumerable<OpenChat>> GetOpenChats(Guid senderId)
        {
            var messages = await _messageRepository
                .GetByCondition(x =>
                    x.SenderId == senderId || x.RecipientId == senderId);

            var distinctRecipients = messages
                .GroupBy(x => new {x.SenderId, x.RecipientId})
                .Select(async x =>
                    await _userService.GetUserById(
                        x.Key.SenderId.Equals(senderId)
                            ? x.Key.RecipientId
                            : x.Key.SenderId)
                )
                .Select(x => x.Result)
                .Where(x => x is not null)
                .Select(x => x!)
                .Distinct(new UserComparer())
                .ToList();

            return distinctRecipients.Select(user =>
            {
                var lastMessage = messages
                    .Where(message =>
                        message.RecipientId == user.Id || message.SenderId == user.Id)
                    .OrderByDescending(message => message.SentDate.Date)
                    .ThenByDescending(message => message.SentDate.TimeOfDay)
                    .FirstOrDefault();

                return new OpenChat {Recipient = user, LastMessage = lastMessage?.ToDto(senderId)};
            });
        }

        public async Task<MessageModel?> NewMessage(string connectionId, MessageCreate owner)
        {
            var userId = await _userService.GetUserIdByConnectionId(connectionId);

            if (!userId.HasValue) return null;

            if (owner.Content.Length == 0) return null;

            var message = await _messageRepository.CreateMessage(userId.Value, owner);

            return message.ToDto(userId.Value);
        }
    }
}