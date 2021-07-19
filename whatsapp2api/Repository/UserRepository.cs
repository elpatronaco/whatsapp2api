using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using whatsapp2api.Contracts;
using whatsapp2api.Models;
using whatsapp2api.Models.Context;
using whatsapp2api.Models.User;

namespace whatsapp2api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly RepositoryContext _context;

        public UserRepository(RepositoryContext context)
        {
            _context = context;
        }

        public UserModel MapToDto(UserEntity owner)
        {
            return new() {Id = owner.Id, Phone = owner.Phone, Username = owner.Username};
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var entities = await _context.Users.ToListAsync();

            return entities.Select(x => MapToDto(x));
        }

        public async Task<UserModel?> GetUserById(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<UserModel> CreateUser(UserCreate owner)
        {
            var entity = new UserEntity(owner.Phone, owner.Username, owner.Password);

            var user = await _context.Users.AddAsync(entity);

            await _context.SaveChangesAsync();

            return MapToDto(user.Entity);
        }

        public async Task<UserModel> UpdateUser(Guid id, UserUpdate owner)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (owner is not {Username: null}) entity.Username = owner.Username;
            if (owner is not {Password: null}) entity.ModifyPassword(owner.Password);

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<UserModel> DeleteUser(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
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