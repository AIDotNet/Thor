﻿using Microsoft.Extensions.Caching.Memory;
using Thor.BuildingBlocks.Cache;

namespace Thor.LocalMemory.Cache;

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

    public ValueTask IncrementAsync(string key, int value = 1, TimeSpan? ttl = null)
    {
        if (memoryCache.TryGetValue(key, out int cache))
        {
            cache += value;
            memoryCache.Set(key, cache, ttl ?? TimeSpan.FromMinutes(1));
        }
        else
        {
            memoryCache.Set(key, value, ttl ?? TimeSpan.FromMinutes(1));
        }

        return new ValueTask();
    }

    public async ValueTask<T?> GetOrCreateAsync<T>(string key, Func<ValueTask<T>> factory, TimeSpan? ttl = null)
    {
        if (memoryCache.TryGetValue(key, out T value))
        {
            return value;
        }

        var result = await factory();

        if (ttl.HasValue)
        {
            memoryCache.Set(key, result, ttl.Value);
        }
        else
        {
            memoryCache.Set(key, result);
        }

        return result;
    }
}