namespace Thor.Service.Options;

public class CacheOptions
{
    public const string Name = "Cache";

    /// <summary>
    /// 缓存类型 Memory/Redis
    /// 默认Memory
    /// </summary>
    public static string Type { get; set; } = "Memory";

    /// <summary>
    /// 连接字符串
    /// </summary>
    public static string? ConnectionString { get; set; }
}