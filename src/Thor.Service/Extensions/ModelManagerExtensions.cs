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
    /// <returns>费用</returns>
    public static decimal CalculatePromptCost(this ModelManager model, int tokenCount)
    {
        if (model.TieredPricing?.Enabled == true)
        {
            return TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.PromptTiers, model.PromptRate);
        }
        
        return tokenCount * model.PromptRate;
    }
    
    /// <summary>
    /// 计算完成词费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <returns>费用</returns>
    public static decimal CalculateCompletionCost(this ModelManager model, int tokenCount)
    {
        if (model.TieredPricing?.Enabled == true)
        {
            return TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.CompletionTiers, model.CompletionRate ?? model.PromptRate);
        }
        
        return tokenCount * (model.CompletionRate ?? model.PromptRate);
    }
    
    /// <summary>
    /// 计算缓存费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <returns>费用</returns>
    public static decimal CalculateCacheCost(this ModelManager model, int tokenCount)
    {
        if (model.TieredPricing?.Enabled == true)
        {
            return TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.CacheTiers, model.CacheRate ?? model.PromptRate);
        }
        
        return tokenCount * (model.CacheRate ?? model.PromptRate);
    }
    
    /// <summary>
    /// 计算音频提示词费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <returns>费用</returns>
    public static decimal CalculateAudioPromptCost(this ModelManager model, int tokenCount)
    {
        if (model.TieredPricing?.Enabled == true)
        {
            return TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.AudioPromptTiers, model.AudioPromptRate ?? model.PromptRate);
        }
        
        return tokenCount * (model.AudioPromptRate ?? model.PromptRate);
    }
    
    /// <summary>
    /// 计算音频输出费用
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <returns>费用</returns>
    public static decimal CalculateAudioOutputCost(this ModelManager model, int tokenCount)
    {
        if (model.TieredPricing?.Enabled == true)
        {
            return TieredPricingHelper.CalculateTieredPrice(tokenCount, model.TieredPricing.AudioOutputTiers, model.AudioOutputRate ?? model.PromptRate);
        }
        
        return tokenCount * (model.AudioOutputRate ?? model.PromptRate);
    }
    
    /// <summary>
    /// 获取提示词费率（用于兼容现有代码）
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <returns>费率</returns>
    public static decimal GetPromptRate(this ModelManager model, int tokenCount)
    {
        if (model.TieredPricing?.Enabled == true)
        {
            return TieredPricingHelper.GetApplicableRate(tokenCount, model.TieredPricing.PromptTiers, model.PromptRate);
        }
        
        return model.PromptRate;
    }
    
    /// <summary>
    /// 获取完成词费率（用于兼容现有代码）
    /// </summary>
    /// <param name="model">模型管理器</param>
    /// <param name="tokenCount">token数量</param>
    /// <returns>费率</returns>
    public static decimal GetCompletionRate(this ModelManager model, int tokenCount)
    {
        if (model.TieredPricing?.Enabled == true)
        {
            return TieredPricingHelper.GetApplicableRate(tokenCount, model.TieredPricing.CompletionTiers, model.CompletionRate ?? model.PromptRate);
        }
        
        return model.CompletionRate ?? model.PromptRate;
    }
}