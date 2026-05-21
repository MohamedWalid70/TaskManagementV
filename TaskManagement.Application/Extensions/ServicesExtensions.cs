using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagement.Application.Features.Auth.Queries.Login;
using TaskManagement.Application.Features.Auth.Queries.RefreshToken;
using TaskManagement.Application.Features.Tasks.Commands;
using TaskManagement.Application.Features.Tasks.Commands.DeleteTask;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Application.Features.Users.Commands;
using TaskManagement.Application.Features.Users.Commands.DeleteUser;
using TaskManagement.Application.Hashing;
using TaskManagement.Application.Tokens;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Extensions
{
    public static class ServicesExtensions
    {
        public static void ResisterApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddMediatR(
                config => {
                    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                    config.AddBehavior<ValidateTaskExistenceBehavoiur<UpdateTaskStatusCommand, Unit>>();
                    config.AddBehavior<ValidateTaskExistenceBehavoiur<DeleteTaskCommand, Unit>>();
                    config.AddBehavior<LoginQueryValidationBehaviour>();
                    config.AddBehavior<ValidateRefreshTokenBehaviour>();
                    config.AddBehavior<ValidateUserExistenceBehavoiur<DeleteUserCommand, Unit>>();
                });

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}
