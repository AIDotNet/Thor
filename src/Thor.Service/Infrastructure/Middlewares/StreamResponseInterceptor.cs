using System.Text;
using System.Text.Json;
using Thor.Service.Options;

namespace Thor.Service.Infrastructure.Middlewares;

/// <summary>
/// 内容收集器包装类
/// </summary>
public sealed class ContentCollector
{
    private readonly StringBuilder _content = new();

    /// <summary>
    /// 添加字符串内容
    /// </summary>
    /// <param name="content">要添加的内容</param>
    public void Append(string content)
    {
        _content.Append(content);
    }

    /// <summary>
    /// 添加对象内容（序列化为JSON）
    /// </summary>
    /// <param name="content">要添加的对象</param>
    public void Append<T>(T content) where T : class
    {
        _content.Append(JsonSerializer.Serialize(content, JsonSerializerOptions.Default));
    }

    /// <summary>
    /// 获取收集的内容
    /// </summary>
    /// <returns>收集的内容</returns>
    public string GetContent()
    {
        return _content.ToString();
    }
}

/// <summary>
/// 流式响应拦截��，用于捕获流式响应的内容
/// </summary>
public static class StreamResponseInterceptor
{
    private static readonly AsyncLocal<ContentCollector> _contentCollector = new();

    /// <summary>
    /// 开始收集流式响应内容
    /// </summary>
    public static void StartCollecting()
    {
        _contentCollector.Value = new ContentCollector();
    }

    /// <summary>
    /// 添加内容到收集器
    /// </summary>
    /// <param name="content">要添加的内容</param>
    public static void AddContent(ref string content)
    {
        if (!ThorOptions.EnableRequestLog) return;
        _contentCollector.Value ??= new ContentCollector();
        _contentCollector.Value?.Append(content);
    }

    public static void AddContent<T>(T content) where T : class
    {
        if (!ThorOptions.EnableRequestLog) return;
        _contentCollector.Value ??= new ContentCollector();
        _contentCollector.Value?.Append(content);
    }

    /// <summary>
    /// 获取收集的内容
    /// </summary>
    /// <returns>收集的内容</returns>
    public static string? GetCollectedContent()
    {
        return _contentCollector.Value?.GetContent();
    }
}