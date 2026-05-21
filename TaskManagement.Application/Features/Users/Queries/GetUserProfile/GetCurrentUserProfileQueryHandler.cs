using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskManagement.Application.Exceptions;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Application.Features.Users.Queries.GetUserProfile
{
    public class GetCurrentUserProfileQueryHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetCurrentUserProfileQuery, GetCurrentUserProfileQueryResponse?>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<GetCurrentUserProfileQueryResponse?> Handle(GetCurrentUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid extractedId))
                throw new BadRequestException("Invalid user credentials!");

            var user = await _userRepository.GetUserProfileByIdAsync(extractedId);

            var getCurrentUserProfileQueryResponse = _mapper.Map<GetCurrentUserProfileQueryResponse?>(user);

            return getCurrentUserProfileQueryResponse;
        }
    }
}
