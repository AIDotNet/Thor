using System.Text.Json.Serialization;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 有效函数调用的定义。
/// </summary>
public class ThorToolFunctionDefinition
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    /// <summary>
    /// 要调用的函数的名称。必须是 a-z、A-Z、0-9 或包含下划线和破折号，最大长度为 64。
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// 函数功能的描述，模型使用它来选择何时以及如何调用函数。
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     函数接受的参数，描述为 JSON 架构对象。有关示例，请参阅指南，有关格式的文档，请参阅 JSON 架构参考。
    ///     省略 parameters 定义一个参数列表为空的函数。
    ///     See the <a href="https://platform.openai.com/docs/guides/gpt/function-calling">guide</a> for examples,
    ///     and the <a href="https://json-schema.org/understanding-json-schema/">JSON Schema reference</a> for
    ///     documentation about the format.
    /// </summary>
    [JsonPropertyName("parameters")]
    public ThorToolFunctionPropertyDefinition Parameters { get; set; }
}