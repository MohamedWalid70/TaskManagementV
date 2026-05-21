using MediatR;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Unit>, IRequestValidation<TaskEntity>
    {
        public Guid EntityId { get; set; }
        public TaskEntity? SharedObject { get; set; }

    }

}
