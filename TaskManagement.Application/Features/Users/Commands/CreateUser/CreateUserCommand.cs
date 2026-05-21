using MediatR;
using TaskManagement.Application.Features.Common;

namespace TaskManagement.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<IdResponse<Guid>>
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}
