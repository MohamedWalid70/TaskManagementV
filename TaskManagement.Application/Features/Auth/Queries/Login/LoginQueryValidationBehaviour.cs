using MediatR;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Features.Auth.Queries.Common;
using TaskManagement.Application.Hashing;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Application.Features.Auth.Queries.Login
{
    public class LoginQueryValidationBehaviour(IUserRepository userRepository, IPasswordHasher passwordHasher) : IPipelineBehavior<LoginQuery, AuthQueryResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<AuthQueryResponse> Handle(LoginQuery request, RequestHandlerDelegate<AuthQueryResponse> next, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user is null)
                throw new BadRequestException("Invalid email or password!");

            var passwordComparisonResult = user.Password == await _passwordHasher.HashAsync(request.Password);

            if (!passwordComparisonResult)
                throw new BadRequestException("Invalid username or password");

            request.SharedUser = user;

            var response = await next();

            return response;
        }
    }
}
