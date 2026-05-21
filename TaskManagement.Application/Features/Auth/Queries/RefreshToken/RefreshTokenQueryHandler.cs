using MediatR;
using TaskManagement.Application.Features.Auth.Queries.Common;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Application.Features.Auth.Queries.RefreshToken
{
    public class RefreshTokenQueryHandler(ITokenGenerator tokenGenerator, IUserRepository userRepository, IAppDbContext dbContext) : IRequestHandler<RefreshTokenQuery, AuthQueryResponse>
    {
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAppDbContext _dbContext = dbContext;

        public async Task<AuthQueryResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.SharedTokenEntity.UserId);

            var accessToken = _tokenGenerator.GenerateJwtToken(user, [user.Role]);

            var refreshToken = _tokenGenerator.GenerateRefreshToken(user.Id);

            await SaveRefreshTokensAsync(refreshToken.Token, request);

            var authQueryResponseParam = new AuthQueryResponse { AccessToken = accessToken, RefreshToken = refreshToken.Token };

            return authQueryResponseParam;
        }
        private async Task SaveRefreshTokensAsync(string refreshToken, RefreshTokenQuery request)
        {
            request.SharedTokenEntity.Update(token: refreshToken, expiryDataTime: DateTime.UtcNow.AddDays(1));

            await _dbContext.SaveChangesAsync();

        }
    }
}
