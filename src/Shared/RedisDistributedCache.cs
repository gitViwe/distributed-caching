using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared
{
    public class RedisDistributedCache : IRedisDistributedCache
    {
        private readonly IDistributedCache _distributedCache;

        public RedisDistributedCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        private static DistributedCacheEntryOptions CreateCacheEntryOptions(TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null)
        {
            return new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(5),
                SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(2),
            };
        }

        public TResult? Get<TResult>(string key) where TResult : class, new()
        {
            var byteValue = _distributedCache.Get(key);

            if (byteValue is null)
            {
                return null;
            }

            var stringValue = Encoding.UTF8.GetString(byteValue);
            var data = JsonSerializer.Deserialize<TResult>(stringValue);

            return data;
        }

        public string? Get(string key)
        {
            var byteValue = _distributedCache.Get(key);

            if (byteValue is null)
            {
                return null;
            }

            return Encoding.UTF8.GetString(byteValue);
        }

        public async Task<TResult?> GetAsync<TResult>(string key, CancellationToken token = default) where TResult : class, new()
        {
            var byteValue = await _distributedCache.GetAsync(key, token);

            if (byteValue is null)
            {
                return null;
            }

            var stringValue = Encoding.UTF8.GetString(byteValue);
            var data = JsonSerializer.Deserialize<TResult>(stringValue);

            return data;
        }

        public async Task<string?> GetAsync(string key, CancellationToken token = default)
        {
            var byteValue = await _distributedCache.GetAsync(key, token);

            if (byteValue is null)
            {
                return null;
            }

            return Encoding.UTF8.GetString(byteValue);
        }

        public void Refresh(string key)
        {
            _distributedCache.Refresh(key);
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            return _distributedCache.RefreshAsync(key, token);
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            return _distributedCache.RemoveAsync(key, token);
        }

        public void Set<TValue>(string key, TValue value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null)
        {
            var stringValue = JsonSerializer.Serialize(value);

            var byteValue = Encoding.UTF8.GetBytes(stringValue);

            var cacheOptions = CreateCacheEntryOptions(absoluteExpirationRelativeToNow, slidingExpiration);

            _distributedCache.Set(key, byteValue, cacheOptions);
        }

        public Task SetAsync<TValue>(string key, TValue value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, CancellationToken token = default)
        {
            var stringValue = JsonSerializer.Serialize(value);

            var byteValue = Encoding.UTF8.GetBytes(stringValue);

            var cacheOptions = CreateCacheEntryOptions(absoluteExpirationRelativeToNow, slidingExpiration);

            return _distributedCache.SetAsync(key, byteValue, cacheOptions, token);
        }
    }
}