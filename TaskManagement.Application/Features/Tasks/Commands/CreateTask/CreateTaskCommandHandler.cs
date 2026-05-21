using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Features.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler(ITaskRepository repository, IAppDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITaskQueue taskQueue) : IRequestHandler<CreateTaskCommand, IdResponse<Guid>>
    {
        private readonly ITaskRepository _repository = repository;
        private readonly IAppDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ITaskQueue _taskQueue = taskQueue;

        public async Task<IdResponse<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid extractedId))
                throw new BadRequestException("Invalid user credentials!");

            var task = await TaskEntity.Create(request.Title, request.Description, request.Status, request.Priority, extractedId, _repository);

            if (task is null)
                throw new BadRequestException("Task creation failed!\nThe system can not create duplicate task");

            await _repository.AddTaskAsync(task);

            await _dbContext.SaveChangesAsync();

            await _taskQueue.EnqueueAsync(task, cancellationToken);

            return  new IdResponse<Guid> { Id = task.Id };
        }
    }
}
