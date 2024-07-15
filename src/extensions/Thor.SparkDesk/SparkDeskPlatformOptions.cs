namespace Thor.SparkDesk;

public sealed class SparkDeskPlatformOptions
{
    /// <summary>
    /// 平台名称
    /// </summary>
    public const string PlatformName = "星火大模型";

    /// <summary>
    /// 平台编码
    /// </summary>
    public const string PlatformCode = "SparkDesk";

    public IServiceProvider ServiceProvider { get; set; }
}