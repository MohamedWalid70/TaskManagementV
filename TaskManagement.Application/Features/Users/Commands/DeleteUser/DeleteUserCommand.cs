using MediatR;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>, IRequestValidation<UserEntity>
    {
        public Guid EntityId { get; set; }
        public UserEntity? SharedObject { get; set; }

    }

}
