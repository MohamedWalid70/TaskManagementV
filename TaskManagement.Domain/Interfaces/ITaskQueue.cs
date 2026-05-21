using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskQueue
    {
        Task EnqueueAsync(TaskEntity task, CancellationToken ct = default);
        Task<TaskEntity> DequeueAsync(CancellationToken ct);
    }
}
