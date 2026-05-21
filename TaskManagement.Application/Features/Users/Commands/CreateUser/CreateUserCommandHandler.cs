using MediatR;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Features.Common;
using TaskManagement.Application.Hashing;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler(IUserRepository userRepository, IAppDbContext appDbContext, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, IdResponse<Guid>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAppDbContext _appDbContext = appDbContext;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<IdResponse<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = await _passwordHasher.HashAsync(request.Password);

            var userEntity = await UserEntity.Create(request.Name, request.Email, hashedPassword, "User", _userRepository);

            if (userEntity is null)
                throw new BadRequestException("A user with the same email already exists!");

            await _userRepository.AddUserAsync(userEntity);

            await _appDbContext.SaveChangesAsync();
                
            return new IdResponse<Guid> { Id = userEntity.Id };
        }

    }
}
