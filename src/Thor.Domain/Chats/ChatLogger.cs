using Thor.Abstractions;
using Thor.Service.Domain.Core;

namespace Thor.Domain.Chats;

public sealed class ChatLogger : Entity<string>
{
    public ThorChatLoggerType Type { get; set; }

    public string Content { get; set; }

    /// <summary>
    /// 请求Token
    /// </summary>
    public int PromptTokens { get; set; }

    /// <summary>
    /// 完成Token
    /// </summary>
    public int CompletionTokens { get; set; }

    /// <summary>
    /// 消费额度
    /// </summary>
    public long Quota { get; set; }

    /// <summary>
    /// 模型
    /// </summary>
    public string ModelName { get; set; }

    /// <summary>
    /// token
    /// </summary>
    public string? TokenName { get; set; }

    public string? UserName { get; set; }

    public string? UserId { get; set; }

    /// <summary>
    /// 渠道Id
    /// </summary>
    public string? ChannelId { get; set; }

    /// <summary>
    /// 总耗时
    /// </summary>
    public int TotalTime { get; set; }

    /// <summary>
    /// 是否是流式
    /// </summary>
    public bool Stream { get; set; }

    /// <summary>
    /// 渠道名称
    /// </summary>
    public string? ChannelName { get; set; }

    public string? IP { get; set; }

    public string? UserAgent { get; set; }

    /// <summary>
    /// 组织id
    /// </summary>
    public string? OrganizationId { get; set; }
    
    /// <summary>
    /// 请求的url
    /// </summary>
    public string? Url { get; set; }
    
    /// <summary>
    /// OpenAI的项目id
    /// </summary>
    public string? OpenAIProject { get; set; } = string.Empty;
    
    /// <summary>
    /// Service服务id
    /// </summary>
    public string? ServiceId { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否请求成功
    /// </summary>
    public bool IsSuccess { get; set; } = true;
    
    /// <summary>
    /// 元数据
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string>? Metadata { get; set; } = new();
}