using System.ComponentModel.DataAnnotations;

namespace whatsapp2api.Models.User
{
    public class UserUpdate
    {
        [MinLength(5)]
        [RegularExpression("^[a-zA-Z0-9]+([._]?[a-zA-Z0-9]+)*$",
            ErrorMessage = "Username has illegal characters")]
        public string? Username { get; set; }

        [MinLength(8)] public string? Password { get; set; }

        public string? SocketConnectionId { get; set; }
    }
}