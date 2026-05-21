using TaskManagement.Common.CustomTypes;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public CustomTaskStatus Status { get; private set; }
        public byte Priority { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid UserId { get; private set; }

        private TaskEntity()
        {
            
        }

        public static async Task<TaskEntity?> Create(string title, string description, CustomTaskStatus status, byte priority, Guid userId, ITaskRepository taskRepository)
        {
            if (await taskRepository.IsTaskDuplicated(title, DateTime.UtcNow, userId))
                return null;

            return new()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Priority = priority,
                Status = status,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void UpdateStatus(CustomTaskStatus status)
        {
            Status = status;
        }
    }
}
