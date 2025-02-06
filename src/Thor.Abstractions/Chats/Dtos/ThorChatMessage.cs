using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.Abstractions.Chats.Consts;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 聊天消息体，建议使用CreeateXXX系列方法构建内容
/// </summary>
public class ThorChatMessage
{
    /// <summary>
    /// 
    /// </summary>
    public ThorChatMessage()
    {

    }

    /// <summary>
    /// 【必填】发出消息的角色，请使用<see cref="ThorChatMessageRoleConst.User"/>赋值,如：ThorChatMessageRoleConst.User
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; }

    /// <summary>
    /// 发出的消息内容,如：你好
    /// </summary>
    [JsonIgnore]
    public string? Content { get; set; }

    /// <summary>
    /// 发出的消息内容，仅当使用 gpt-4o 模型时才支持图像输入。
    /// </summary>
    /// <example>
    /// 示例数据：
    /// "content": [
    ///   {
    ///     "type": "text",
    ///     "text": "What'\''s in this image?"
    ///   },
    ///   {
    ///     "type": "image_url",
    ///     "image_url": {
    ///       "url": "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg"
    ///     }
    ///   }
    /// ]
    /// </example>
    [JsonIgnore]
    public IList<ThorChatMessageContent>? Contents { get; set; }

    /// <summary>
    ///  发出的消息内容计算，用于json序列号和反序列化，Content 和 Contents 不能同时赋值，只能二选一
    /// </summary>
    [JsonPropertyName("content")]
    public object ContentCalculated
    {
        get
        {
            if (Content is not null && Contents is not null)
            {
                throw new ValidationException("Messages 中 Content 和 Contents 字段不能同时有值");
            }

            if (Content is not null)
            {
                return Content;
            }

            return Contents!;
        }
        set
        {
            if (value is JsonElement str)
            {
                if (str.ValueKind == JsonValueKind.String)
                {
                    Content = value?.ToString();
                }
                else if (str.ValueKind == JsonValueKind.Array)
                {
                    Contents = JsonSerializer.Deserialize<IList<ThorChatMessageContent>>(value?.ToString());
                }
            }
            else
            {
                Content = value?.ToString();
            }

        }
    }

    /// <summary>
    /// 【可选】参与者的可选名称。提供模型信息以区分相同角色的参与者。
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 工具调用 ID,此消息正在响应的工具调用。
    /// </summary>
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }

    /// <summary>
    /// 函数调用，已过期，不要使用，请使用 ToolCalls
    /// </summary>
    [JsonPropertyName("function_call")]
    public ThorChatMessageFunction? FunctionCall { get; set; }
    
    /// <summary>
    /// 【可选】推理内容
    /// </summary>
    [JsonPropertyName("reasoning_content")]
    public string? ReasoningContent { get; set; }

    /// <summary>
    /// 工具调用列表，模型生成的工具调用，例如函数调用。<br/>
    /// 此属性存储在客户端进行tool use 第一次调用模型返回的使用的函数名和传入的参数
    /// </summary>
    [JsonPropertyName("tool_calls")]
    public List<ThorToolCall>? ToolCalls { get; set; }

    /// <summary>
    /// 创建系统消息
    /// </summary>
    /// <param name="content">系统消息内容</param>
    /// <param name="name">参与者的可选名称。提供模型信息以区分同一角色的参与者。</param>
    /// <returns></returns>
    public static ThorChatMessage CreateSystemMessage(string content, string? name = null)
    {
        return new()
        {
            Role = ThorChatMessageRoleConst.System,
            Content = content,
            Name = name
        };
    }

    /// <summary>
    /// 创建用户消息
    /// </summary>
    /// <param name="content">系统消息内容</param>
    /// <param name="name">参与者的可选名称。提供模型信息以区分同一角色的参与者。</param>
    /// <returns></returns>
    public static ThorChatMessage CreateUserMessage(string content, string? name = null)
    {
        return new()
        {
            Role = ThorChatMessageRoleConst.User,
            Content = content,
            Name = name
        };
    }

    /// <summary>
    /// 创建助手消息
    /// </summary>
    /// <param name="content">系统消息内容</param>
    /// <param name="name">参与者的可选名称。提供模型信息以区分同一角色的参与者。</param>
    /// <param name="toolCalls">工具调用参数列表</param>
    /// <returns></returns>
    public static ThorChatMessage CreateAssistantMessage(string content, string? name = null, List<ThorToolCall> toolCalls = null)
    {
        return new()
        {
            Role = ThorChatMessageRoleConst.Assistant,
            Content = content,
            Name = name,
            ToolCalls=toolCalls,
        };
    }

    /// <summary>
    /// 创建工具消息
    /// </summary>
    /// <param name="content">系统消息内容</param>
    /// <param name="toolCallId">工具调用 ID,此消息正在响应的工具调用。</param>
    /// <returns></returns>
    public static ThorChatMessage CreateToolMessage(string content, string toolCallId = null)
    {
        return new()
        {
            Role = ThorChatMessageRoleConst.Tool,
            Content = content,
            ToolCallId= toolCallId
        };
    }
}