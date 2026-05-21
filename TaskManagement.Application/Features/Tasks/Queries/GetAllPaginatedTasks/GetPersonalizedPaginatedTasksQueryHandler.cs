using AutoMapper;
using MediatR;
using System.Runtime.CompilerServices;
using TaskManagement.Application.Features.Tasks.Queries.GetTaskById;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Queries.GetAllPaginatedTasks
{
    public class GetAllPaginatedTasksQueryHandler(ITaskRepository repository, IMapper mapper) : IStreamRequestHandler<GetAllPaginatedTasksQuery, GetTaskByIdQueryResponse>
    {
        private readonly ITaskRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async IAsyncEnumerable<GetTaskByIdQueryResponse> Handle(GetAllPaginatedTasksQuery request, [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            if (request.PageNumber > 0 && request.PageSize > 0)
            {
                var tasks = _repository.GetAllPaginatedTasksAsync(request.PageNumber - 1, request.PageSize);

                await foreach (var item in tasks)
                {
                    yield return _mapper.Map<GetTaskByIdQueryResponse>(item);
                }
            }
        }
    }
}
