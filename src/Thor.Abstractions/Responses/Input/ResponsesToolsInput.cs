using System.Text.Json.Serialization;
using Thor.Abstractions.Chats.Dtos;

namespace Thor.Abstractions.Responses;

public class ResponsesToolsInput
{
    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("vector_store_ids")] public List<string>? VectorStoreIds { get; set; }

    [JsonPropertyName("max_num_results")] public decimal? MaxNumResults { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    /// <summary>
    ///     函数接受的参数，描述为 JSON 架构对象。有关示例，请参阅指南，有关格式的文档，请参阅 JSON 架构参考。
    ///     省略 parameters 定义一个参数列表为空的函数。
    ///     See the <a href="https://platform.openai.com/docs/guides/gpt/function-calling">guide</a> for examples,
    ///     and the <a href="https://json-schema.org/understanding-json-schema/">JSON Schema reference</a> for
    ///     documentation about the format.
    /// </summary>
    [JsonPropertyName("parameters")]
    public ThorToolFunctionPropertyDefinition? Parameters { get; set; }

    [JsonPropertyName("server_label")] public string? ServerLabel { get; set; }

    [JsonPropertyName("server_url")] public string? ServerUrl { get; set; }

    [JsonPropertyName("allowed_tools")] public List<object>? AllowedTools { get; set; }

    [JsonPropertyName("headers")] public object? Headers { get; set; }

    [JsonPropertyName("require_approval")] public object? RequireApproval { get; set; }

    [JsonPropertyName("filters")] public object? Filters { get; set; }

    [JsonPropertyName("ranking_options")] public ResponsesToolsRankingOptions? RankingOptions { get; set; }

    [JsonPropertyName("search_context_size")]
    public string? SearchContextSize { get; set; }

    [JsonPropertyName("user_location")] public ResponsesToolsInputUserLocation? UserLocation { get; set; }

    [JsonPropertyName("display_height")] public int? DisplayHeight { get; set; }

    [JsonPropertyName("display_width")] public int? DisplayWidth { get; set; }

    /// <summary>
    /// The type of computer environment to control.
    /// </summary>
    [JsonPropertyName("environment")]
    public string? Environment { get; set; }

    [JsonPropertyName("container")] public string? Container { get; set; }

    /// <summary>
    /// Background type for the generated image. One of transparent, opaque, or auto. Default: auto.
    /// </summary>
    [JsonPropertyName("background")]
    public string? Background { get; set; }

    [JsonPropertyName("input_image_mask")] public ResponsesToolsInputImageMask? InputImageMask { get; set; }

    [JsonPropertyName("model")] public string? Model { get; set; }

    /// <summary>
    /// Moderation level for the generated image. Default: auto.
    /// </summary>
    [JsonPropertyName("moderation")]
    public string? Moderation { get; set; }

    /// <summary>
    /// Compression level for the output image. Default: 100.
    /// </summary>
    [JsonPropertyName("output_compression")]
    public string? OutputCompression { get; set; }

    /// <summary>
    /// The output format of the generated image. One of png, webp, or jpeg. Default: png.
    /// </summary>
    [JsonPropertyName("output_format")]
    public string? OutputFormat { get; set; }

    [JsonPropertyName("partial_images")] public int? PartialImages { get; set; }

    /// <summary>
    /// The quality of the generated image. One of low, medium, high, or auto. Default: auto.
    /// </summary>
    [JsonPropertyName("quality")]
    public string? Quality { get; set; }

    [JsonPropertyName("size")] public string? Size { get; set; }
}

public class ResponsesToolsInputImageMask
{
    [JsonPropertyName("file_id")] public string? FileId { get; set; }

    [JsonPropertyName("image_url")] public string? ImageUrl { get; set; }
}

public class ResponsesToolsInputUserLocation
{
    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("city")] public string? City { get; set; }

    [JsonPropertyName("country")] public string? Country { get; set; }

    [JsonPropertyName("region")] public string? Region { get; set; }

    [JsonPropertyName("timezone")] public string? Timezone { get; set; }
}

public class ResponsesToolsRankingOptions
{
    [JsonPropertyName("ranker")] public string? Ranker { get; set; }

    [JsonPropertyName("score_threshold")] public int? ScoreThreshold { get; set; }
}