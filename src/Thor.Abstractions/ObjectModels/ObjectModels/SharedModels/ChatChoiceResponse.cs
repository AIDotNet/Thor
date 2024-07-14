using System.Text.Json.Serialization;
using OpenAI.ObjectModels.RequestModels;
using Thor.Abstractions.Chats.Dtos;

namespace Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

public record ChatChoiceResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("delta")]
    public ThorChatMessage Delta
    {
        get => Message;
        set => Message = value;
    }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("message")] 
    public ThorChatMessage Message { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("index")] 
    public int? Index { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("finish_reason")] 
    public string FinishReason { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("finish_details")] 
    public FinishDetailsResponse? FinishDetails { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public class FinishDetailsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("stop")]
        public string Stop { get; set; }
    }
}