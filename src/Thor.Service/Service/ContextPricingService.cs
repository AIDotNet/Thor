using Thor.Service.Domain;

namespace Thor.Service.Service;

/// <summary>
/// 上下文长度定价服务
/// </summary>
public class ContextPricingService
{
    /// <summary>
    /// 根据上下文长度计算定价
    /// </summary>
    /// <param name="model">模型信息</param>
    /// <param name="promptTokens">提示词数</param>
    /// <param name="completionTokens">完成词数</param>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>计算后的定价信息</returns>
    public ContextPricingResult CalculatePricing(ModelManager model, int promptTokens, int completionTokens, int contextLength)
    {
        var result = new ContextPricingResult
        {
            BasePromptCost = model.PromptRate * promptTokens,
            BaseCompletionCost = (model.CompletionRate ?? model.PromptRate) * completionTokens,
            ContextLength = contextLength,
            AppliedTier = null
        };

        // 查找适用的定价层级
        var applicableTier = model.ContextPricingTiers
            .Where(tier => tier.IsInRange(contextLength))
            .OrderBy(tier => tier.MinContextLength)
            .FirstOrDefault();

        if (applicableTier != null)
        {
            result.AppliedTier = applicableTier;
            result.AdjustedPromptCost = result.BasePromptCost * applicableTier.PromptRateMultiplier;
            result.AdjustedCompletionCost = result.BaseCompletionCost * applicableTier.CompletionRateMultiplier;
            result.AdditionalCost = applicableTier.FixedAdditionalCost;
            result.TotalCost = result.AdjustedPromptCost + result.AdjustedCompletionCost + result.AdditionalCost;
        }
        else
        {
            // 如果没有匹配的层级，使用默认价格
            result.AdjustedPromptCost = result.BasePromptCost;
            result.AdjustedCompletionCost = result.BaseCompletionCost;
            result.AdditionalCost = 0;
            result.TotalCost = result.BasePromptCost + result.BaseCompletionCost;
        }

        return result;
    }

    /// <summary>
    /// 获取推荐的上下文长度
    /// </summary>
    /// <param name="model">模型信息</param>
    /// <param name="requestedContextLength">请求的上下文长度</param>
    /// <returns>推荐的上下文长度和对应定价信息</returns>
    public RecommendedContextInfo GetRecommendedContext(ModelManager model, int? requestedContextLength = null)
    {
        var targetLength = requestedContextLength ?? model.DefaultContextLength;
        
        var applicableTier = model.ContextPricingTiers
            .Where(tier => tier.IsInRange(targetLength))
            .OrderBy(tier => tier.MinContextLength)
            .FirstOrDefault();

        return new RecommendedContextInfo
        {
            RecommendedLength = targetLength,
            AppliedTier = applicableTier,
            PricingHint = applicableTier != null 
                ? $"使用{applicableTier.Description}，提示词倍率: {applicableTier.PromptRateMultiplier}，完成词倍率: {applicableTier.CompletionRateMultiplier}"
                : "使用默认定价"
        };
    }
}

/// <summary>
/// 上下文定价结果
/// </summary>
public class ContextPricingResult
{
    /// <summary>基础提示词费用</summary>
    public decimal BasePromptCost { get; set; }
    
    /// <summary>基础完成词费用</summary>
    public decimal BaseCompletionCost { get; set; }
    
    /// <summary>调整后的提示词费用</summary>
    public decimal AdjustedPromptCost { get; set; }
    
    /// <summary>调整后的完成词费用</summary>
    public decimal AdjustedCompletionCost { get; set; }
    
    /// <summary>额外费用</summary>
    public decimal AdditionalCost { get; set; }
    
    /// <summary>总费用</summary>
    public decimal TotalCost { get; set; }
    
    /// <summary>上下文长度</summary>
    public int ContextLength { get; set; }
    
    /// <summary>应用的定价层级</summary>
    public ContextPricingTier? AppliedTier { get; set; }
}

/// <summary>
/// 推荐上下文信息
/// </summary>
public class RecommendedContextInfo
{
    /// <summary>推荐的上下文长度</summary>
    public int RecommendedLength { get; set; }
    
    /// <summary>应用的定价层级</summary>
    public ContextPricingTier? AppliedTier { get; set; }
    
    /// <summary>定价提示</summary>
    public string PricingHint { get; set; } = string.Empty;
}