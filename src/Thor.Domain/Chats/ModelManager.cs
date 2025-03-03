using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

/// <summary>
/// 模型倍率管理
/// </summary>
public sealed class ModelManager : Entity<Guid>
{
    /// <summary>
    /// 模型
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 模型描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 是否可用
    /// </summary>
    public bool Available { get; set; }

    /// <summary>
    /// 模型完成倍率
    /// </summary>
    public decimal? CompletionRate { get; set; }

    /// <summary>
    /// 模型提示倍率
    /// </summary>
    public decimal PromptRate { get; set; }

    /// <summary>
    /// Audio倍率
    /// </summary>
    public decimal? AudioPromptRate { get; set; }

    /// <summary>
    /// Audio输出倍率
    /// </summary>
    public decimal? AudioOutputRate { get; set; }

    /// <summary>
    /// 写入缓存倍率
    /// </summary>
    public decimal? CacheRate { get; set; }

    /// <summary>
    /// 缓存命中倍率
    /// </summary>
    /// <returns></returns>
    public decimal? CacheHitRate { get; set; }

    /// <summary>
    /// Audio缓存倍率
    /// </summary>
    public decimal? AudioCacheRate { get; set; }

    /// <summary>
    /// quota_type
    /// </summary>
    public ModelQuotaType QuotaType { get; set; }

    /// <summary>
    /// 模型额度最大上文
    /// </summary>
    public string? QuotaMax { get; set; }

    /// <summary>
    /// 模型标签
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// 模型图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 是否v2版本
    /// </summary>
    public bool IsVersion2 { get; set; } = false;
}