using MediatR;

namespace TaskManagement.Application.Features.Tasks.Queries.GetTaskById
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<GetTaskByIdQueryResponse?>;
    
}
