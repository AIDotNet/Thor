using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thor.Abstractions.Responses;

public class ResponsesMessageInput
{
    [JsonPropertyName("role")] public string Role { get; set; }

    [JsonPropertyName("content")]
    public object ContentCalculated
    {
        get
        {
            if (Content is not null && Contents is not null)
            {
                throw new ValidationException("Messages 中 Content 和 Contents 字段不能同时有值");
            }

            if (Content is not null)
            {
                return Content;
            }

            return Content!;
        }
        set
        {
            if (value is JsonElement str)
            {
                if (str.ValueKind == JsonValueKind.String)
                {
                    Content = value?.ToString();
                }
                else if (str.ValueKind == JsonValueKind.Array)
                {
                    Contents = JsonSerializer.Deserialize<IList<ResponsesMessageContentInput>>(value?.ToString());
                }
            }
            else
            {
                Content = value?.ToString();
            }
        }
    }

    [JsonIgnore] public string? Content { get; set; }

    [JsonIgnore]
    public bool IsMessageArray
    {
        get
        {
            if (string.IsNullOrEmpty(Content))
            {
                return false;
            }

            return true;
        }
    }

    [JsonIgnore] public IList<ResponsesMessageContentInput>? Contents { get; set; }
}

public class ResponsesMessageContentInput
{
    [JsonPropertyName("type")] public string Type { get; set; }
    
    [JsonPropertyName("image_url")] public string? ImageUrl { get; set; }
    
    [JsonPropertyName("text")] public string? Text { get; set; }
}
