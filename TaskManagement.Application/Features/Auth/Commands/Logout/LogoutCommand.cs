using MediatR;

namespace TaskManagement.Application.Features.Auth.Commands.Logout
{
    public record LogoutCommand() : IRequest<Unit>;
}
