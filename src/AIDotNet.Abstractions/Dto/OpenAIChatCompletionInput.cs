using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

/// <summary>
/// 对话输入
/// </summary>
/// <typeparam Name="T"></typeparam>
public class OpenAIChatCompletionInput<T> : OpenAICompletionInput
{
    [JsonPropertyName("messages")] public List<T> Messages { get; set; } = new List<T>();
}