using MediatR;
using TaskManagement.Application.Features.Users.Queries.GetUserById;

namespace TaskManagement.Application.Features.Users.Queries.GetPaginatedUsers
{
    public record GetPaginatedUsersQuery(int PageNumber, int PageSize) : IStreamRequest<GetUserByIdQueryResponse>;
}
