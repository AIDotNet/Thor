namespace Thor.BuildingBlocks.Cache;

public interface IServiceCache
{
    /// <summary>
    /// 创建缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="ttl"></param>
    /// <returns></returns>
    ValueTask CreateAsync(string key, object value, TimeSpan? ttl = null);

    /// <summary>
    /// 获取指定缓存
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask<T?> GetAsync<T>(string key);

    /// <summary>
    /// 删除指定缓存
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ValueTask RemoveAsync(string key);

    /// <summary>
    /// 递增
    /// </summary>
    ValueTask IncrementAsync(string key, int value = 1, TimeSpan? ttl = null);
}