using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if(Guid.TryParse(userIdClaim?.Value, out Guid userId))
                await _refreshTokenRepository.RemoveRefreshTokensByUserIdAsync(userId);
            

            return Unit.Value;
        }
    }
}
