using MediatR;
using TaskManagement.Application.Features.Auth.Queries.Common;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Features.Auth.Queries.RefreshToken
{
    public class RefreshTokenQuery : IRequest<AuthQueryResponse>
    {
        public string RefreshToken { get; set; }
        public RefreshTokenEntity SharedTokenEntity { get; set; }
    }
}
