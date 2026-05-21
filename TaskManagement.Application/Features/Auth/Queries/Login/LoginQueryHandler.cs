using MediatR;
using TaskManagement.Application.Features.Auth.Queries.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Auth.Queries.Login
{
    public class LoginQueryHandler(ITokenGenerator tokenGenerator, IRefreshTokenRepository refreshTokenRepository, IAppDbContext dbContext) : IRequestHandler<LoginQuery, AuthQueryResponse>
    {
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        private readonly IAppDbContext _dbContext = dbContext;

        public async Task<AuthQueryResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var accessToken = _tokenGenerator.GenerateJwtToken(request.SharedUser, [request.SharedUser.Role]);

            var refreshToken = _tokenGenerator.GenerateRefreshToken(request.SharedUser.Id);

            await SaveRefreshTokensAsync(refreshToken);

            var authQueryResponseParam = new AuthQueryResponse { AccessToken = accessToken, RefreshToken = refreshToken.Token };

            return authQueryResponseParam;
        }

        private async Task SaveRefreshTokensAsync(RefreshTokenEntity refreshToken)
        {
            await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);

            await _dbContext.SaveChangesAsync();

        }

    }
}
