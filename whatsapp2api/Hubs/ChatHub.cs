using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using whatsapp2api.Contracts.Services;
using whatsapp2api.Models.Message;

namespace whatsapp2api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public ChatHub(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        public async Task SendGlobalMessage(params object[] args)
        {
            await Clients.All.SendAsync("test", args);
        }

        public async Task NewSession(string idToken)
        {
            var user = await _userService.UserFromToken(idToken);

            if (user == null) return;

            await _userService.SetUserConnectionId(user.Id, Context.ConnectionId);

            var chats = await _messageService.GetOpenChats(user.Id);

            await Clients.Caller.SendAsync("Chats", chats);
        }

        public async Task OpenChat(string id)
        {
            if (!Guid.TryParse(id, out Guid parsedId)) throw new ArgumentException("Id sent is not a valid Guid");

            var callerId = await _userService.GetUserIdByConnectionId(Context.ConnectionId);

            if (callerId == null) return;

            var messages = await _messageService.GetMessagesFromSenderToRecipient((Guid) callerId, parsedId);

            await Clients.Caller.SendAsync("Messages", messages);
        }

        public async Task SendMessage(string msg, string recipientId)
        {
            if (!Guid.TryParse(recipientId, out Guid parsedRecipientId))
                throw new ArgumentException("Id sent is not a valid Guid");

            MessageCreate message = new() {Content = msg, RecipientId = parsedRecipientId};

            var model = await _messageService.NewMessage(Context.ConnectionId, message);

            if (model == null) return;

            await Clients.Caller.SendAsync("New Message", model);

            var recipient = await _userService.GetUserById(parsedRecipientId);

            model.AmISender = false;

            if (recipient is not null && recipient is not {SocketConnectionId: null})
                await Clients.Client(recipient.SocketConnectionId)
                    .SendAsync("New Message", model);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _userService.RemoveUserConnectionId(Context.ConnectionId);
        }
    }
}