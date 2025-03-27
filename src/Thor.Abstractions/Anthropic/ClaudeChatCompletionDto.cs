using System.Text.Json.Serialization;

namespace Thor.Abstractions.Anthropic;

public class ClaudeStreamDto
{
    public string type { get; set; }
    
    public int index { get; set; }
    public ClaudeChatCompletionDtoContent_block content_block { get; set; }
    
    public ClaudeChatCompletionDtoDelta? delta { get; set; }
    
    public ClaudeChatCompletionDto message { get; set; }
    

    public ClaudeChatCompletionDtoUsage Usage { get; set; }
}


public class ClaudeChatCompletionDtoDelta
{
    public string type { get; set; }
    
    public string text { get; set; }
    
    public string? thinking { get; set; }
    
    public string? partial_json { get; set; }
}


public class ClaudeChatCompletionDtoContent_block
{
    public string type { get; set; }
    
    public string thinking { get; set; }
    
    public string signature { get; set; }
    
    public string? id { get; set; }
    
    public string? name { get; set; }
    
    public object? input { get; set; }
}
public class RootObject
{
    public string type { get; set; }
    
    public int index { get; set; }
    
    public ClaudeChatCompletionDtoContent_block content_block { get; set; }
}

public class ClaudeChatCompletionDto
{
    public string id { get; set; }

    public string type { get; set; }

    public string role { get; set; }

    public ClaudeChatCompletionDtoContent[] content { get; set; }

    public string model { get; set; }

    public string stop_reason { get; set; }

    public object stop_sequence { get; set; }

    public ClaudeChatCompletionDtoUsage Usage { get; set; }
}

public class ClaudeChatCompletionDtoContent
{
    public string type { get; set; }

    public string? text { get; set; }

    public string? id { get; set; }

    public string? name { get; set; }

    public object? input { get; set; }

    [JsonPropertyName("thinking")] 
    public string? Thinking { get; set; }

    public string? signature { get; set; }
}

public class ClaudeChatCompletionDtoUsage
{
    public int? input_tokens { get; set; }

    public int? cache_creation_input_tokens { get; set; }

    public int? cache_read_input_tokens { get; set; }
    
    public int? output_tokens { get; set; }
}