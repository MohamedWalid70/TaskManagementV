using AutoMapper;
using MediatR;
using System.Runtime.CompilerServices;
using TaskManagement.Application.Features.Users.Queries.GetUserById;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Application.Features.Users.Queries.GetPaginatedUsers
{
    public class GetPaginatedUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IStreamRequestHandler<GetPaginatedUsersQuery, GetUserByIdQueryResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        public async IAsyncEnumerable<GetUserByIdQueryResponse> Handle(GetPaginatedUsersQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (request.PageNumber > 0 && request.PageSize > 0)
            {
                var users = _userRepository.GetPaginatedUsersAsync(request.PageNumber - 1, request.PageSize);

                await foreach (var item in users)
                {
                    yield return _mapper.Map<GetUserByIdQueryResponse>(item);
                }
            }
        }
    }
}
