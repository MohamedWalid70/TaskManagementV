using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Presistence.Repositories
{
    public class TaskRepository(IAppDbContext appDbContext) : ITaskRepository
    {
        IAppDbContext _appDbContext = appDbContext;

        public async Task AddTaskAsync(TaskEntity task)
        {
            await _appDbContext.Tasks.AddAsync(task);
        }

        public async Task<TaskEntity?> GetTaskByIdAsync(Guid taskId)
        {
            return await _appDbContext.Tasks.FindAsync(taskId);
        }

        public void RemoveTask(TaskEntity task)
        {
            _appDbContext.Tasks.Remove(task); 
        }

        public async Task<bool> DoesTaskExist(Guid taskId)
        {
            return await _appDbContext.Tasks.AnyAsync(task => task.Id == taskId);
        }

        public async Task RemoveTaskByIdAsync(Guid taskId)
        {
            await _appDbContext.Tasks.Where(e => e.Id == taskId).ExecuteDeleteAsync();
        }

        public async Task<bool> IsTaskDuplicated(string title, DateTime creationDateTime, Guid userId)
        {
            return await _appDbContext.Tasks.AnyAsync(task => task.Title == title
                && task.CreatedAt.Date == creationDateTime.Date
                && task.UserId == userId);
        }

        public IAsyncEnumerable<TaskEntity> GetPersonalizedPaginatedTasksAsync(int pageNumber, int pageSize, Guid userId)
        {
            return _appDbContext.Tasks
                .AsNoTracking()
                .Where(task => task.UserId == userId)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .AsAsyncEnumerable();
        }

        public IAsyncEnumerable<TaskEntity> GetAllPaginatedTasksAsync(int pageNumber, int pageSize)
        {
            return _appDbContext.Tasks
                .AsNoTracking()
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .AsAsyncEnumerable();
        }
    }
}
