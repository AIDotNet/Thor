namespace AIDotNet.Abstractions;

public interface IServiceCache
{
    ValueTask CreateAsync(string key, object value, TimeSpan? ttl = null);
    
    ValueTask<T?> GetAsync<T>(string key);
    
    ValueTask RemoveAsync(string key);
}