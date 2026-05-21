using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Presistence.Repositories
{
    public class RefreshTokenRepository(IAppDbContext appDbContext) : IRefreshTokenRepository
    {
        private readonly IAppDbContext _appDbContext = appDbContext;

        public async Task AddRefreshTokenAsync(RefreshTokenEntity refreshToken)
        {
           await _appDbContext.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task<RefreshTokenEntity?> GetRefreshTokenByTokenAsync(string token)
        {
            return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<ICollection<RefreshTokenEntity>> GetRefreshTokensByUserIdAsync(Guid userId)
        {
            return await _appDbContext.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
        }

        public void RemoveRefreshToken(RefreshTokenEntity refreshToken)
        {
            _appDbContext.RefreshTokens.Remove(refreshToken);
        }

        public async Task<int> RemoveRefreshTokensByUserIdAsync(Guid userId)
        {
            return await _appDbContext.RefreshTokens.Where(rt => rt.UserId == userId).ExecuteDeleteAsync();
        }

        public async Task<int> RemoveVulnerableRefreshTokensAsync(string exceptionToken, Guid userId)
        {
            return await _appDbContext.RefreshTokens.Where(rt => rt.UserId == userId && rt.Token != exceptionToken).ExecuteDeleteAsync();
        }
    }
}
