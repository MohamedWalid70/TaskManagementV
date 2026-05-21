using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Infrastructure.BackgroundServices
{
    public class SeedingService(IServiceProvider serviceProvider) : IHostedService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateAsyncScope();

            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var appDbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            var passwordHash = await passwordHasher.HashAsync("Admin@123");

            var adminUser = await UserEntity.Create("AdminExample", "admin@example.com", passwordHash, "Admin", userRepository);

            if (adminUser is null)
                return;

            await userRepository.AddUserAsync(adminUser);

            await appDbContext.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
