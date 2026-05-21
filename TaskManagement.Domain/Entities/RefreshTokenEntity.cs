namespace TaskManagement.Domain.Entities
{
    public class RefreshTokenEntity
    {
        public Guid Id { get; private set; }
        public string Token { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime ExpiryDateUtc { get; private set; }

        private RefreshTokenEntity()
        {
            
        }

        public static RefreshTokenEntity Create(string token, Guid userId, DateTime expiryDateTime)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Token = token,
                ExpiryDateUtc = expiryDateTime,
                UserId = userId,
            };
        }

        public void Update(string token, DateTime expiryDataTime)
        {
            Token = token;
            ExpiryDateUtc = expiryDataTime;
        }
    }
}
