using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public Task SaveChangesAsync();

    }
}
