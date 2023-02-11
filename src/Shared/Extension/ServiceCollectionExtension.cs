using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Option;

namespace Shared.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<RedisDistributedCacheOption>(configuration.GetSection(nameof(RedisDistributedCacheOption)))
            .AddTransient<IRedisDistributedCache, RedisDistributedCache>()
            .AddSingleton<RedisDistributedCacheOption>()
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = configuration["RedisDistributedCacheOption:InstanceName"];
            });
    }

    public static IServiceCollection RegisterRedisCache(this IServiceCollection services, Action<RedisDistributedCacheOption> options)
    {
        var optionValue = new RedisDistributedCacheOption();
        options(optionValue);

        return services.Configure(options)
            .AddTransient<IRedisDistributedCache, RedisDistributedCache>()
            .AddSingleton<RedisDistributedCacheOption>()
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = optionValue.Configuration;
                options.InstanceName = optionValue.InstanceName;
            });
    }
}
