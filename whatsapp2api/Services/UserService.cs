using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using whatsapp2api.Contracts.Repositories;
using whatsapp2api.Contracts.Services;
using whatsapp2api.Models.Context;
using whatsapp2api.Models.User;

namespace whatsapp2api.Services
{
    public class UserService : IUserService
    {
        private readonly RepositoryContext _context;
        private readonly IUserRepository _repo;

        public UserService(RepositoryContext context, IUserRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = await _repo.GetAllUsers();

            return users.Select(x => x.ToDto());
        }

        public async Task<UserModel?> GetUserById(Guid id)
        {
            var entity = await _repo.GetOneByCondition(x => x.Id == id);

            return entity.ToDto();
        }

        public async Task<UserModel?> GetUserByPhone(string phone)
        {
            var entity = await _repo.GetOneByCondition(x => x.Phone == phone);

            return entity.ToDto();
        }

        public async Task<UserModel?> CreateUser(UserCreate owner)
        {
            var isUserExist = await DoesUserExist(owner.Phone);

            if (isUserExist) return null;

            var entity = await _repo.CreateUser(owner);

            return entity.ToDto();
        }

        public async Task<UserModel?> UpdateUser(Guid id, UserUpdate owner)
        {
            var isUserExist = await DoesUserExist(id);

            if (isUserExist) return null;

            var entity = await _repo.UpdateUser(id, owner);

            return entity.ToDto();
        }

        public async Task<UserModel?> DeleteUser(Guid id)
        {
            var isUserExist = await DoesUserExist(id);

            if (isUserExist) return null;

            var entity = await _repo.DeleteUser(id);

            return entity.ToDto();
        }

        public async Task<bool> Authenticate(UserAuthenticate owner)
        {
            var isUserExist = await DoesUserExist(owner.Phone);

            if (isUserExist) return false;

            var entity = await _repo.GetOneByCondition(x => x.Phone == owner.Phone);

            return entity.ValidatePassword(owner.Password);
        }

        public Task<bool> DoesUserExist(Guid id)
        {
            return _context.Users.AnyAsync(x => x.Id == id);
        }

        public Task<bool> DoesUserExist(string phone)
        {
            return _context.Users.AnyAsync(x => x.Phone == phone);
        }
    }
}