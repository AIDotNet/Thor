using System.Text.Json;
using Thor.Abstractions;
using Thor.Service.Domain.Core;

namespace Thor.Domain.Chats;

public sealed class RequestLog : Entity<string>
{
    /// <summary>
    /// 关联的ChatLogger ID
    /// </summary>
    public string? ChatLoggerId { get; set; }

    /// <summary>
    /// 请求路由路径
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 请求时间
    /// </summary>
    public DateTime RequestTime { get; set; }

    /// <summary>
    /// 响应时间
    /// </summary>
    public DateTime ResponseTime { get; set; }

    /// <summary>
    /// 处理耗时（毫秒）
    /// </summary>
    public long DurationMs { get; set; }

    /// <summary>
    /// 处理状态
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// HTTP状态码
    /// </summary>
    public int HttpStatusCode { get; set; }

    /// <summary>
    /// 客户端IP
    /// </summary>
    public string? ClientIp { get; set; }

    /// <summary>
    /// 用户代理
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 请求头信息（JSON格式）
    /// </summary>
    public string? RequestHeaders { get; set; }

    /// <summary>
    /// 完整请求参数（字符串格式）
    /// </summary>
    public string? RequestBody { get; set; }

    /// <summary>
    /// 完整响应参数（字符串格式）
    /// </summary>
    public string? ResponseBody { get; set; }

    /// <summary>
    /// 错误信息（如果失败）
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 设置请求头信息
    /// </summary>
    public void SetRequestHeaders(Dictionary<string, string> headers)
    {
        RequestHeaders = JsonSerializer.Serialize(headers);
    }

    /// <summary>
    /// 获取请求头信息
    /// </summary>
    public Dictionary<string, string>? GetRequestHeaders()
    {
        if (string.IsNullOrEmpty(RequestHeaders))
            return null;

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(RequestHeaders);
        }
        catch
        {
            return null;
        }
    }
}