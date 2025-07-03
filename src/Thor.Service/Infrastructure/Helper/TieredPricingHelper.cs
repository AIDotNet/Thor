using Thor.Service.Domain.Core;

namespace Thor.Service.Infrastructure.Helper;

/// <summary>
/// 分层定价计算助手
/// </summary>
public static class TieredPricingHelper
{
    /// <summary>
    /// 计算分层定价
    /// </summary>
    /// <param name="tokenCount">token数量</param>
    /// <param name="tiers">价格层级列表</param>
    /// <param name="fallbackRate">回退费率（当未启用分层定价时使用）</param>
    /// <returns>计算后的费用</returns>
    public static decimal CalculateTieredPrice(int tokenCount, List<ModelPricingTier> tiers, decimal fallbackRate)
    {
        // 如果没有配置分层定价，使用回退费率
        if (tiers == null || tiers.Count == 0)
        {
            return tokenCount * fallbackRate;
        }
        
        // 按阈值排序（升序，-1表示无限制放到最后）
        var sortedTiers = tiers.OrderBy(t => t.Threshold == -1 ? int.MaxValue : t.Threshold).ToList();
        
        decimal totalCost = 0;
        int processedTokens = 0;
        
        foreach (var tier in sortedTiers)
        {
            // 确定当前层级的上限
            int tierLimit = tier.Threshold == -1 ? tokenCount : tier.Threshold;
            
            // 计算在当前层级的token数量
            int tokensInThisTier = Math.Min(tokenCount - processedTokens, tierLimit - processedTokens);
            
            if (tokensInThisTier <= 0)
                break;
            
            // 计算当前层级的费用
            totalCost += tokensInThisTier * tier.Rate;
            processedTokens += tokensInThisTier;
            
            // 如果已经处理完所有token，跳出循环
            if (processedTokens >= tokenCount)
                break;
        }
        
        return totalCost;
    }
    
    /// <summary>
    /// 获取指定token数量下的费率
    /// </summary>
    /// <param name="tokenCount">token数量</param>
    /// <param name="tiers">价格层级列表</param>
    /// <param name="fallbackRate">回退费率</param>
    /// <returns>适用的费率</returns>
    public static decimal GetApplicableRate(int tokenCount, List<ModelPricingTier> tiers, decimal fallbackRate)
    {
        // 如果没有配置分层定价，使用回退费率
        if (tiers == null || tiers.Count == 0)
        {
            return fallbackRate;
        }
        
        // 按阈值排序（升序，-1表示无限制放到最后）
        var sortedTiers = tiers.OrderBy(t => t.Threshold == -1 ? int.MaxValue : t.Threshold).ToList();
        
        // 找到适用的层级
        foreach (var tier in sortedTiers)
        {
            if (tier.Threshold == -1 || tokenCount <= tier.Threshold)
            {
                return tier.Rate;
            }
        }
        
        // 如果没有找到适用的层级，使用最后一个层级的费率
        return sortedTiers.Count > 0 ? sortedTiers.Last().Rate : fallbackRate;
    }
    
    /// <summary>
    /// 创建分层定价配置模板
    /// </summary>
    /// <returns>分层定价配置模板</returns>
    public static ModelTieredPricing CreateTieredPricingTemplate()
    {
        return new ModelTieredPricing
        {
            Enabled = true,
            PromptTiers = new List<ModelPricingTier>
            {
                new ModelPricingTier { Threshold = 200000, Rate = 1.25m, Description = "≤200K tokens" },
                new ModelPricingTier { Threshold = -1, Rate = 2.50m, Description = ">200K tokens" }
            },
            CompletionTiers = new List<ModelPricingTier>
            {
                new ModelPricingTier { Threshold = 200000, Rate = 10.00m, Description = "≤200K tokens" },
                new ModelPricingTier { Threshold = -1, Rate = 15.00m, Description = ">200K tokens" }
            },
            CacheTiers = new List<ModelPricingTier>
            {
                new ModelPricingTier { Threshold = 200000, Rate = 0.31m, Description = "≤200K tokens" },
                new ModelPricingTier { Threshold = -1, Rate = 0.625m, Description = ">200K tokens" }
            }
        };
    }
    
    /// <summary>
    /// 创建上下文定价配置模板
    /// </summary>
    /// <returns>上下文定价配置</returns>
    public static ModelContextPricing CreateContextPricingTemplate()
    {
        return new ModelContextPricing
        {
            Enabled = true,
            PricingType = ContextPricingType.Multiplier,
            ContextTiers = new List<ModelPricingTier>
            {
                new ModelPricingTier { Threshold = 4000, Rate = 1.0m, Description = "短上下文 ≤4K tokens" },
                new ModelPricingTier { Threshold = 16000, Rate = 1.2m, Description = "中等上下文 4K-16K tokens" },
                new ModelPricingTier { Threshold = 32000, Rate = 1.5m, Description = "长上下文 16K-32K tokens" },
                new ModelPricingTier { Threshold = -1, Rate = 2.0m, Description = "超长上下文 >32K tokens" }
            }
        };
    }
}