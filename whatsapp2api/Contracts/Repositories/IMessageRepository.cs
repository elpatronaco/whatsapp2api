using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using whatsapp2api.Entities;
using whatsapp2api.Models.Message;

namespace whatsapp2api.Contracts.Repositories
{
    public interface IMessageRepository
    {
        
        Task<List<MessageEntity>> GetByCondition(Expression<Func<MessageEntity, bool>> expression);
        Task<MessageEntity?> GetOneByCondition(Expression<Func<MessageEntity, bool>> expression);
        Task<MessageEntity> CreateMessage(Guid senderId, MessageCreate owner);
    }
}