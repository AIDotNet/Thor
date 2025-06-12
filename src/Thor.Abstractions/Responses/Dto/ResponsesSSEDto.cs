using System.Text.Json.Serialization;

namespace Thor.Abstractions.Responses.Dto;

public class ResponsesSSEDto<T>
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("sequence_number")] public int SequenceNumber { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("response")]
    public T Response { get; set; }

    [JsonPropertyName("delta")] public string? Delta { get; set; }

    [JsonPropertyName("content_index")] public int? ContentIndex { get; set; }

    [JsonPropertyName("output_index")] public int? OutputIndex { get; set; }

    [JsonPropertyName("item_id")] public string? ItemId { get; set; }

    [JsonPropertyName("text")] public string? Text { get; set; }


    [JsonPropertyName("part")] public ResponsesSSEDtoPart? Parts { get; set; }

    [JsonPropertyName("item")] public ResponsesSSEDtoItem? Item { get; set; }
}

public class ResponsesSSEDtoPart
{
    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("annotations")] public object[]? Annotations { get; set; }

    [JsonPropertyName("text")] public string? Text { get; set; }
}

public class ResponsesSSEDtoItem
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("content")] public ResponsesSSEDtoPart[] Content { get; set; }
    
    [JsonPropertyName("role")]
    public string Role { get; set; }
}