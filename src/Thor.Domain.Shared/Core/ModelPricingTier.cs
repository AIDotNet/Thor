namespace Thor.Service.Domain.Core;

/// <summary>
/// 模型定价层级
/// </summary>
public class ModelPricingTier
{
    /// <summary>
    /// 阈值 (token数量上限，-1表示无限制)
    /// </summary>
    public int Threshold { get; set; }
    
    /// <summary>
    /// 该层级的费率
    /// </summary>
    public decimal Rate { get; set; }
    
    /// <summary>
    /// 层级描述
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// 模型分层定价配置
/// </summary>
public class ModelTieredPricing
{
    /// <summary>
    /// 是否启用分层定价
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// 提示词分层定价
    /// </summary>
    public List<ModelPricingTier> PromptTiers { get; set; } = new();
    
    /// <summary>
    /// 完成词分层定价
    /// </summary>
    public List<ModelPricingTier> CompletionTiers { get; set; } = new();
    
    /// <summary>
    /// 缓存分层定价
    /// </summary>
    public List<ModelPricingTier> CacheTiers { get; set; } = new();
    
    /// <summary>
    /// 音频提示词分层定价
    /// </summary>
    public List<ModelPricingTier> AudioPromptTiers { get; set; } = new();
    
    /// <summary>
    /// 音频输出分层定价
    /// </summary>
    public List<ModelPricingTier> AudioOutputTiers { get; set; } = new();
}