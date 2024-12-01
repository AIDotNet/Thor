using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Thor.BuildingBlocks.Cache;

namespace Thor.RedisMemory.Cache;

public sealed class RedisCache : IServiceCache
{
    private readonly IServiceProvider _serviceProvider;

    public RedisCache(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _connectionMultiplexer = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
        Subscriber = _connectionMultiplexer.GetSubscriber();
    }

    private IConnectionMultiplexer _connectionMultiplexer;
    protected ISubscriber Subscriber;

    protected IDatabase Db
    {
        get
        {
            EnsureDbConnection();
            return _connectionMultiplexer.GetDatabase();
        }
    }

    protected void EnsureDbConnection()
    {
        if (_connectionMultiplexer is { IsConnected: false, IsConnecting: false })
        {
            _connectionMultiplexer =
                _connectionMultiplexer = _serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            Subscriber = _connectionMultiplexer.GetSubscriber();
        }

        if (_connectionMultiplexer is { IsConnected: false, IsConnecting: false })
        {
            throw new NotSupportedException(
                "Unable to reconnect to Redis, please check the connection settings and try again.");
        }
    }

    public async ValueTask CreateAsync(string key, object value, TimeSpan? ttl = null)
    {
        await Db.StringSetAndGetAsync(key, RedisValueConvert.ConvertToRedisValue(value));

        if (ttl.HasValue)
        {
            await Db.KeyExpireAsync(key, ttl);
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await Db.KeyExistsAsync(key);
    }

    public async ValueTask<T?> GetAsync<T>(string key)
    {
        if (!await ExistsAsync(key)) return default;
        var value = await Db.StringGetAsync(key);

        if (value.IsNullOrEmpty) return default;

        return RedisValueConvert.ConvertTo<T>(value);
    }

    public async ValueTask RemoveAsync(string key)
    {
        await Db.KeyDeleteAsync(key);
    }

    public async ValueTask IncrementAsync(string key, int value = 1, TimeSpan? ttl = null)
    {
        await Db.StringIncrementAsync(key, value);

        if (ttl.HasValue)
        {
            await Db.KeyExpireAsync(key, ttl);
        }
    }

    public async ValueTask<T?> GetOrCreateAsync<T>(string key, Func<ValueTask<T>> factory, TimeSpan? ttl = null,bool isLock = false)
    {
        if (!isLock)
        {
            if (await ExistsAsync(key))
            {
                var result = await GetAsync<T>(key);
                if (result != null) return result;
            }

            var value = await factory();
            await CreateAsync(key, value, ttl);
            return value;
        }
        
        var lockKey = $"{key}_lock";
        
        var expiry = TimeSpan.FromSeconds(3);

        await using var redisLock = new RedisLock(Db, lockKey, expiry);

        if (!await redisLock.AcquireAsync()) throw new InvalidOperationException("Unable to acquire lock");
        
        try
        {
            if (await ExistsAsync(key))
            {
                var result = await GetAsync<T>(key);
                if (result != null) return result;
            }

            var value = await factory();
            await CreateAsync(key, value, ttl);
            return value;
        }
        finally
        {
            await redisLock.ReleaseAsync();
        }

        throw new InvalidOperationException("Unable to acquire lock");
    }
}