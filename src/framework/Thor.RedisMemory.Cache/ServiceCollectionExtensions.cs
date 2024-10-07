using System.Text.Json;
using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddSingleton<RedisClient>((_) => new RedisClient(redisConnectionString)
        {
            Serialize = o => JsonSerializer.Serialize(o),
            Deserialize = (s, type) => JsonSerializer.Deserialize(s, type),
            DeserializeRaw = (s, type) => JsonSerializer.Deserialize(s, type)
        });
        services
            .AddSingleton<IServiceCache, RedisCache>();

        return services;
    }
}