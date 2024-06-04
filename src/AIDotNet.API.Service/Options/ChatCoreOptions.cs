namespace AIDotNet.API.Service.Options;

public class ChatCoreOptions
{
    public const string Name = "Chat";

    /// <summary>
    /// 支持vision的模型列表
    /// </summary>
    public static string[] VisionModel { get; set; }
}