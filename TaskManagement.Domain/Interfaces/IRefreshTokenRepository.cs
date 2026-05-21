using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddRefreshTokenAsync(RefreshTokenEntity refreshToken);
        void RemoveRefreshToken(RefreshTokenEntity refreshToken);
        Task<ICollection<RefreshTokenEntity>> GetRefreshTokensByUserIdAsync(Guid userId);
        Task<RefreshTokenEntity?> GetRefreshTokenByTokenAsync(string token);
        Task<int> RemoveRefreshTokensByUserIdAsync(Guid userId);
        Task<int> RemoveVulnerableRefreshTokensAsync(string exceptionToken, Guid userId);

    }
}
