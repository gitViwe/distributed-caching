using StackExchange.Redis;

namespace Shared;

public class MyRedisCache
{
    private readonly IDatabase _database;

    public MyRedisCache(IConnectionMultiplexer multiplexer)
    {
        _database = multiplexer.GetDatabase();
    }

    public async Task<string> GetItem(string key)
    {
        return await _database.StringGetAsync(key);
    }

    public async Task<bool> SetItem(string key, string value)
    {
        return await _database.StringSetAsync(key, value);
    }
}