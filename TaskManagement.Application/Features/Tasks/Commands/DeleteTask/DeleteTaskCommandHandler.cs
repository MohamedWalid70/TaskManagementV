using MediatR;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler(ITaskRepository repository, ICacheService cacheService) : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskRepository _repository = repository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            await _repository.RemoveTaskByIdAsync(request.EntityId);

            await _cacheService.RemoveAsync($"Task-{request.EntityId}", cancellationToken);

            return Unit.Value;
        }
    }
}
