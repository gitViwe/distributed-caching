using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Shared.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRedisConnectionMultiplexer(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis://redis:6379"));
        services.AddSingleton<MyRedisCache>();

        return services;
    }
}
