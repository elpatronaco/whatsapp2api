using System.ComponentModel.DataAnnotations;

namespace whatsapp2api.Models.User
{
    public class UserCreate
    {
        [Required]
        [RegularExpression(@"(\+34|0034|34)?[ -]*(6|7)[ -]*([0-9][ -]*){8}",
            ErrorMessage = "Phone number is incorrect")]
        public string Phone { get; }

        [Required]
        [MinLength(5)]
        [RegularExpression("^[a-zA-Z0-9]+([._]?[a-zA-Z0-9]+)*$",
            ErrorMessage = "Username has illegal characters")]
        public string Username { get; }

        [Required] [MinLength(8)] public string Password { get; }
    }
}