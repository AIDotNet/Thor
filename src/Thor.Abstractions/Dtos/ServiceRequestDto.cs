namespace Thor.Abstractions.Dtos;

/// <summary>
/// 请求服务记录
/// </summary>
public class ServiceRequestDto
{
    /// <summary>
    /// 接口地址
    /// </summary>
    public string ApiEndpoint { get; set; }

    public string ApiName => GetServiceType(ApiEndpoint);

    /// <summary>
    /// 请求数
    /// </summary>
    public long RequestCount { get; set; }

    /// <summary>
    /// 消耗Token数
    /// </summary>
    public long TokenCount { get; set; }

    /// <summary>
    /// 消耗图片数
    /// </summary>
    public long ImageCount { get; set; }

    /// <summary>
    /// 时间戳 (精确到天)
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 模型名称
    /// </summary>
    public string ModelName { get; set; }

    /// <summary>
    /// 消费额度
    /// </summary>
    public long Cost { get; set; }


    /// <summary>
    /// 根据Url确定服务类型
    /// </summary>
    private static string GetServiceType(string url)
    {
        if (string.IsNullOrEmpty(url))
            return "未知服务";

        if (url.Contains("/v1/chat/completions"))
            return "聊天完成";

        if (url.Contains("/v1/images/generations") ||
            url.Contains("/v1/images/edits") ||
            url.Contains("/v1/images/variations"))
            return "图片服务";

        if (url.Contains("/v1/embeddings"))
            return "嵌入服务";

        if (url.Contains("/v1/organization/usage/audio_speeches"))
            return "Audio Speeches";

        if (url.Contains("/v1/organization/usage/audio_transcriptions"))
            return "Audio Transcriptions";

        if (url.Contains("/v1/audio/speech") ||
            url.Contains("/v1/audio/transcriptions") ||
            url.Contains("/v1/audio/translations"))
            return "音频服务";

        return "其他服务";
    }
}