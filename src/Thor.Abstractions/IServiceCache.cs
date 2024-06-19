namespace Thor.Abstractions;

public interface IServiceCache
{
    ValueTask CreateAsync(string key, object value, TimeSpan? ttl = null);

    ValueTask<T?> GetAsync<T>(string key);

    ValueTask RemoveAsync(string key);

    /// <summary>
    /// 递增
    /// </summary>
    ValueTask IncrementAsync(string key, int value = 1, TimeSpan? ttl = null);
}