using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using whatsapp2api.Entities;

namespace whatsapp2api.Contracts.Repositories
{
    public interface IMessageRepository
    {
        
        Task<List<MessageEntity>> GetByCondition(Expression<Func<MessageEntity, bool>> expression);

        Task<MessageEntity?> GetOneByCondition(Expression<Func<MessageEntity, bool>> expression);
        // Task<UserEntity> CreateMessage(UserCreate owner);
    }
}