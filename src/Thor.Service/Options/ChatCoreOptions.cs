namespace Thor.Service.Options;

public class ChatCoreOptions
{
    public const string Name = "Chat";

    /// <summary>
    /// 主节点地址
    /// 一般子节点并没有提供Url，所以需要主节点来提供然后/跟地址重定向到主节点
    /// </summary>
    public static string Master { get; set; }

    public static Shared? Invite { get; set; }

    public static FreeModel? FreeModel { get; set; }

    public static ModelMapping ModelMapping { get; set; }
}

/// <summary>
/// 免费模型
/// </summary>
public sealed class FreeModel
{
    /// <summary>
    /// 是否启用免费模式
    /// </summary>
    public bool EnableFree { get; set; }

    /// <summary>
    /// 免费模型列表
    /// </summary>
    public FreeModelItem[]? Items { get; set; }
}

/// <summary>
/// 免费模型列表
/// </summary>
public sealed class FreeModelItem
{
    /// <summary>
    /// 模型
    /// </summary>
    public string[] Model { get; set; }

    /// <summary>
    /// 免费次数（超出免费额度则按照收费）
    /// </summary>
    public int Limit { get; set; }
}

/// <summary>
/// 分享配置
/// </summary>
public sealed class Shared
{
    /// <summary>
    /// 是否启用邀请奖励
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 邀请奖励
    /// </summary>
    public int Credit { get; set; }

    /// <summary>
    /// 邀请奖励上限
    /// </summary>
    public int Limit { get; set; }
}

public sealed class ModelMapping
{
    /// <summary>
    /// 是否启用模型映射
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 模型映射配置
    /// 映射规则，
    /// 请求model:["",""] // value则是需要映射的模型，可以配置多个，自动负载均衡
    /// </summary>
    public Dictionary<string, ModelMappingItem[]> Models { get; set; } = new();
}

public sealed class ModelMappingItem
{
    public string Model { get; set; }

    /// <summary>
    /// 权重，数值越大使用概率越大
    /// </summary>
    public int Order { get; set; } = 1;
}