using System.Text.Json.Serialization;
using Thor.Abstractions.Chats.Dtos;

namespace Thor.Abstractions.Responses;

public class ResponsesToolsInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("vector_store_ids")]
    public List<string>? VectorStoreIds { get; set; }
    
    [JsonPropertyName("max_num_results")]
    public decimal? MaxNumResults { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
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
    public ThorToolFunctionPropertyDefinition? Parameters { get; set; }
}