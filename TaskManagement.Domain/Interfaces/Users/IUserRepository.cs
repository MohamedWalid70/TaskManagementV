using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces.Users
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserEntity user);
        void RemoveUser(UserEntity user);
        IAsyncEnumerable<UserEntity> GetPaginatedUsersAsync(int pageNumber, int pageSize);
        Task<UserEntity?> GetUserByIdAsync(Guid userId);
        Task<bool> IsEmailDuplicatedAsync(string email);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<UserProfileInfo?> GetUserProfileByIdAsync(Guid userId);
    }
}
