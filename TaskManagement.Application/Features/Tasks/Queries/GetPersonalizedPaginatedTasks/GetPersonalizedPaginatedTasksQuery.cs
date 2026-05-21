using MediatR;
using TaskManagement.Application.Features.Tasks.Queries.GetTaskById;

namespace TaskManagement.Application.Features.Tasks.Queries.GetPersonalizedPaginatedTasks
{
    public record GetPersonalizedPaginatedTasksQuery(int PageNumber, int PageSize) : IStreamRequest<GetTaskByIdQueryResponse>;
}
