using Microsoft.EntityFrameworkCore;
using whatsapp2api.Entities;

namespace whatsapp2api.Models.Context
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}