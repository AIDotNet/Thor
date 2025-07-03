using Thor.Service.Domain;
using Thor.Service.Domain.Core;
using Thor.Service.Extensions;
using Thor.Service.Infrastructure.Helper;

namespace Thor.Service.Tests.Integration;

/// <summary>
/// 集成测试：验证ModelManager与分层定价的完整功能
/// </summary>
public class TieredPricingIntegrationTests
{
    public static void RunIntegrationTest()
    {
        Console.WriteLine("开始集成测试：ModelManager分层定价功能");
        
        // 创建一个测试用的ModelManager实例
        var modelManager = new ModelManager
        {
            Model = "test-model",
            PromptRate = 1.0m, // 默认费率作为回退
            CompletionRate = 2.0m,
            TieredPricing = TieredPricingHelper.CreateTieredPricingTemplate()
        };
        
        Console.WriteLine($"测试模型: {modelManager.Model}");
        Console.WriteLine($"分层定价启用: {modelManager.TieredPricing?.Enabled}");
        
        // 测试1: 小量token使用（100K）
        var promptCost1 = modelManager.CalculatePromptCost(100000);
        var completionCost1 = modelManager.CalculateCompletionCost(50000);
        Console.WriteLine($"测试1 - 100K提示词费用: {promptCost1} (期望: 125000)");
        Console.WriteLine($"测试1 - 50K完成词费用: {completionCost1} (期望: 500000)");
        
        // 测试2: 大量token使用（300K）
        var promptCost2 = modelManager.CalculatePromptCost(300000);
        var completionCost2 = modelManager.CalculateCompletionCost(250000);
        Console.WriteLine($"测试2 - 300K提示词费用: {promptCost2} (期望: 500000)");
        Console.WriteLine($"测试2 - 250K完成词费用: {completionCost2} (期望: 2750000)");
        
        // 测试3: 禁用分层定价，应该使用固定费率
        modelManager.TieredPricing.Enabled = false;
        var promptCost3 = modelManager.CalculatePromptCost(100000);
        var completionCost3 = modelManager.CalculateCompletionCost(50000);
        Console.WriteLine($"测试3 - 禁用分层定价，100K提示词费用: {promptCost3} (期望: 100000)");
        Console.WriteLine($"测试3 - 禁用分层定价，50K完成词费用: {completionCost3} (期望: 100000)");
        
        // 测试4: 测试缓存费用
        modelManager.TieredPricing.Enabled = true;
        var cacheCost1 = modelManager.CalculateCacheCost(100000);
        var cacheCost2 = modelManager.CalculateCacheCost(300000);
        Console.WriteLine($"测试4 - 100K缓存费用: {cacheCost1} (期望: 31000)");
        Console.WriteLine($"测试4 - 300K缓存费用: {cacheCost2} (期望: 124500)");
        
        // 测试5: 验证JSON序列化/反序列化
        try
        {
            var jsonString = System.Text.Json.JsonSerializer.Serialize(modelManager.TieredPricing);
            var deserializedPricing = System.Text.Json.JsonSerializer.Deserialize<ModelTieredPricing>(jsonString);
            Console.WriteLine($"测试5 - JSON序列化成功，启用状态: {deserializedPricing?.Enabled}");
            Console.WriteLine($"测试5 - JSON序列化成功，提示词层级数: {deserializedPricing?.PromptTiers.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"测试5 - JSON序列化失败: {ex.Message}");
        }
        
        Console.WriteLine("集成测试完成！");
    }
}