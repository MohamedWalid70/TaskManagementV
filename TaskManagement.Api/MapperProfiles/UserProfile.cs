using AutoMapper;
using TaskManagement.Api.Models.Users;
using TaskManagement.Application.Features.Users.Commands.CreateUser;
using TaskManagement.Application.Features.Users.Queries.GetUserById;
using TaskManagement.Application.Features.Users.Queries.GetUserProfile;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Api.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {

            CreateMap<CreateUserCommandParam, CreateUserCommand>();
            CreateMap<UserEntity, GetUserByIdQueryResponse>();
            CreateMap<UserProfileInfo, GetCurrentUserProfileQueryResponse>();
        }
    }
}
