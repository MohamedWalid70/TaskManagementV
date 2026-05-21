namespace TaskManagement.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        Task<string> HashAsync(string password);
    }
}
