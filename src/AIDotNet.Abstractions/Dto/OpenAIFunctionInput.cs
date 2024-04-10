using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto
{
    public sealed class OpenAIToolsFunctionInput<T> : OpenAIChatCompletionInput<T>
    {
        [JsonPropertyName("tools")]
        public OpenAIFunctionInput[] Tools { get; set; } = Array.Empty<OpenAIFunctionInput>();

        /// <summary>
        /// Function to use for selecting the next token. One of: "greedy", "top_k", "nucleus"
        /// </summary>
        [JsonPropertyName("tool_choice")]
        public string ToolChoice { get; set; }
    }

    public class OpenAIFunctionInput
    {
        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("function")] public OpenAIFunctionFunction Function { get; set; }
    }

    public class OpenAIFunctionFunction
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("parameters")]
        public OpenAIFunctionParameters Parameters { get; set; }
    }

    public class OpenAIFunctionParameters
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, OpenAIFunctionProperties> Properties { get; set; }

        [JsonPropertyName("required")]
        public string[] Required { get; set; }
    }


    public class OpenAIFunctionProperties
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("Description")]
        public string? Description { get; set; }
        //
        [JsonPropertyName("enum")]
        public string[]? Enum { get; set; }

        /// <summary>
        ///     The number of Properties on an object can be restricted using the minProperties and maxProperties keywords. Each of
        ///     these must be a non-negative integer.
        /// </summary>
        [JsonPropertyName("minProperties")]
        public int? MinProperties { get; set; }

        /// <summary>
        ///     The number of Properties on an object can be restricted using the minProperties and maxProperties keywords. Each of
        ///     these must be a non-negative integer.
        /// </summary>
        [JsonPropertyName("maxProperties")]
        public int? MaxProperties { get; set; }
    }
}