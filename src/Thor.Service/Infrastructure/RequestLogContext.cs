using Thor.Domain.Chats;

namespace Thor.Service.Infrastructure;

/// <summary>
/// 请求日志上下文，使用 AsyncLocal 来存储当前请求的日志对象
/// </summary>
public static class RequestLogContext
{
    private static readonly AsyncLocal<RequestLog?> _current = new();

    /// <summary>
    /// 获取当前请求的日志对象
    /// </summary>
    public static RequestLog? Current => _current.Value;

    /// <summary>
    /// 设置当前请求的日志对象
    /// </summary>
    /// <param name="requestLog">请求日志对象</param>
    public static void SetCurrent(RequestLog? requestLog)
    {
        _current.Value = requestLog;
    }

    /// <summary>
    /// 清理当前请求的日志对象
    /// </summary>
    public static void Clear()
    {
        _current.Value = null;
    }
}
