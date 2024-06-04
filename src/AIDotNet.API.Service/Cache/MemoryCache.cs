using AIDotNet.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace AIDotNet.API.Service.Cache;

public sealed class MemoryCache(IMemoryCache memoryCache) : IServiceCache
{
    public async ValueTask CreateAsync(string key, object value, TimeSpan? ttl = null)
    {
        if (ttl.HasValue)
        {
            memoryCache.Set(key, value, ttl.Value);
        }
        else
        {
            memoryCache.Set(key, value);
        }

        await ValueTask.CompletedTask;
    }

    public ValueTask<T?> GetAsync<T>(string key)
    {
        var value = memoryCache.Get<T>(key);
        return new ValueTask<T?>(value);
    }

    public ValueTask RemoveAsync(string key)
    {
        memoryCache.Remove(key);
        return new ValueTask();
    }
}