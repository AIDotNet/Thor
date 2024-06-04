using AIDotNet.Abstractions;
using FreeRedis;

namespace AIDotNet.API.Service.Cache;

public sealed class RedisCache(RedisClient redis) : IServiceCache
{
    public async ValueTask CreateAsync(string key, object value, TimeSpan? ttl = null)
    {
        await redis.SetAsync(key, value);

        if (ttl.HasValue)
        {
            await redis.ExpireAsync(key, ttl.Value);
        }
    }

    public async ValueTask<T?> GetAsync<T>(string key)
    {
        return await redis.GetAsync<T>(key);
    }

    public async ValueTask RemoveAsync(string key)
    {
        await redis.DelAsync(key);
    }
}