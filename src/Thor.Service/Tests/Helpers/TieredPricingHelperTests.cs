using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure.Helper;

namespace Thor.Service.Tests.Helpers;

public class TieredPricingHelperTests
{
    [Test]
    public void CalculateTieredPrice_WithNoTiers_ShouldUseFallbackRate()
    {
        // Arrange
        var tiers = new List<ModelPricingTier>();
        var fallbackRate = 1.0m;
        var tokenCount = 1000;
        
        // Act
        var result = TieredPricingHelper.CalculateTieredPrice(tokenCount, tiers, fallbackRate);
        
        // Assert
        Assert.AreEqual(1000m, result);
    }
    
    [Test]
    public void CalculateTieredPrice_WithGeminiStyleTiers_ShouldCalculateCorrectly()
    {
        // Arrange
        var tiers = new List<ModelPricingTier>
        {
            new ModelPricingTier { Threshold = 200000, Rate = 1.25m },
            new ModelPricingTier { Threshold = -1, Rate = 2.50m }
        };
        
        // Act - Test with tokens under threshold
        var result1 = TieredPricingHelper.CalculateTieredPrice(100000, tiers, 0m);
        
        // Act - Test with tokens over threshold
        var result2 = TieredPricingHelper.CalculateTieredPrice(300000, tiers, 0m);
        
        // Assert
        Assert.AreEqual(125000m, result1); // 100000 * 1.25
        Assert.AreEqual(475000m, result2); // 200000 * 1.25 + 100000 * 2.50
    }
    
    [Test]
    public void CalculateTieredPrice_WithMultipleTiers_ShouldCalculateCorrectly()
    {
        // Arrange
        var tiers = new List<ModelPricingTier>
        {
            new ModelPricingTier { Threshold = 10000, Rate = 1.0m },
            new ModelPricingTier { Threshold = 50000, Rate = 1.5m },
            new ModelPricingTier { Threshold = -1, Rate = 2.0m }
        };
        
        // Act
        var result = TieredPricingHelper.CalculateTieredPrice(70000, tiers, 0m);
        
        // Assert
        // 10000 * 1.0 + 40000 * 1.5 + 20000 * 2.0
        Assert.AreEqual(110000m, result);
    }
    
    [Test]
    public void GetApplicableRate_WithGeminiStyleTiers_ShouldReturnCorrectRate()
    {
        // Arrange
        var tiers = new List<ModelPricingTier>
        {
            new ModelPricingTier { Threshold = 200000, Rate = 1.25m },
            new ModelPricingTier { Threshold = -1, Rate = 2.50m }
        };
        
        // Act
        var rate1 = TieredPricingHelper.GetApplicableRate(100000, tiers, 0m);
        var rate2 = TieredPricingHelper.GetApplicableRate(300000, tiers, 0m);
        
        // Assert
        Assert.AreEqual(1.25m, rate1);
        Assert.AreEqual(2.50m, rate2);
    }
    
    [Test]
    public void CreateGeminiTieredPricing_ShouldCreateCorrectConfiguration()
    {
        // Act
        var geminiPricing = TieredPricingHelper.CreateGeminiTieredPricing();
        
        // Assert
        Assert.IsTrue(geminiPricing.Enabled);
        Assert.AreEqual(2, geminiPricing.PromptTiers.Count);
        Assert.AreEqual(2, geminiPricing.CompletionTiers.Count);
        Assert.AreEqual(2, geminiPricing.CacheTiers.Count);
        
        // Check prompt tiers
        Assert.AreEqual(200000, geminiPricing.PromptTiers[0].Threshold);
        Assert.AreEqual(1.25m, geminiPricing.PromptTiers[0].Rate);
        Assert.AreEqual(-1, geminiPricing.PromptTiers[1].Threshold);
        Assert.AreEqual(2.50m, geminiPricing.PromptTiers[1].Rate);
        
        // Check completion tiers
        Assert.AreEqual(200000, geminiPricing.CompletionTiers[0].Threshold);
        Assert.AreEqual(10.00m, geminiPricing.CompletionTiers[0].Rate);
        Assert.AreEqual(-1, geminiPricing.CompletionTiers[1].Threshold);
        Assert.AreEqual(15.00m, geminiPricing.CompletionTiers[1].Rate);
        
        // Check cache tiers
        Assert.AreEqual(200000, geminiPricing.CacheTiers[0].Threshold);
        Assert.AreEqual(0.31m, geminiPricing.CacheTiers[0].Rate);
        Assert.AreEqual(-1, geminiPricing.CacheTiers[1].Threshold);
        Assert.AreEqual(0.625m, geminiPricing.CacheTiers[1].Rate);
    }
}