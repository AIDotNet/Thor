using FreeRedis;
using Thor.BuildingBlocks.Cache;

namespace Thor.RedisMemory.Cache;

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

    public async ValueTask IncrementAsync(string key, int value = 1, TimeSpan? ttl = null)
    {
        await redis.IncrByAsync(key, value);

        if (ttl.HasValue)
        {
            await redis.ExpireAsync(key, ttl.Value);
        }
    }
}