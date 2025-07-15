using Thor.Service.Domain;

namespace Thor.Service.Extensions;

/// <summary>
/// 模型管理器扩展方法
/// </summary>
public static class ModelManagerExtensions
{
    /// <summary>
    /// 初始化默认的上下文定价层级
    /// </summary>
    /// <param name="model">模型管理器</param>
    public static void InitializeDefaultContextPricingTiers(this ModelManager model)
    {
        if (model.ContextPricingTiers.Count == 0)
        {
            // 根据不同的上下文长度设置默认定价层级
            model.ContextPricingTiers = new List<ContextPricingTier>
            {
                new ContextPricingTier
                {
                    MinContextLength = 0,
                    MaxContextLength = 4096,
                    PromptRateMultiplier = 1.0m,
                    CompletionRateMultiplier = 1.0m,
                    FixedAdditionalCost = 0.0m,
                    Description = "标准上下文（0-4K tokens）"
                },
                new ContextPricingTier
                {
                    MinContextLength = 4097,
                    MaxContextLength = 8192,
                    PromptRateMultiplier = 1.2m,
                    CompletionRateMultiplier = 1.1m,
                    FixedAdditionalCost = 0.0m,
                    Description = "长上下文（4K-8K tokens）"
                },
                new ContextPricingTier
                {
                    MinContextLength = 8193,
                    MaxContextLength = 16384,
                    PromptRateMultiplier = 1.5m,
                    CompletionRateMultiplier = 1.3m,
                    FixedAdditionalCost = 0.0m,
                    Description = "超长上下文（8K-16K tokens）"
                },
                new ContextPricingTier
                {
                    MinContextLength = 16385,
                    MaxContextLength = 32768,
                    PromptRateMultiplier = 2.0m,
                    CompletionRateMultiplier = 1.5m,
                    FixedAdditionalCost = 0.0m,
                    Description = "极长上下文（16K-32K tokens）"
                },
                new ContextPricingTier
                {
                    MinContextLength = 32769,
                    MaxContextLength = 131072,
                    PromptRateMultiplier = 3.0m,
                    CompletionRateMultiplier = 2.0m,
                    FixedAdditionalCost = 0.0m,
                    Description = "超大上下文（32K-128K tokens）"
                }
            };
        }
    }

    /// <summary>
    /// 根据模型类型获取推荐的默认上下文长度
    /// </summary>
    /// <param name="model">模型名称</param>
    /// <returns>推荐的默认上下文长度</returns>
    public static int GetRecommendedDefaultContextLength(string model)
    {
        return model.ToLower() switch
        {
            // GPT-4 系列
            string m when m.Contains("gpt-4-1106") => 128000,
            string m when m.Contains("gpt-4-turbo") => 128000,
            string m when m.Contains("gpt-4") => 8192,

            // GPT-3.5 系列
            string m when m.Contains("gpt-3.5-turbo-16k") => 16384,
            string m when m.Contains("gpt-3.5-turbo") => 4096,

            // Claude 系列
            string m when m.Contains("claude-3-5-sonnet") => 200000,
            string m when m.Contains("claude-3-sonnet") => 200000,
            string m when m.Contains("claude-3-opus") => 200000,
            string m when m.Contains("claude-3-haiku") => 200000,

            // 默认值
            _ => 4096
        };
    }
}