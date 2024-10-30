using System.Text.Json.Serialization;
using Thor.Abstractions.Chats.Dtos;

namespace Thor.Abstractions.Realtime.Dto;

public class RealtimeInput
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("audio")]
    public string? Audio { get; set; }
    
    [JsonPropertyName("event_id")] 
    public string EventId { get; set; }
    
    [JsonPropertyName("session")] 
    public RealtimeInputSession? Session { get; set; }

    [JsonPropertyName("item")]
    public RealtimeItem? Item { get; set; }
}

public class RealtimeInputSession
{
    [JsonPropertyName("modalities")] 
    public string[] Modalities { get; set; }
    
    [JsonPropertyName("instructions")]
    public string Instructions { get; set; }
    
    [JsonPropertyName("voice")]
    public string Voice { get; set; }
    
    [JsonPropertyName("input_audio_format")]
    public string? InputAudioFormat { get; set; }
    
    [JsonPropertyName("output_audio_format")]
    public string? OutputAudioFormat { get; set; }
    
    [JsonPropertyName("input_audio_transcription")]
    public InputAudioTranscriptionItem? InputAudioTranscription { get; set; }
    
    [JsonPropertyName("turn_detection")]
    public TurnDetectionItem? TurnDetection { get; set; }
    
    [JsonPropertyName("tools")]
    public ThorToolFunctionDefinition[]? Tools { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public string? tool_choice { get; set; }
    
    [JsonPropertyName("temperature")]
    public double? temperature { get; set; }
    
    [JsonPropertyName("max_response_output_tokens")]
    public int? MaxResponseOutputTokens { get; set; }
}

public class InputAudioTranscriptionItem
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
}

public class TurnDetectionItem
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("threshold")]
    public double Threshold { get; set; }
    
    [JsonPropertyName("prefix_padding_ms")]
    public int PrefixPaddingMs { get; set; }
    
    [JsonPropertyName("silence_duration_ms")]
    public int SilenceDurationMs { get; set; }
    
}
