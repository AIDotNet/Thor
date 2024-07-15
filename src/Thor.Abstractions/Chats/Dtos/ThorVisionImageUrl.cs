using System.Text.Json.Serialization;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 图片消息内容对象
/// </summary>
public class ThorVisionImageUrl
{
    /// <summary>
    /// 图片的url地址，如：https://localhost/logo.jpg ，一般只支持 .png , .jpg .webp .gif
    /// 也可以是base64字符串,如：data:image/jpeg;base64,{base64_image}
    /// 要看底层平台具体要求 
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// 指定图像的细节级别。在愿景指南中了解更多信息。https://platform.openai.com/docs/guides/vision/low-or-high-fidelity-image-understanding
    /// <para>
    /// 指定图像的详细程度。通过控制 detail 参数（该参数具有三个选项： low 、 high 或 auto ），您
    /// 可以控制模型的处理方式图像并生成其文本理解。默认情况下，模型将使用 auto 设置，
    /// 该设置将查看图像输入大小并决定是否应使用 low 或 high 设置。
    /// </para>
    /// </summary>
    [JsonPropertyName("detail")]
    public string? Detail { get; set; } = "auto";

}