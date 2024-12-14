using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Thor.BuildingBlocks.Cache;

namespace Thor.RedisMemory.Cache
{
    public sealed class RedisCache : IServiceCache, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<RedisCache> _logger;
        private ISubscriber _subscriber;
        private readonly string _cacheInvalidationChannel = "Thor.Cache.Invalidation";

        private IConnectionMultiplexer _connectionMultiplexer;

        // 定义活动源
        private static readonly ActivitySource ActivitySource = new("Thor.RedisMemory.Cache.RedisCache");

        public RedisCache(IServiceProvider serviceProvider, IMemoryCache memoryCache, ILogger<RedisCache> logger)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
            _logger = logger;

            _connectionMultiplexer = _serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            _subscriber = _connectionMultiplexer.GetSubscriber();

            // 订阅缓存失效消息
            _subscriber.Subscribe(_cacheInvalidationChannel, OnCacheInvalidated);
        }

        private void OnCacheInvalidated(RedisChannel channel, RedisValue message)
        {
            using var activity = ActivitySource.StartActivity("CacheInvalidated", ActivityKind.Consumer);
            var cacheKey = message.ToString();
            _logger.LogInformation($"[CacheInvalidated] Invalidating local cache for key: {cacheKey}");
            _memoryCache.Remove(cacheKey);
            activity?.SetTag("cache.key", cacheKey);
            activity?.SetTag("cache.source", "Redis");
        }

        private IDatabase Db
        {
            get
            {
                EnsureDbConnection();
                return _connectionMultiplexer.GetDatabase();
            }
        }

        private void EnsureDbConnection()
        {
            if (_connectionMultiplexer is { IsConnected: false, IsConnecting: false })
            {
                _connectionMultiplexer = _serviceProvider.GetRequiredService<IConnectionMultiplexer>();
                _subscriber = _connectionMultiplexer.GetSubscriber();
                _subscriber.Subscribe(_cacheInvalidationChannel, OnCacheInvalidated);
                _logger.LogInformation("Reconnected to Redis and resubscribed to invalidation channel.");
            }

            if (_connectionMultiplexer is { IsConnected: false, IsConnecting: false })
            {
                throw new NotSupportedException(
                    "Unable to reconnect to Redis, please check the connection settings and try again.");
            }
        }

        private async Task PublishInvalidationAsync(string key)
        {
            using var activity = ActivitySource.StartActivity("PublishInvalidationAsync", ActivityKind.Producer);
            await _subscriber.PublishAsync(_cacheInvalidationChannel, key);
            _logger.LogInformation($"[PublishInvalidation] Published invalidation for key: {key}");
            activity?.SetTag("cache.key", key);
            activity?.SetTag("cache.source", "Redis");
        }

        public async ValueTask CreateAsync(string key, object value, TimeSpan? ttl = null)
        {
            using var activity =
                Activity.Current?.Source.StartActivity("CreateAsync", ActivityKind.Producer);
            var redisValue = RedisValueConvert.ConvertToRedisValue(value);
            await Db.StringSetAsync(key, redisValue, ttl);
            _logger.LogInformation($"[CreateAsync] Created Redis cache entry for key: {key} with TTL: {ttl}");

            // 更新本地内存缓存
            if (ttl.HasValue)
            {
                _memoryCache.Set(key, value, ttl.Value);
                _logger.LogInformation($"[CreateAsync] Set in-memory cache for key: {key} with TTL: {ttl}");
            }
            else
            {
                _memoryCache.Set(key, value);
                _logger.LogInformation($"[CreateAsync] Set in-memory cache for key: {key} with no TTL");
            }

            // 发布缓存更新消息
            await PublishInvalidationAsync(key);
            activity?.SetTag("cache.key", key);
            activity?.SetTag("cache.source", "Redis");
        }

        public async Task<bool> ExistsAsync(string key)
        {
            using var activity =
                Activity.Current?.Source.StartActivity("ExistsAsync", ActivityKind.Consumer);
            // 优先检查内存缓存
            if (_memoryCache.TryGetValue(key, out _))
            {
                _logger.LogInformation($"[ExistsAsync] Key: {key} exists in Memory Cache.");
                activity?.SetTag("cache.key", key);
                activity?.SetTag("cache.source", "Memory");
                return true;
            }

            // 检查 Redis
            bool exists = await Db.KeyExistsAsync(key);
            _logger.LogInformation($"[ExistsAsync] Key: {key} exists in Redis: {exists}");
            activity?.SetTag("cache.key", key);
            activity?.SetTag("cache.source", "Redis");
            return exists;
        }

        public async ValueTask<T?> GetAsync<T>(string key)
        {
            using var activity =
                Activity.Current?.Source.StartActivity("GetAsync", ActivityKind.Consumer);

            // 尝试从内存缓存获取
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                _logger.LogInformation($"[GetAsync] Retrieved key: {key} from Memory Cache.");
                activity?.SetTag("cache.key", key);
                activity?.SetTag("cache.source", "Memory");
                return cachedValue;
            }
            var value = await Db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                _logger.LogInformation($"[GetAsync] Redis returned null or empty for key: {key}.");
                activity?.SetTag("cache.key", key);
                activity?.SetTag("cache.source", "Redis");
                return default;
            }

            var result = RedisValueConvert.ConvertTo<T>(value);
            _memoryCache.Set(key, result);
            _logger.LogInformation($"[GetAsync] Retrieved key: {key} from Redis and cached in Memory Cache.");
            activity?.SetTag("cache.key", key);
            activity?.SetTag("cache.source", "Redis");
            return result;
        }

        public async ValueTask RemoveAsync(string key)
        {
            using var activity =
                Activity.Current?.Source.StartActivity("RemoveAsync", ActivityKind.Internal);
            // 从 Redis 删除
            await Db.KeyDeleteAsync(key);
            _logger.LogInformation($"[RemoveAsync] Deleted key: {key} from Redis.");

            // 从内存缓存删除
            _memoryCache.Remove(key);
            _logger.LogInformation($"[RemoveAsync] Removed key: {key} from Memory Cache.");

            // 发布缓存删除消息
            await PublishInvalidationAsync(key);
            activity?.SetTag("cache.key", key);
            activity?.SetTag("cache.source", "Redis");
        }

        public async ValueTask IncrementAsync(string key, int value = 1, TimeSpan? ttl = null)
        {
            using var activity =
                Activity.Current?.Source.StartActivity("IncrementAsync", ActivityKind.Internal);
            await Db.StringIncrementAsync(key, value);
            _logger.LogInformation($"[IncrementAsync] Incremented key: {key} by {value} in Redis.");

            // 更新内存缓存
            if (_memoryCache.TryGetValue(key, out int currentValue))
            {
                _memoryCache.Set(key, currentValue + value, ttl ?? TimeSpan.FromHours(1));
                _logger.LogInformation($"[IncrementAsync] Updated Memory Cache for key: {key} to {currentValue + value}.");
            }

            // 设置过期时间
            if (ttl.HasValue)
            {
                await Db.KeyExpireAsync(key, ttl);
                _logger.LogInformation($"[IncrementAsync] Set TTL for key: {key} to {ttl}.");
            }

            // 发布缓存更新消息
            await PublishInvalidationAsync(key);
            activity?.SetTag("cache.key", key);
            activity?.SetTag("cache.source", "Redis");
        }

        public async ValueTask<T?> GetOrCreateAsync<T>(string key, Func<ValueTask<T>> factory, TimeSpan? ttl = null,
            bool isLock = false)
        {

            using var activity =
                Activity.Current?.Source.StartActivity("获取缓存或创建内存", ActivityKind.Internal);
            // 尝试从内存缓存获取
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                _logger.LogInformation($"[GetOrCreateAsync] Retrieved key: {key} from Memory Cache.");
                activity?.SetTag("cache.key", key);
                activity?.SetTag("cache.source", "Memory");
                return cachedValue;
            }

            if (!isLock)
            {
                var result = await GetAsync<T>(key);
                if (result != null)
                {
                    activity?.SetTag("cache.key", key);
                    activity?.SetTag("cache.source", "Redis");
                    return result;
                }

                var value = await factory();
                await CreateAsync(key, value, ttl);
                activity?.SetTag("cache.key", key);
                activity?.SetTag("cache.source", "FactoryCreated");
                return value;
            }

            var lockKey = $"{key}_lock";
            var expiry = TimeSpan.FromSeconds(3);

            await using var redisLock = new RedisLock(Db, lockKey, expiry);

            if (!await redisLock.AcquireAsync())
            {
                _logger.LogWarning($"[GetOrCreateAsync] Unable to acquire lock for key: {key}.");
                throw new InvalidOperationException("Unable to acquire lock");
            }

            try
            {
                // 再次检查缓存，防止重复创建
                if (_memoryCache.TryGetValue(key, out cachedValue))
                {
                    _logger.LogInformation($"[GetOrCreateAsync] Retrieved key: {key} from Memory Cache after acquiring lock.");
                    activity?.SetTag("cache.key", key);
                    activity?.SetTag("cache.source", "Memory");
                    return cachedValue;
                }

                if (await ExistsAsync(key))
                {
                    var result = await GetAsync<T>(key);
                    if (result != null)
                    {
                        activity?.SetTag("cache.key", key);
                        activity?.SetTag("cache.source", "Redis");
                        return result;
                    }
                }

                var value = await factory();
                await CreateAsync(key, value, ttl);
                _logger.LogInformation($"[GetOrCreateAsync] Created key: {key} using factory.");
                activity?.SetTag("cache.key", key);
                activity?.SetTag("cache.source", "FactoryCreated");
                return value;
            }
            finally
            {
                await redisLock.ReleaseAsync();
                _logger.LogInformation($"[GetOrCreateAsync] Released lock for key: {key}.");
            }
        }

        public void Dispose()
        {
            _subscriber.Unsubscribe(_cacheInvalidationChannel, OnCacheInvalidated);
            _connectionMultiplexer?.Dispose();
            _logger.LogInformation("Disposed RedisCache and unsubscribed from invalidation channel.");
        }
    }
}