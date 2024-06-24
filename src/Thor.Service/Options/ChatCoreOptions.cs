namespace Thor.Service.Options;

public class ChatCoreOptions
{
    public const string Name = "Chat";

    /// <summary>
    /// 主节点地址
    /// 一般子节点并没有提供Url，所以需要主节点来提供然后/跟地址重定向到主节点
    /// </summary>
    public static string Master { get; set; }

    public static Shared? Shared { get; set; }
}

public sealed class Shared
{
    /// <summary>
    /// 是否启用分享广告
    /// </summary>
    public bool EnableShareAd { get; set; }

    /// <summary>
    /// 分享每次点击奖励（一个ip算一次）
    /// </summary>
    public int ShareCredit { get; set; }

    /// <summary>
    /// 分享领取限制（每个人每天的限制）
    /// </summary>
    public int ShareLimit { get; set; }
}