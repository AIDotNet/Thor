using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

/// <summary>
/// 上下文长度定价层级
/// </summary>
public class ContextPricingTier
{
    /// <summary>
    /// 最小上下文长度（tokens）
    /// </summary>
    public int MinContextLength { get; set; }

    /// <summary>
    /// 最大上下文长度（tokens）
    /// </summary>
    public int MaxContextLength { get; set; }

    /// <summary>
    /// 提示词倍率（相对于基础价格）
    /// </summary>
    public decimal PromptRateMultiplier { get; set; } = 1.0m;

    /// <summary>
    /// 完成词倍率（相对于基础价格）
    /// </summary>
    public decimal CompletionRateMultiplier { get; set; } = 1.0m;

    /// <summary>
    /// 固定额外费用
    /// </summary>
    public decimal FixedAdditionalCost { get; set; } = 0.0m;

    /// <summary>
    /// 层级描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 检查上下文长度是否在此层级范围内
    /// </summary>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>是否匹配</returns>
    public bool IsInRange(int contextLength)
    {
        return contextLength >= MinContextLength && contextLength <= MaxContextLength;
    }
}