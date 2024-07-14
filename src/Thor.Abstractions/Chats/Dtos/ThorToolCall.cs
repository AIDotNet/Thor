using System.Text.Json.Serialization;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 工具调用对象定义
/// </summary>
public class ThorToolCall
{
    public ThorToolCall()
    {
        Id = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// 工具调用序号值 
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary>
    /// 工具调用的 ID
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// 工具的类型。目前仅支持 function
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "function";

    /// <summary>
    /// 模型调用的函数。
    /// </summary>
    [JsonPropertyName("function")]
    public ThorChatMessageFunction? Function { get; set; }
}