using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Shared.Option;
using System.Text;
using System.Text.Json;

namespace Shared;

internal class RedisDistributedCache : IRedisDistributedCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly RedisDistributedCacheOption _cacheOption;

    public RedisDistributedCache(IDistributedCache distributedCache, IOptionsMonitor<RedisDistributedCacheOption> options)
    {
        _distributedCache = distributedCache;
        _cacheOption = options.CurrentValue;
    }

    private DistributedCacheEntryOptions CreateCacheEntryOptions(TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null)
    {
        return new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(_cacheOption.AbsoluteExpirationInMinutes),
            SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(_cacheOption.SlidingExpirationInMinutes),
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

        _distributedCache.Set(key, byteValue, CreateCacheEntryOptions(absoluteExpirationRelativeToNow, slidingExpiration));
    }

    public Task SetAsync<TValue>(string key, TValue value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, CancellationToken token = default)
    {
        var stringValue = JsonSerializer.Serialize(value);

        var byteValue = Encoding.UTF8.GetBytes(stringValue);

        return _distributedCache.SetAsync(key, byteValue, CreateCacheEntryOptions(absoluteExpirationRelativeToNow, slidingExpiration), token);
    }
}