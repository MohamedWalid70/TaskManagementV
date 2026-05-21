using MediatR;

namespace TaskManagement.Application.Features.Users.Queries.GetUserProfile
{
    public record GetCurrentUserProfileQuery: IRequest<GetCurrentUserProfileQueryResponse?>;
}
