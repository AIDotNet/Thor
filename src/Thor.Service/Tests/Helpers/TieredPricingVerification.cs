using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure.Helper;

namespace Thor.Service.Tests.Helpers;

/// <summary>
/// 简单的测试验证脚本
/// </summary>
public class TieredPricingVerification
{
    public static void VerifyTieredPricing()
    {
        Console.WriteLine("开始验证分层定价逻辑...");
        
        // 测试1：无分层配置时使用回退费率
        var tiers = new List<ModelPricingTier>();
        var result1 = TieredPricingHelper.CalculateTieredPrice(1000, tiers, 1.0m);
        Console.WriteLine($"测试1 - 无分层配置: {result1} (期望: 1000)");
        
        // 测试2：Gemini风格分层定价
        var geminiTiers = new List<ModelPricingTier>
        {
            new ModelPricingTier { Threshold = 200000, Rate = 1.25m },
            new ModelPricingTier { Threshold = -1, Rate = 2.50m }
        };
        
        var result2 = TieredPricingHelper.CalculateTieredPrice(100000, geminiTiers, 0m);
        Console.WriteLine($"测试2 - Gemini风格(100K tokens): {result2} (期望: 125000)");
        
        var result3 = TieredPricingHelper.CalculateTieredPrice(300000, geminiTiers, 0m);
        Console.WriteLine($"测试3 - Gemini风格(300K tokens): {result3} (期望: 475000)");
        
        // 测试3：多层级定价
        var multiTiers = new List<ModelPricingTier>
        {
            new ModelPricingTier { Threshold = 10000, Rate = 1.0m },
            new ModelPricingTier { Threshold = 50000, Rate = 1.5m },
            new ModelPricingTier { Threshold = -1, Rate = 2.0m }
        };
        
        var result4 = TieredPricingHelper.CalculateTieredPrice(70000, multiTiers, 0m);
        Console.WriteLine($"测试4 - 多层级(70K tokens): {result4} (期望: 110000)");
        
        // 测试4：获取适用费率
        var rate1 = TieredPricingHelper.GetApplicableRate(100000, geminiTiers, 0m);
        var rate2 = TieredPricingHelper.GetApplicableRate(300000, geminiTiers, 0m);
        Console.WriteLine($"测试5 - 获取费率(100K): {rate1} (期望: 1.25)");
        Console.WriteLine($"测试6 - 获取费率(300K): {rate2} (期望: 2.50)");
        
        // 测试5：创建Gemini配置
        var geminiConfig = TieredPricingHelper.CreateGeminiTieredPricing();
        Console.WriteLine($"测试7 - Gemini配置启用: {geminiConfig.Enabled} (期望: True)");
        Console.WriteLine($"测试8 - Gemini提示词层级数: {geminiConfig.PromptTiers.Count} (期望: 2)");
        Console.WriteLine($"测试9 - Gemini完成词层级数: {geminiConfig.CompletionTiers.Count} (期望: 2)");
        Console.WriteLine($"测试10 - Gemini缓存层级数: {geminiConfig.CacheTiers.Count} (期望: 2)");
        
        Console.WriteLine("验证完成！");
    }
}