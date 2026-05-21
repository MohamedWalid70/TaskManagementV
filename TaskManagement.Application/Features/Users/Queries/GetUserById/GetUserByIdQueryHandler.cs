using AutoMapper;
using MediatR;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper, ICacheService cacheService) : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResponse?>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<GetUserByIdQueryResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            UserEntity? user = await _cacheService.GetAsync<UserEntity>($"User-{request.Id}", cancellationToken);

            if (user is null)
            {

                user = await _userRepository.GetUserByIdAsync(request.Id);

                if (user is null)
                    return null;

                await _cacheService.SetAsync($"User-{request.Id}", user, cancellationToken);

            }

            var userByIdQueryParam = _mapper.Map<GetUserByIdQueryResponse?>(user);

            return userByIdQueryParam;
        }
    }
}
