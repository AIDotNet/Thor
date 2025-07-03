using Thor.Service.Domain;
using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure.Helper;

namespace Thor.Service.Extensions;

/// <summary>
/// 模型管理器扩展方法
/// </summary>
public static class ModelManagerExtensions
{
    /// <summary>
    /// 计算提示词费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>费用</returns>
    public static decimal CalculatePromptCost(this ModelManager model, int tokenCount, int contextLength = 0)
    {
        var baseCost = model.TieredPricing?.Enabled == true
            ? TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.PromptTiers, model.PromptRate)
            : tokenCount * model.PromptRate;
            
        return ApplyContextPricing(model, baseCost, contextLength, model.PromptRate);
    }
    
    /// <summary>
    /// 计算完成词费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>费用</returns>
    public static decimal CalculateCompletionCost(this ModelManager model, int tokenCount, int contextLength = 0)
    {
        var baseRate = model.CompletionRate ?? model.PromptRate;
        var baseCost = model.TieredPricing?.Enabled == true
            ? TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.CompletionTiers, baseRate)
            : tokenCount * baseRate;
            
        return ApplyContextPricing(model, baseCost, contextLength, baseRate);
    }
    
    /// <summary>
    /// 计算缓存费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>费用</returns>
    public static decimal CalculateCacheCost(this ModelManager model, int tokenCount, int contextLength = 0)
    {
        var baseRate = model.CacheRate ?? model.PromptRate;
        var baseCost = model.TieredPricing?.Enabled == true
            ? TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.CacheTiers, baseRate)
            : tokenCount * baseRate;
            
        return ApplyContextPricing(model, baseCost, contextLength, baseRate);
    }
    
    /// <summary>
    /// 计算音频提示词费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>费用</returns>
    public static decimal CalculateAudioPromptCost(this ModelManager model, int tokenCount, int contextLength = 0)
    {
        var baseRate = model.AudioPromptRate ?? model.PromptRate;
        var baseCost = model.TieredPricing?.Enabled == true
            ? TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.AudioPromptTiers, baseRate)
            : tokenCount * baseRate;
            
        return ApplyContextPricing(model, baseCost, contextLength, baseRate);
    }
    
    /// <summary>
    /// 计算音频输出费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>费用</returns>
    public static decimal CalculateAudioOutputCost(this ModelManager model, int tokenCount, int contextLength = 0)
    {
        var baseRate = model.AudioOutputRate ?? model.PromptRate;
        var baseCost = model.TieredPricing?.Enabled == true
            ? TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.AudioOutputTiers, baseRate)
            : tokenCount * baseRate;
            
        return ApplyContextPricing(model, baseCost, contextLength, baseRate);
    }
    
    /// <summary>
    /// 获取提示词费率（用于兼容现有代码）
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>费率</returns>
    public static decimal GetPromptRate(this ModelManager model, int tokenCount, int contextLength = 0)
    {
        var baseRate = model.TieredPricing?.Enabled == true
            ? TieredPricingHelper.GetApplicableRate(tokenCount, model.TieredPricing.PromptTiers, model.PromptRate)
            : model.PromptRate;
            
        return ApplyContextRate(model, baseRate, contextLength, model.PromptRate);
    }
    
    /// <summary>
    /// 获取完成词费率（用于兼容现有代码）
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <param name="contextLength">上下文长度</param>
    /// <returns>费率</returns>
    public static decimal GetCompletionRate(this ModelManager model, int tokenCount, int contextLength = 0)
    {
        var baseRate = model.CompletionRate ?? model.PromptRate;
        var rate = model.TieredPricing?.Enabled == true
            ? TieredPricingHelper.GetApplicableRate(tokenCount, model.TieredPricing.CompletionTiers, baseRate)
            : baseRate;
            
        return ApplyContextRate(model, rate, contextLength, baseRate);
    }
    
    /// <summary>
    /// 应用上下文定价
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="baseCost">基础费用</param>
    /// <param name="contextLength">上下文长度</param>
    /// <param name="baseRate">基础费率</param>
    /// <returns>应用上下文定价后的费用</returns>
    private static decimal ApplyContextPricing(ModelManager model, decimal baseCost, int contextLength, decimal baseRate)
    {
        if (model.ContextPricing?.Enabled != true || contextLength <= 0)
            return baseCost;
            
        var contextMultiplier = TieredPricingHelper.GetApplicableRate(contextLength, model.ContextPricing.ContextTiers, 1.0m);
        
        return model.ContextPricing.PricingType switch
        {
            ContextPricingType.Multiplier => baseCost * contextMultiplier,
            ContextPricingType.Replacement => (baseCost / baseRate) * contextMultiplier,
            _ => baseCost
        };
    }
    
    /// <summary>
    /// 应用上下文费率
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="baseRate">基础费率</param>
    /// <param name="contextLength">上下文长度</param>
    /// <param name="fallbackRate">回退费率</param>
    /// <returns>应用上下文定价后的费率</returns>
    private static decimal ApplyContextRate(ModelManager model, decimal baseRate, int contextLength, decimal fallbackRate)
    {
        if (model.ContextPricing?.Enabled != true || contextLength <= 0)
            return baseRate;
            
        var contextMultiplier = TieredPricingHelper.GetApplicableRate(contextLength, model.ContextPricing.ContextTiers, 1.0m);
        
        return model.ContextPricing.PricingType switch
        {
            ContextPricingType.Multiplier => baseRate * contextMultiplier,
            ContextPricingType.Replacement => contextMultiplier,
            _ => baseRate
        };
    }
    
    // Backward compatibility methods
    /// <summary>
    /// 计算提示词费用（向后兼容）
    /// </summary>
    public static decimal CalculatePromptCost(this ModelManager model, int tokenCount) 
        => model.CalculatePromptCost(tokenCount, 0);
        
    /// <summary>
    /// 计算完成词费用（向后兼容）
    /// </summary>
    public static decimal CalculateCompletionCost(this ModelManager model, int tokenCount) 
        => model.CalculateCompletionCost(tokenCount, 0);
        
    /// <summary>
    /// 计算缓存费用（向后兼容）
    /// </summary>
    public static decimal CalculateCacheCost(this ModelManager model, int tokenCount) 
        => model.CalculateCacheCost(tokenCount, 0);
        
    /// <summary>
    /// 计算音频提示词费用（向后兼容）
    /// </summary>
    public static decimal CalculateAudioPromptCost(this ModelManager model, int tokenCount) 
        => model.CalculateAudioPromptCost(tokenCount, 0);
        
    /// <summary>
    /// 计算音频输出费用（向后兼容）
    /// </summary>
    public static decimal CalculateAudioOutputCost(this ModelManager model, int tokenCount) 
        => model.CalculateAudioOutputCost(tokenCount, 0);
        
    /// <summary>
    /// 获取提示词费率（向后兼容）
    /// </summary>
    public static decimal GetPromptRate(this ModelManager model, int tokenCount) 
        => model.GetPromptRate(tokenCount, 0);
        
    /// <summary>
    /// 获取完成词费率（向后兼容）
    /// </summary>
    public static decimal GetCompletionRate(this ModelManager model, int tokenCount) 
        => model.GetCompletionRate(tokenCount, 0);
}