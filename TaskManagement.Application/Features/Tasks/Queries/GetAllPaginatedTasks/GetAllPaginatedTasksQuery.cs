using MediatR;
using TaskManagement.Application.Features.Tasks.Queries.GetTaskById;

namespace TaskManagement.Application.Features.Tasks.Queries.GetAllPaginatedTasks
{
    public record GetAllPaginatedTasksQuery(int PageNumber, int PageSize) : IStreamRequest<GetTaskByIdQueryResponse>;
}
