using MediatR;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(IUserRepository userRepository, IAppDbContext appDbContext, ICacheService cacheService) : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAppDbContext _appDbContext = appDbContext;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _userRepository.RemoveUser(request.SharedObject);

            await _appDbContext.SaveChangesAsync();

            await _cacheService.RemoveAsync($"User-{request.EntityId}", cancellationToken);

            return Unit.Value;
        }
    }
}
