using AutoMapper;
using MediatR;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdHandler(ITaskRepository repository, IMapper mapper, ICacheService cacheService) : IRequestHandler<GetTaskByIdQuery, GetTaskByIdQueryResponse?>
    {
        private readonly ITaskRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<GetTaskByIdQueryResponse?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            TaskEntity? task = await _cacheService.GetAsync<TaskEntity>($"Task-{request.Id}", cancellationToken);

            if (task is null)
            {

                task = await _repository.GetTaskByIdAsync(request.Id);

                if(task is null)
                    return null;

                await _cacheService.SetAsync($"Task-{request.Id}", task, cancellationToken);

            }

            var getTaskByIdQueryResponse = _mapper.Map<GetTaskByIdQueryResponse?>(task);

            return getTaskByIdQueryResponse;
        }
    }
}
