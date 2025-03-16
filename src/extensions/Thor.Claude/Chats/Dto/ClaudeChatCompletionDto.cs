using System.Text.Json.Serialization;

namespace Thor.Claude.Chats.Dto;

public class ClaudeStreamDto
{
    public string type { get; set; }
    
    public int index { get; set; }
    public Content_block content_block { get; set; }
    
    public Delta delta { get; set; }
    
    public ClaudeChatCompletionDto message { get; set; }
    

    public Usage usage { get; set; }
}


public class Delta
{
    public string type { get; set; }
    
    public string text { get; set; }
    
    public string thinking { get; set; }
}


public class Content_block
{
    public string type { get; set; }
    
    public string thinking { get; set; }
    
    public string signature { get; set; }
}


public class ClaudeChatCompletionDto
{
    public string id { get; set; }

    public string type { get; set; }

    public string role { get; set; }

    public Content[] content { get; set; }

    public string model { get; set; }

    public string stop_reason { get; set; }

    public object stop_sequence { get; set; }

    public Usage usage { get; set; }
}

public class Content
{
    public string type { get; set; }

    public string? text { get; set; }

    public string? id { get; set; }

    public string? name { get; set; }

    public object? input { get; set; }

    [JsonPropertyName("thinking")] public string? Thinking { get; set; }

    public string? signature { get; set; }
}

public class Usage
{
    public int input_tokens { get; set; }

    public int? cache_creation_input_tokens { get; set; }

    public int? cache_read_input_tokens { get; set; }
    public int output_tokens { get; set; }
}