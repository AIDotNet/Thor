using Thor.Abstractions;
using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

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
    public int Quota { get; set; }

    /// <summary>
    /// 模型
    /// </summary>
    public string ModelName { get; set; }

    /// <summary>
    /// token名称
    /// </summary>
    public string? TokenName { get; set; }

    public string? UserName { get; set; }

    public string? UserId { get; set; }

    /// <summary>
    /// 渠道Id
    /// </summary>
    public string? ChannelId { get; set; }

    /// <summary>
    /// 渠道名称
    /// </summary>
    public string? ChannelName { get; set; }
}