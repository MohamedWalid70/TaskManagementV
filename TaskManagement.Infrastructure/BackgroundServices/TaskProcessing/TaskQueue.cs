using System.Threading.Channels;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.BackgroundServices.TaskProcessing
{
    public class TaskQueue : ITaskQueue
    {
        private readonly Channel<TaskEntity> _channel = Channel.CreateUnbounded<TaskEntity>();

        public async Task<TaskEntity> DequeueAsync(CancellationToken ct)
        {
            return await _channel.Reader.ReadAsync(ct);
        }

        public async Task EnqueueAsync(TaskEntity task, CancellationToken ct = default)
        {
            await _channel.Writer.WriteAsync(task, ct);
        }
    }
}
