using System.Text.Json;
using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using Thor.BuildingBlocks.Cache;

namespace Thor.RedisMemory.Cache;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisMemory(this IServiceCollection services, string redisConnectionString)
    {
        services.AddSingleton<RedisClient>((_) => new RedisClient(redisConnectionString)
        {
            Serialize = o => JsonSerializer.Serialize(o),
            Deserialize = (s, type) => JsonSerializer.Deserialize(s, type)
        });
        services
            .AddSingleton<IServiceCache, RedisCache>();

        return services;
    }
}