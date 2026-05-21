using MediatR;
using TaskManagement.Application.Features.Common;
using TaskManagement.Common.CustomTypes;

namespace TaskManagement.Application.Features.Tasks.Commands.CreateTask
{
    public record CreateTaskCommand : IRequest<IdResponse<Guid>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public CustomTaskStatus Status { get; set; }
        public byte Priority { get; set; }
    }
}

