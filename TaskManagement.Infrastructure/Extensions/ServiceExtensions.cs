using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;
using TaskManagement.Infrastructure.BackgroundServices;
using TaskManagement.Infrastructure.BackgroundServices.TaskProcessing;
using TaskManagement.Infrastructure.Caching;
using TaskManagement.Infrastructure.Presistence.Data;
using TaskManagement.Infrastructure.Presistence.Repositories;

namespace TaskManagement.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterInfrastructureServices(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

            builder.Services.AddScoped<IAppDbContext, AppDbContext>();

            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();


            builder.Services.AddHostedService<SeedingService>();

            builder.Services.AddSingleton<ITaskQueue, TaskQueue>();
            builder.Services.AddHostedService<TaskProcessingService>();

            builder.Services.AddStackExchangeRedisCache(redisOptions => redisOptions.Configuration = builder.Configuration.GetConnectionString("Redis"));

            builder.Services.AddSingleton<ICacheService, CacheService>();

        }
    }
}
