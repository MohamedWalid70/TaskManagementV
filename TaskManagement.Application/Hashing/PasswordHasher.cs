using System.Security.Cryptography;
using System.Text;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Hashing
{
    public class PasswordHasher: IPasswordHasher
    {
        public async Task<string> HashAsync(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                Stream passwordStream = new MemoryStream(bytes);

                byte[] hash = await sha256.ComputeHashAsync(passwordStream);

                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
