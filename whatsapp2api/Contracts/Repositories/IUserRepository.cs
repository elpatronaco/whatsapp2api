using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using whatsapp2api.Entities;
using whatsapp2api.Models.User;

namespace whatsapp2api.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAllUsers();
        Task<UserEntity?> GetOneByCondition(Expression<Func<UserEntity, bool>> expression);
        Task<UserEntity> CreateUser(UserCreate owner);
        Task<UserEntity> UpdateUser(Guid id, UserUpdate owner);
        Task<UserEntity> DeleteUser(Guid id);
    }
}