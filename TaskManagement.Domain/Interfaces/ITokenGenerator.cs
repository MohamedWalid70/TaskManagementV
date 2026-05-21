using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateJwtToken(UserEntity user, IList<string> userRoles);
        RefreshTokenEntity GenerateRefreshToken(Guid userId);
    }
}
