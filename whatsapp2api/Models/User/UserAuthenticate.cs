using System.ComponentModel.DataAnnotations;

namespace whatsapp2api.Models.User
{
    public class UserAuthenticate
    {
        [Required]
        [RegularExpression(@"(\+34|0034|34)?[ -]*(6|7)[ -]*([0-9][ -]*){8}",
            ErrorMessage = "Phone number is incorrect")]
        public string Phone { get; }

        [Required] [MinLength(8)] public string Password { get; set; }
    }
}