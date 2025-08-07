namespace Thor.Service.Options;

/// <summary>
/// Tracing链路跟踪配置选项
/// </summary>
public class TracingOptions
{
    /// <summary>
    /// 配置节点名称
    /// </summary>
    public const string SectionName = "Tracing";

    /// <summary>
    /// 是否启用Tracing功能，默认启用
    /// 可通过环境变量 TRACING__ENABLE 控制
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 是否启用TracingMiddleware中间件，默认启用
    /// 可通过环境变量 TRACING__ENABLEMIDDLEWARE 控制
    /// </summary>
    public bool EnableMiddleware { get; set; } = true;

    /// <summary>
    /// 是否启用TracingEventHandler事件处理器（用于保存到数据库），默认启用
    /// 可通过环境变量 TRACING__ENABLEEVENTHANDLER 控制
    /// </summary>
    public bool EnableEventHandler { get; set; } = true;
}