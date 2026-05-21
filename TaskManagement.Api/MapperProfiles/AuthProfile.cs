using AutoMapper;
using TaskManagement.Api.Models.Auth;
using TaskManagement.Application.Features.Auth.Queries.Login;

namespace TaskManagement.Api.MapperProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginQueryParam, LoginQuery>();
        }
    }
}
