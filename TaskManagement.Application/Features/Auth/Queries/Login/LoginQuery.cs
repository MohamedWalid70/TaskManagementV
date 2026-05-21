using MediatR;
using TaskManagement.Application.Features.Auth.Queries.Common;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Features.Auth.Queries.Login
{
    public class LoginQuery: IRequest<AuthQueryResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserEntity SharedUser { get; set; }
    }
}
