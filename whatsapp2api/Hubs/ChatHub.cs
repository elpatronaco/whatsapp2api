using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using whatsapp2api.Contracts.Services;

namespace whatsapp2api.Services
{
    public class ChatHub : Hub
    {
        private IUserService _userService;
        private IMessageService _messageService;

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

            await Clients.Caller.SendCoreAsync("Chats", new object?[] {chats});
        }

        public async Task OpenChat(string id)
        {
            if (!Guid.TryParse(id, out Guid parsedId)) throw new ArgumentException("Id sent is not a valid Guid");

            var callerId = await _userService.GetUserIdByConnectionId(Context.ConnectionId);

            if (callerId == null) return;

            var messages = await _messageService.GetMessagesFromSenderToRecipient((Guid) callerId, parsedId);

            await Clients.Caller.SendCoreAsync("Messages", new object?[] {messages});
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _userService.RemoveUserConnectionId(Context.ConnectionId);
        }
    }
}