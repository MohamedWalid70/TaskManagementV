using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Infrastructure.Presistence.Repositories
{
    public class UserRepository(IAppDbContext appDbContext) : IUserRepository
    {
        IAppDbContext _appDbContext = appDbContext;

        public async Task AddUserAsync(UserEntity user)
        {
            await _appDbContext.Users.AddAsync(user);
        }

        public void RemoveUser(UserEntity user)
        {
            user.Remove();
        }

        public IAsyncEnumerable<UserEntity> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            return _appDbContext.Users
                .AsNoTracking()
                .Where(user => !user.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .AsAsyncEnumerable();
        }

        public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
        {
            return await _appDbContext.Users.Where(user => !user.IsDeleted).SingleOrDefaultAsync(user => user.Id == userId);
        }

        public async Task<bool> IsEmailDuplicatedAsync(string email)
        {
            return await _appDbContext.Users.AnyAsync(user => user.Email == email && !user.IsDeleted);
        }

        public Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return _appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email && !user.IsDeleted);
        }

        public Task<UserProfileInfo?> GetUserProfileByIdAsync(Guid userId)
        {
            return _appDbContext.Users.Where(user => user.Id == userId).Select(user => new UserProfileInfo { Name = user.Name, Email = user.Email }).SingleOrDefaultAsync();
        }
    }
}
