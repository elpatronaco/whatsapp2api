using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using whatsapp2api.Contracts.Repositories;
using whatsapp2api.Entities;
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

        public Task<List<UserEntity>> GetAllUsers()
        {
            return _context.Users.ToListAsync();
        }

        public async Task<UserEntity?> GetOneByCondition(Expression<Func<UserEntity, bool>> expression)
        {
            return await _context.Users.FirstOrDefaultAsync(expression);
        }

        public async Task<UserEntity> CreateUser(UserCreate owner)
        {
            var entity = new UserEntity(owner.Phone, owner.Username, owner.Password);

            var user = await _context.Users.AddAsync(entity);

            await _context.SaveChangesAsync();

            return user.Entity;
        }

        public async Task<UserEntity> UpdateUser(Guid id, UserUpdate owner)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (owner is not {Username: null}) entity.Username = owner.Username;
            if (owner is not {Password: null}) entity.ModifyPassword(owner.Password);

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<UserEntity> DeleteUser(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}