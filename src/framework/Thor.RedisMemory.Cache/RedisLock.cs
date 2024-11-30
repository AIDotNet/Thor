using StackExchange.Redis;

namespace Thor.RedisMemory.Cache;

public class RedisLock : IAsyncDisposable
{
    private readonly IDatabase _database;
    private readonly string _lockKey;
    private readonly string _lockValue;
    private readonly TimeSpan _expiry;

    public RedisLock(IDatabase database, string lockKey, TimeSpan expiry)
    {
        _database = database;
        _lockKey = lockKey;
        _lockValue = Guid.NewGuid().ToString();
        _expiry = expiry;
    }

    public async Task<bool> AcquireAsync()
    {
        return await _database.LockTakeAsync(_lockKey, _lockValue, _expiry);
    }

    public async Task ReleaseAsync()
    {
        await _database.LockReleaseAsync(_lockKey, _lockValue);
    }

    public async ValueTask DisposeAsync()
    {
        await ReleaseAsync();
    }
}