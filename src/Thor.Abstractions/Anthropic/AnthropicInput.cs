using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thor.Abstractions.Anthropic;

public sealed class AnthropicInput
{
    [JsonPropertyName("stream")] public bool Stream { get; set; }

    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("max_tokens")] public int? MaxTokens { get; set; }

    [JsonPropertyName("messages")] public IList<AnthropicMessageInput> Messages { get; set; }

    [JsonPropertyName("tools")] public IList<AnthropicMessageTool>? Tools { get; set; }

    [JsonPropertyName("tool_choice")]
    public object ToolChoiceCalculated
    {
        get
        {
            if (string.IsNullOrEmpty(ToolChoiceString))
            {
                return ToolChoiceString;
            }

            if (ToolChoice?.Type == "function")
            {
                return ToolChoice;
            }

            return ToolChoice?.Type;
        }
        set
        {
            if (value is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.String)
                {
                    ToolChoiceString = jsonElement.GetString();
                }
                else if (jsonElement.ValueKind == JsonValueKind.Object)
                {
                    ToolChoice = jsonElement.Deserialize<AnthropicTooChoiceInput>();
                }
            }
            else
            {
                ToolChoice = (AnthropicTooChoiceInput)value;
            }
        }
    }

    [JsonIgnore] public string ToolChoiceString { get; set; }

    [JsonIgnore] public AnthropicTooChoiceInput? ToolChoice { get; set; }

    [JsonPropertyName("system")] public List<AnthropicMessageContent>? System { get; set; }
}

public class AnthropicTooChoiceInput
{
    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }
}

public class AnthropicMessageTool
{
    [JsonPropertyName("name")] public string name { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("input_schema")] public Input_schema InputSchema { get; set; }
}

public class Input_schema
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("properties")] public Dictionary<string, object>? Properties { get; set; }

    [JsonPropertyName("required")] public string[]? Required { get; set; }
}