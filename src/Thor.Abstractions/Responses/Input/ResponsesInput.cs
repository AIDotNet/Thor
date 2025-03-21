using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace Thor.Abstractions.Responses;

public class ResponsesInput : IOpenAiModels.IModel, IOpenAiModels.IUser
{
    [JsonPropertyName("model")] public string? Model { get; set; }

    [JsonPropertyName("stream")] public bool? Stream { get; set; }

    [JsonPropertyName("instructions")] public string? Instructions { get; set; }

    /// <summary>
    /// 发出的消息内容,如：你好
    /// </summary>
    [JsonIgnore]
    public string? Input { get; set; }

    /// <summary>
    /// 发出的消息内容，仅当使用 gpt-4o 模型时才支持图像输入。
    /// </summary>
    [JsonIgnore]
    public IList<ResponsesMessageInput>? Inputs { get; set; }

    /// <summary>
    ///  发出的消息内容计算，用于json序列号和反序列化，Content 和 Contents 不能同时赋值，只能二选一
    /// </summary>
    [JsonPropertyName("input")]
    public object InputCalculated
    {
        get
        {
            if (Input is not null && Inputs is not null)
            {
                throw new ValidationException("Messages 中 Content 和 Contents 字段不能同时有值");
            }

            if (Input is not null)
            {
                return Input;
            }

            return Inputs!;
        }
        set
        {
            if (value is JsonElement str)
            {
                if (str.ValueKind == JsonValueKind.String)
                {
                    Input = value?.ToString();
                }
                else if (str.ValueKind == JsonValueKind.Array)
                {
                    Inputs = JsonSerializer.Deserialize<IList<ResponsesMessageInput>>(value?.ToString());
                }
            }
            else
            {
                Input = value?.ToString();
            }
        }
    }

    /// <summary>
    /// 是否数组
    /// </summary>
    /// <returns></returns>
    [JsonIgnore]
    public bool IsMessageArray
    {
        get
        {
            if (InputCalculated is IList<ResponsesMessageInput>)
            {
                return true;
            }

            return false;
        }
    }

    [JsonPropertyName("user")] public string? User { get; set; }

    [JsonPropertyName("reasoning")] public ReasoningResponsesInput? Reasoning { get; set; }

    [JsonPropertyName("tools")] public IList<ResponsesToolsInput>? Tools { get; set; }

    [JsonPropertyName("max_output_tokens")]
    public int? MaxOutputTokens { get; set; }
    
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public object? ToolChoice { get; set; }
    
    [JsonPropertyName("top_p")]
    public double? TopP { get; set; }
    
    [JsonPropertyName("truncation")]
    public string? Truncation { get; set; }
    
    [JsonPropertyName("store")]
    public bool? Store { get; set; }
}

public sealed class ReasoningResponsesInput : ResponsesInput
{
    [JsonPropertyName("effort")] public string? Effort { get; set; }
}