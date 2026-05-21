using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task AddTaskAsync(TaskEntity task);
        void RemoveTask(TaskEntity task);
        IAsyncEnumerable<TaskEntity> GetPersonalizedPaginatedTasksAsync(int pageNumber, int pageSize, Guid userId);
        IAsyncEnumerable<TaskEntity> GetAllPaginatedTasksAsync(int pageNumber, int pageSize);
        Task<TaskEntity?> GetTaskByIdAsync(Guid taskId);
        Task<bool> DoesTaskExist(Guid taskId);
        Task RemoveTaskByIdAsync(Guid taskId);
        Task<bool> IsTaskDuplicated(string title, DateTime CreationDateTime, Guid userId);
    }
}
