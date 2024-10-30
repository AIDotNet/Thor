using System.Text.Json.Serialization;

namespace Thor.Abstractions.Realtime.Dto;

public class RealtimeResult
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("event_id")]
    public string EventId { get; set; }
    
    [JsonPropertyName("session")]
    public Session? Session { get; set; }
    
    [JsonPropertyName("response")]
    public RealtimeResponse? Response { get; set; }

    [JsonPropertyName("response_id")]
    public string? ResponseId { get; set; }

    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }
    
    [JsonPropertyName("item")]
    public RealtimeItem? Item { get; set; }
    
    [JsonPropertyName("part")]
    public RealtimePartContent? Part { get; set; }
    
    [JsonPropertyName("transcript")]
    public string? Transcript { get; set; }
    
    [JsonPropertyName("item_id")]
    public string? ItemId { get; set; }
    
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }
    
    [JsonPropertyName("delta")]
    public string? Delta { get; set; }
}

public class RealtimeResponse
{
    [JsonPropertyName("object")]
    public string? @object { get; set; }
    
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    
    [JsonPropertyName("status_details")]
    public object? StatusDetails { get; set; }
    
    [JsonPropertyName("output")]
    public Output[]? Output { get; set; }
    
    [JsonPropertyName("usage")]
    public Usage? Usage { get; set; }
}

public class Output
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("object")]
    public string @object { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("role")]
    public string Role { get; set; }
    
    [JsonPropertyName("content")]
    public RealtimePartContent[] Content { get; set; }
}

public class RealtimePartContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("transcript")]
    public string Transcript { get; set; }
}

public class Usage
{
    [JsonPropertyName("total_tokens")]
    public int? TotalTokens { get; set; }
    
    [JsonPropertyName("input_tokens")]
    public int? InputTokens { get; set; }
    
    [JsonPropertyName("output_tokens")]
    public int? OutputTokens { get; set; }
    
    [JsonPropertyName("input_token_details")]
    public Input_token_details? InputTokenDetails { get; set; }
    
    [JsonPropertyName("output_token_details")]
    public Output_token_details? OutputTokenDetails { get; set; }
}

public class Input_token_details
{
    [JsonPropertyName("text_tokens")]
    public int? TextTokens { get; set; }
    
    [JsonPropertyName("audio_tokens")]
    public int? AudioTokens { get; set; }
    
    [JsonPropertyName("cached_tokens")]
    public int? CachedTokens { get; set; }
    
    [JsonPropertyName("cached_tokens_details")]
    public CachedTokensDetails CachedTokensDetails { get; set; }
}

public class CachedTokensDetails
{
    [JsonPropertyName("text")]
    public int Text { get; set; }
    
    [JsonPropertyName("audio")]
    public int Audio { get; set; }
}

public class Output_token_details
{
    [JsonPropertyName("text_tokens")]
    public int TextTokens { get; set; }
    
    [JsonPropertyName("audio_tokens")]
    public int AudioTokens { get; set; }
}


public class Session
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("object")]
    public string Object { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("expires_at")]
    public long ExpiresAt { get; set; }
    
    [JsonPropertyName("modalities")]
    public string[] Modalities { get; set; }
    
    [JsonPropertyName("instructions")]
    public string Instructions { get; set; }
    
    [JsonPropertyName("voice")]
    public string Voice { get; set; }
    
    [JsonPropertyName("turn_detection")]
    public Turn_detection? TurnDetection { get; set; }
    
    [JsonPropertyName("input_audio_format")]
    public string? InputAudioFormat { get; set; }
    
    [JsonPropertyName("output_audio_format")]
    public string? OutputAudioFormat { get; set; }
    
    [JsonPropertyName("input_audio_transcription")]
    public object InputAudioTranscription { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public string ToolChoice { get; set; }
    
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
    
    [JsonPropertyName("max_response_output_tokens")]
    public object MaxResponseOutputTokens { get; set; }
    
    [JsonPropertyName("tools")]
    public object[] Tools { get; set; }
}

public class Turn_detection
{
    public string type { get; set; }
    public double threshold { get; set; }
    public int prefix_padding_ms { get; set; }
    public int silence_duration_ms { get; set; }
}

