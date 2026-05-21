using MediatR;
using TaskManagement.Application.Exceptions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Application.Features.Users.Commands
{
    public class ValidateUserExistenceBehavoiur<TRequest, TResponse>(IUserRepository repository) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IRequestValidation<UserEntity>
    {
        private readonly IUserRepository _repository = repository;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByIdAsync(request.EntityId);

            if (user == null)
            {
                throw new NotFoundException("The specified id doesn't match any entry");
            }

            request.SharedObject = user;

            var response = await next();

            return response;
        }
    }
}

