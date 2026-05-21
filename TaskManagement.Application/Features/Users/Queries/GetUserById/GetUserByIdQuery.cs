using MediatR;

namespace TaskManagement.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid Id) : IRequest<GetUserByIdQueryResponse?>;
}
