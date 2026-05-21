using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Serialization;

namespace TaskManagement.Infrastructure.Caching
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly JsonSerializerOptions _privateJsonSerializerOptions = new() { TypeInfoResolver = new PrivateMemberResolver() };

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (cachedValue is null)
                return null;

            T? deserializedValue = JsonSerializer.Deserialize<T>(cachedValue, _privateJsonSerializerOptions);

            return deserializedValue;
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            string cacheString = JsonSerializer.Serialize(value);

            await _distributedCache.SetStringAsync(key, cacheString, cancellationToken);
        }
    }
}
