using MediatR;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskStatusCommandHandler(IAppDbContext dbContext, ICacheService cacheService) : IRequestHandler<UpdateTaskStatusCommand, Unit>
    {
        private readonly IAppDbContext _dbContext = dbContext;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Unit> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            request.SharedObject?.UpdateStatus(request.Status);

            await _dbContext.SaveChangesAsync();

            await _cacheService.RemoveAsync($"Task-{request.EntityId}", cancellationToken);

            return Unit.Value;
        }
    }
}
