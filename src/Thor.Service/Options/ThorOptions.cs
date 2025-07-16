namespace Thor.Service.Options;

public class ThorOptions
{
    /// <summary>
    /// 是否启用记录请求日志
    /// </summary>
    /// <returns></returns>
    public static bool EnableRequestLog { get; set; } = true;

    public static void Initialize(IConfiguration configuration)
    {
        // 获取配置
        var enableRequestLog = configuration.GetValue<bool?>("EnableRequestLog") ??
                               configuration.GetValue<bool?>("ENABLE_REQUESTLOG");
        ;

        if (enableRequestLog.HasValue)
        {
            EnableRequestLog = enableRequestLog.Value;
        }
    }
}