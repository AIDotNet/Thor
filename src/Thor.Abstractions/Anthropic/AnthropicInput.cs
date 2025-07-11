using System.ComponentModel.DataAnnotations;
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

    [JsonIgnore]
    public IList<AnthropicMessageContent>? Systems { get; set; }

    [JsonIgnore]
    public string System { get; set; }

    [JsonPropertyName("system")]
    public object SystemCalculated
    {
        get
        {
            if (System is not null && Systems is not null)
            {
                throw new ValidationException("System 和 Systems 字段不能同时有值");
            }

            if (System is not null)
            {
                return System;
            }

            return Systems!;
        }
        set
        {
            if (value is JsonElement str)
            {
                if (str.ValueKind == JsonValueKind.String)
                {
                    System = value?.ToString();
                }
                else if (str.ValueKind == JsonValueKind.Array)
                {
                    Systems = JsonSerializer.Deserialize<IList<AnthropicMessageContent>>(value?.ToString());
                }
            }
            else
            {
                System = value?.ToString();
            }
        }
    }
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