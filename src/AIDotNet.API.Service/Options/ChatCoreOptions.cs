namespace AIDotNet.API.Service.Options;

public class ChatCoreOptions
{
    public const string Name = "Chat";

    /// <summary>
    /// 支持vision的模型列表
    /// </summary>
    public static string[] VisionModel { get; set; }

    /// <summary>
    /// 主节点地址
    /// 一般子节点并没有提供Url，所以需要主节点来提供然后/跟地址重定向到主节点
    /// </summary>
    public static string Master { get; set; }
}