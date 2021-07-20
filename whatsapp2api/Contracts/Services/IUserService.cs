using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using whatsapp2api.Models.Auth;
using whatsapp2api.Models.User;

namespace whatsapp2api.Contracts.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAllUsers();
        Task<UserModel?> GetUserById(Guid id);
        Task<UserModel?> GetUserByPhone(string phone);
        Task<UserModel?> CreateUser(UserCreate owner);
        Task<UserModel?> UpdateUser(Guid id, UserUpdate owner);
        Task<UserModel?> DeleteUser(Guid id);
        Task<Tuple<Tokens, string>?> Authenticate(UserAuthenticate owner);
        Task<Tuple<Tokens, string>?> Register(UserCreate owner);
        Task<bool> DoesUserExist(Guid id);
        Task<bool> DoesUserExist(string phone);
    }
}