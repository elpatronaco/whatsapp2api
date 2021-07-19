using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using whatsapp2api.Helpers;

namespace whatsapp2api.Models
{
    [Table("users"), Index(nameof(Phone), IsUnique = true)]
    public class UserEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }

        [Required, RegularExpression(@"(\+34|0034|34)?[ -]*(6|7)[ -]*([0-9][ -]*){8}",
             ErrorMessage = "Phone number is incorrect")]
        public string Phone { get; }

        [Required, MinLength(5)] public string Username { get; set; }

        private readonly byte[] PasswordSalt;
        private byte[] PasswordHash;

        public UserEntity(string phoneNumber, string username, string password)
        {
            Phone = phoneNumber;
            Username = username;

            PasswordSalt = Crypto.Salt();
            PasswordHash = Crypto.Hash(password, PasswordSalt);
        }

        public void ModifyPassword(string newPassword)
        {
            PasswordHash = Crypto.Hash(newPassword, PasswordSalt);
        }

        public bool ValidatePassword(string password)
        {
            var hash = Crypto.Hash(password, PasswordSalt);

            return hash == PasswordHash;
        }
    }
}