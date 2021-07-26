using System;

namespace whatsapp2api.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string? SocketConnectionId { get; set; }
    }
}