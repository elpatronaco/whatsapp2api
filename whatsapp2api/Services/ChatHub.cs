using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using whatsapp2api.Contracts.Services;

namespace whatsapp2api.Services
{
    public class ChatHub : Hub
    {
        private IUserService _userService;

        public ChatHub(IUserService userService)
        {
            _userService = userService;
        }

        public async Task SendGlobalMessage(params object[] args)
        {
            await Clients.All.SendAsync("test", args);
        }

        public async Task NewSession(string idToken)
        {
            var user = await _userService.UserFromToken(idToken);

            if (user == null) return; // TODO: HANDLE

            await _userService.SetUserConnectionId(user.Id, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _userService.RemoveUserConnectionId(Context.ConnectionId);
        }
    }
}