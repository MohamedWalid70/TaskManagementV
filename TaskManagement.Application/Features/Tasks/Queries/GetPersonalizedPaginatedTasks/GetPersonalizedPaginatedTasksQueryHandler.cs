using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Features.Tasks.Queries.GetTaskById;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Queries.GetPersonalizedPaginatedTasks
{
    public class GetPersonalizedPaginatedTasksQueryHandler(ITaskRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : IStreamRequestHandler<GetPersonalizedPaginatedTasksQuery, GetTaskByIdQueryResponse>
    {
        private readonly ITaskRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async IAsyncEnumerable<GetTaskByIdQueryResponse> Handle(GetPersonalizedPaginatedTasksQuery request, [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            if (request.PageNumber > 0 && request.PageSize > 0)
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(userId, out Guid extractedId))
                    throw new BadRequestException("Invalid user credentials!");

                var tasks = _repository.GetPersonalizedPaginatedTasksAsync(request.PageNumber - 1, request.PageSize, extractedId);

                await foreach (var item in tasks)
                {
                    yield return _mapper.Map<GetTaskByIdQueryResponse>(item);
                }
            }
        }
    }
}
