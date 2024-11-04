using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Thor.Ollama.Chats.Dtos;

internal class OllamaChatCompletionsRequest
{
    public string model { get; set; } = null!;

    public List<OllamaChatRequestMessage> messages { get; set; } = null!;

    public string? format { get; set; }

    public OllamaChatOptions? options { get; set; }

    public bool? stream { get; set; }

    public string? keep_alive { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Tool>? Tools { get; set; }

}

/// <summary>
/// Represents a tool that the model can use, if supported.
/// </summary>
public class Tool
{
    /// <summary>
    /// Gets or sets the type of the tool, default is "function".
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "function";

    /// <summary>
    /// Gets or sets the function definition associated with this tool.
    /// </summary>
    [JsonPropertyName("function")]
    public Function? Function { get; set; }
}

/// <summary>
/// Represents a function that can be executed by a tool.
/// </summary>
public class Function
{
    /// <summary>
    /// Gets or sets the name of the function.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the function.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the parameters required by the function.
    /// </summary>
    [JsonPropertyName("parameters")]
    public Parameters? Parameters { get; set; }
}

/// <summary>
/// Represents the parameters required by a function, including their properties and required fields.
/// </summary>
public class Parameters
{
    /// <summary>
    /// Gets or sets the type of the parameters, default is "object".
    /// </summary>
    [JsonPropertyName("type")]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string? Type { get; set; } = "object";

    /// <summary>
    /// Gets or sets the properties of the parameters with their respective types and descriptions.
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, Properties>? Properties { get; set; }

    /// <summary>
    /// Gets or sets a list of required fields within the parameters.
    /// </summary>
    [JsonPropertyName("required")]
    public IEnumerable<string>? Required { get; set; }
}

/// <summary>
/// Represents a property within a function's parameters, including its type, description, and possible values.
/// </summary>
public class Properties
{
    /// <summary>
    /// Gets or sets the type of the property.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the description of the property.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets an enumeration of possible values for the property.
    /// </summary>
    [JsonPropertyName("enum")]
    public IEnumerable<string>? Enum { get; set; }
}
