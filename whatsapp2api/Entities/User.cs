using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using whatsapp2api.Helpers;
using whatsapp2api.Models.User;

namespace whatsapp2api.Entities
{
    [Table("users"), Index(nameof(Phone), IsUnique = true)]
    public class UserEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, RegularExpression(@"(\+34|0034|34)?[ -]*(6|7)[ -]*([0-9][ -]*){8}",
             ErrorMessage = "Phone number is incorrect")]
        public string Phone { get; set; }

        [Required, MinLength(5)] public string Username { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public byte[]? PasswordHash { get; set; }

        public UserEntity()
        {
        }

        public UserEntity(string phone, string username, string? password = null)
        {
            Phone = phone;
            Username = username;

            if (password != null) ModifyPassword(password);
        }

        public void ModifyPassword(string newPassword)
        {
            PasswordSalt ??= Crypto.Salt();
            PasswordHash = Crypto.Hash(newPassword, PasswordSalt);
        }

        public bool ValidatePassword(string password)
        {
            if (PasswordSalt == null) return false;

            var hash = Crypto.Hash(password, PasswordSalt);

            return hash == PasswordHash;
        }

        public UserModel ToDto()
        {
            return new() {Id = Id, Phone = Phone, Username = Username};
        }
    }
}