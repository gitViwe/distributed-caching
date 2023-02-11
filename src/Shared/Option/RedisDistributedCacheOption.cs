namespace Shared.Option;

internal class RedisDistributedCacheOption
{
    public int AbsoluteExpirationInMinutes { get; set; } = 1;
    public int SlidingExpirationInMinutes { get; set; } = 1;
}
