using System.Text.Json.Serialization;

namespace Thor.Abstractions.Chats.Dtos;

public class ThorChatClaudeThinking
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } 
    
    [JsonPropertyName("budget_tokens")]
    public int? BudgetToken { get; set; }
}