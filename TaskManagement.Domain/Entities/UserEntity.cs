using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Domain.Entities
{
    public class UserEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }

        private UserEntity()
        {
            
        }

        public static async Task<UserEntity?> Create(string name, string email, string password, string role, IUserRepository userRepository)
        {
            if (await userRepository.IsEmailDuplicatedAsync(email))
                return null;

            return new UserEntity() { Id = Guid.NewGuid(), Name = name, Email = email.ToUpper(), Password = password, CreatedAt = DateTime.UtcNow, Role = role };
        }

        public void Remove()
        {
            IsDeleted = true;
        }
    }
}
