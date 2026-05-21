using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Common.CustomTypes;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.BackgroundServices.TaskProcessing
{
    public class TaskProcessingService(ITaskQueue taskQueue, IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly ITaskQueue _taskQueue = taskQueue;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await _taskQueue.DequeueAsync(stoppingToken);

                var updateTaskStatusCommand = new UpdateTaskStatusCommand() { SharedObject = task , Status = CustomTaskStatus.InProgress, EntityId = task.Id };

                using var scope = _serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                await sender.Send(updateTaskStatusCommand, stoppingToken);

                Log.Information($"Processing a task with an id of {task.Id}");

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

                updateTaskStatusCommand.Status = CustomTaskStatus.Done;

                Log.Information($"Finished task processing. Task of id {task.Id} is done");

                await sender.Send(updateTaskStatusCommand, stoppingToken);
            }
        }
    }
}
