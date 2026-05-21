using MediatR;
using TaskManagement.Application.Exceptions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Commands
{
    public class ValidateTaskExistenceBehavoiur<TRequest, TResponse>(ITaskRepository repository) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IRequestValidation<TaskEntity>
    {
        private readonly ITaskRepository _repository = repository;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var task = await _repository.GetTaskByIdAsync(request.EntityId);

            if (task == null)
            {
                throw new NotFoundException("The specified id doesn't match any entry");
            }

            request.SharedObject = task;

            var response = await next();

            return response;
        }
    }
}

