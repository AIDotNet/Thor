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

    /// <summary>
    /// 平台基础Url
    /// </summary>
    public const string PlatformBaseUrl = "https://spark-api.xf-yun.com";

    public IServiceProvider ServiceProvider { get; set; }
}