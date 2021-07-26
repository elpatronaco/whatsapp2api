using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using whatsapp2api.Contracts.Repositories;
using whatsapp2api.Entities;
using whatsapp2api.Models.Context;
using whatsapp2api.Models.Message;

namespace whatsapp2api.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly RepositoryContext _context;

        public MessageRepository(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<List<MessageEntity>> GetByCondition(Expression<Func<MessageEntity, bool>> expression)
        {
            return await _context.Messages
                .Where(expression)
                .Include(x => x.Sender)
                .Include(x => x.Recipient)
                .ToListAsync();
        }

        public async Task<MessageEntity?> GetOneByCondition(Expression<Func<MessageEntity, bool>> expression)
        {
            return await _context.Messages
                .Include(x => x.Sender)
                .Include(x => x.Recipient)
                .FirstOrDefaultAsync(expression);
        }

        public async Task<MessageEntity> CreateMessage(Guid senderId, MessageCreate owner)
        {
            var entity = new MessageEntity(senderId, owner.RecipientId, owner.Content);

            var message = await _context.Messages.AddAsync(entity);

            await _context.SaveChangesAsync();

            return message.Entity;
        }
    }
}