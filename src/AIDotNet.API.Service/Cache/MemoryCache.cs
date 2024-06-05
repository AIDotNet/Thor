using AIDotNet.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace AIDotNet.API.Service.Cache;

public sealed class MemoryCache(IMemoryCache memoryCache) : IServiceCache
{
    public async ValueTask CreateAsync(string key, object value, TimeSpan? ttl = null)
    {
        if (ttl.HasValue)
        {
            memoryCache.Set(key, value, ttl.Value);

            var token = new CancellationTokenSource(ttl.Value);
            token.Token.Register(() => memoryCache.Remove(key));
            memoryCache.Set(key, value);
        }
        else
        {
            memoryCache.Set(key, value);
        }

        await ValueTask.CompletedTask;
    }

    public ValueTask<T?> GetAsync<T>(string key)
    {
        if (memoryCache.TryGetValue(key, out T value))
        {
            return new ValueTask<T?>(value);
        }

        return new ValueTask<T?>(default(T));
    }

    public ValueTask RemoveAsync(string key)
    {
        memoryCache.Remove(key);
        return new ValueTask();
    }
}