using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Thor.BuildingBlocks.Cache;

namespace Thor.RedisMemory.Cache;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加Redis内存
    /// </summary>
    /// <param name="services"></param>
    /// <param name="redisConnectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedisMemory(this IServiceCollection services, string redisConnectionString)
    {
        services.AddMemoryCache();
        services.AddTransient<IConnectionMultiplexer>((_) =>
        {
            var connection = ConnectionMultiplexer.Connect(redisConnectionString);
            return connection;
        });
        
        services
            .AddSingleton<IServiceCache, RedisCache>();

        return services;
    }
}