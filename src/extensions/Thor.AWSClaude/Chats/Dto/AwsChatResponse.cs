using System.Text.Json.Serialization;
using Amazon.Runtime;

namespace Thor.AWSClaude.Chats.Dto;

public class AwsChatResponse : AmazonWebServiceResponse
{
    public AwsMetrics? metrics { get; set; }

    public AwsOutput? output { get; set; }

    public string stopReason { get; set; }

    public AwsUsage? usage { get; set; }
}

public class AwsMetrics
{
    public int latencyMs { get; set; }
}

public class AwsOutput
{
    public AwsMessage? message { get; set; }
}

public class AwsMessage
{
    public AwsContent[] content { get; set; }

    public string role { get; set; }
}

public class AwsContent
{
    [JsonPropertyName("reasoningContent")] public ReasoningContent? ReasoningContent { get; set; }

    [JsonPropertyName("text")] public string Text { get; set; }

    [JsonPropertyName("toolUse")] public AwsResponseContentToolUse? ToolUse { get; set; }
}

public class AwsStreamResponseContentToolUse
{
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("toolUseId")] public string? ToolUseId { get; set; }

    [JsonPropertyName("input")] public string Input { get; set; }
}

public class AwsResponseContentToolUse
{
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("toolUseId")] public string? ToolUseId { get; set; }

    [JsonPropertyName("input")] public Dictionary<string, object>? Input { get; set; }
}

public class ReasoningContent
{
    public ReasoningText? reasoningText { get; set; }
}

public class ReasoningText
{
    public string signature { get; set; }

    public string? text { get; set; }
}

public class AwsUsage
{
    public int cacheReadInputTokenCount { get; set; }

    public int cacheReadInputTokens { get; set; }

    public int cacheWriteInputTokenCount { get; set; }

    public int cacheWriteInputTokens { get; set; }

    public int inputTokens { get; set; }

    public int outputTokens { get; set; }

    public int totalTokens { get; set; }
}