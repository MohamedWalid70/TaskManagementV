using MediatR;
using TaskManagement.Common.CustomTypes;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommand : IRequest<Unit>, IRequestValidation<TaskEntity>
    {
        public Guid EntityId { get; set; }
        public CustomTaskStatus Status { get; set; }
        public TaskEntity? SharedObject { get; set; }

    }
}
