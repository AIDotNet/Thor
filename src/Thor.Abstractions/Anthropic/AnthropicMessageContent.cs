using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thor.Abstractions.Anthropic;

public class AnthropicMessageContent
{
    [JsonPropertyName("cache_control")] public AnthropicCacheControl? CacheControl { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("text")] public string? Text { get; set; }

    [JsonPropertyName("tool_use_id")] public string? ToolUseId { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("id")] public string? Id { get; set; }

    [JsonPropertyName("input")] public object? Input { get; set; }

    [JsonPropertyName("content")]
    public object? Content
    {
        get
        {
            if (_content is not null && _contents is not null)
            {
                throw new ValidationException("Messages 中 Content 和 Contents 字段不能同时有值");
            }

            if (_content is not null)
            {
                return _content;
            }

            return _contents!;
        }
        set
        {
            if (value is JsonElement str)
            {
                if (str.ValueKind == JsonValueKind.String)
                {
                    _content = value?.ToString();
                }
                else if (str.ValueKind == JsonValueKind.Array)
                {
                    _contents = JsonSerializer.Deserialize<List<AnthropicMessageContent>>(value?.ToString());
                }
            }
            else
            {
                _content = value?.ToString();
            }
        }
    }

    private string? _content;

    private List<AnthropicMessageContent> _contents;

    public class AnthropicMessageContentSource
    {
        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("media_type")] public string? MediaType { get; set; }

        [JsonPropertyName("data")] public string? Data { get; set; }
    }

    [JsonPropertyName("source")] public AnthropicMessageContentSource? Source { get; set; }
}