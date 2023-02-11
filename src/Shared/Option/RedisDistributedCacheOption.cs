using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Shared.Option;

public class RedisDistributedCacheOption : RedisCacheOptions
{
    /// <summary>
    /// Gets or sets an absolute expiration time, relative to now.
    /// </summary>
    public int AbsoluteExpirationInMinutes { get; set; } = 1;
    /// <summary>
    /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
    /// This will not extend the entry lifetime beyond the absolute expiration (if set).
    /// </summary>
    public int SlidingExpirationInMinutes { get; set; } = 1;
}
