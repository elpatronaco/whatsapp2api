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
        Task<IEnumerable<UserModel>> GetUsersMinusCaller(Guid senderId);
        Task<UserModel?> GetUserById(Guid id);
        Task<UserModel?> GetUserByPhone(string phone);
        Task<Guid?> GetUserIdByConnectionId(string connectionId);
        Task<UserModel?> CreateUser(UserCreate owner);
        Task<UserModel?> UpdateUser(Guid id, UserUpdate owner);
        Task<UserModel?> DeleteUser(Guid id);
        Task<Tuple<Tokens, string>?> Authenticate(UserAuthenticate owner);
        Task<Tuple<Tokens, string>?> Register(UserCreate owner);
        Task<bool> DoesUserExist(Guid id);
        Task<bool> DoesUserExist(string phone);
        Task<UserModel?> UserFromToken(string idToken);
        Task SetUserConnectionId(Guid user, string connectionId);
        Task RemoveUserConnectionId(string connectionId);
    }
}