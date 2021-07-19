using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using whatsapp2api.Entities;
using whatsapp2api.Models;
using whatsapp2api.Models.User;

namespace whatsapp2api.Contracts
{
    public interface IUserRepository
    {
        UserModel MapToDto(UserEntity owner);
        Task<IEnumerable<UserModel>> GetAllUsers();
        Task<UserModel?> GetUserById(Guid id);
        Task<UserModel> CreateUser(UserCreate owner);
        Task<UserModel> UpdateUser(Guid id, UserUpdate owner);
        Task<UserModel> DeleteUser(Guid id);
        Task<bool> DoesUserExist(Guid id);
        Task<bool> DoesUserExist(string phone);
    }
}