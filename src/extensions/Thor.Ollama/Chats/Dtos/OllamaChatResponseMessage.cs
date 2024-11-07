using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Thor.Ollama.Chats.Dtos;

internal class OllamaChatResponseMessage
{
    public string role { get; set; } = null!;
    public string content { get; set; } = null!;
    public List<string>? Images { get; set; }
    /// <summary>
    /// Gets or sets a list of tools the model wants to use (for models that support function calls, such as llama3.1).
    /// </summary>
    [JsonPropertyName("tool_calls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ToolCall>? ToolCalls { get; set; }

    /// <summary>
    /// Represents a tool call within a message.
    /// </summary>
    public class ToolCall
    {
        /// <summary>
        /// Gets or sets the function to be called by the tool.
        /// </summary>
        [JsonPropertyName("function")]
        public Function? Function { get; set; }
    }

    /// <summary>
    /// Represents a function that can be called by a tool.
    /// </summary>
    public class Function
    {
        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the arguments for the function, represented as a dictionary of argument names and values.
        /// </summary>
        [JsonPropertyName("arguments")]
        public IDictionary<string, object?>? Arguments { get; set; }
    }
}