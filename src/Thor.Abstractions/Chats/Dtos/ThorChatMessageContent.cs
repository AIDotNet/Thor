using System.Text.Json.Serialization;
using Thor.Abstractions.Chats.Consts;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 发出的消息内容，包含图文，一般是一文一图，一文多图两种情况，请使用CreeateXXX系列方法构建内容
/// </summary>
public class ThorChatMessageContent
{
    public ThorChatMessageContent()
    {

    }

    /// <summary>
    /// 消息内容类型，只能使用<see cref="ThorMessageContentTypeConst"/> 定义的值赋值，如：ThorMessageContentTypeConst.Text
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// 消息内容类型为 text 时候的赋值，如：图片上描述了什么
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// 消息内容类型为 image_url 时候的赋值
    /// </summary>
    [JsonPropertyName("image_url")]
    public ThorVisionImageUrl? ImageUrl { get; set; }

    /// <summary>
    /// 音频消息内容，包含音频数据和格式信息。
    /// </summary>
    [JsonPropertyName("input_audio")]
    public ThorChatMessageAudioContent? InputAudio { get; set; }

    /// <summary>
    /// 创建文本类消息
    /// <param name="text">文本内容</param>
    /// </summary>
    public static ThorChatMessageContent CreateTextContent(string text)
    {
        return new()
        {
            Type = ThorMessageContentTypeConst.Text,
            Text = text
        };
    }

    /// <summary>
    /// 创建图片类消息，图片url形式
    /// <param name="imageUrl">图片 url</param>
    /// <param name="detail">指定图像的详细程度。通过控制 detail 参数（该参数具有三个选项： low 、 high 或 auto ），您
    /// 可以控制模型的处理方式图像并生成其文本理解。默认情况下，模型将使用 auto 设置，
    /// 该设置将查看图像输入大小并决定是否应使用 low 或 high 设置。</param>
    /// </summary>
    public static ThorChatMessageContent CreateImageUrlContent(string imageUrl, string? detail = "auto")
    {
        return new()
        {
            Type = ThorMessageContentTypeConst.ImageUrl,
            ImageUrl = new()
            {
                Url = imageUrl,
                Detail = detail
            }
        };
    }

    /// <summary>
    /// 创建图片类消息,字节流转base64字符串形式
    /// <param name="binaryImage">The image binary data as byte array</param>
    /// <param name="imageType">图片类型，如 png,jpg</param>
    /// <param name="detail">指定图像的详细程度。</param>
    /// </summary>
    public static ThorChatMessageContent CreateImageBinaryContent(
        byte[] binaryImage,
        string imageType,
        string? detail = "auto"
    )
    {
        return new()
        {
            Type = ThorMessageContentTypeConst.ImageUrl,
            ImageUrl = new()
            {
                Url = string.Format(
                    "data:image/{0};base64,{1}",
                    imageType,
                    Convert.ToBase64String(binaryImage)
                ),
                Detail = detail
            }
        };
    }
}
