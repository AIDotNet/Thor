using Thor.Service.Domain;
using Thor.Service.Domain.Core;
using Thor.Service.Extensions;
using Xunit;

namespace Thor.Service.Tests.Helpers;

/// <summary>
/// 上下文定价测试
/// </summary>
public class ContextPricingTests
{
    [Fact]
    public void CalculatePromptCost_WithoutContextPricing_ShouldReturnBaseCost()
    {
        // Arrange
        var model = new ModelManager
        {
            PromptRate = 1.0m,
            ContextPricing = null
        };

        // Act
        var cost = model.CalculatePromptCost(1000, 4000);

        // Assert
        Assert.Equal(1000m, cost);
    }

    [Fact]
    public void CalculatePromptCost_WithContextPricingDisabled_ShouldReturnBaseCost()
    {
        // Arrange
        var model = new ModelManager
        {
            PromptRate = 1.0m,
            ContextPricing = new ModelContextPricing { Enabled = false }
        };

        // Act
        var cost = model.CalculatePromptCost(1000, 4000);

        // Assert
        Assert.Equal(1000m, cost);
    }

    [Fact]
    public void CalculatePromptCost_WithContextPricingMultiplier_ShouldApplyMultiplier()
    {
        // Arrange
        var model = new ModelManager
        {
            PromptRate = 1.0m,
            ContextPricing = new ModelContextPricing
            {
                Enabled = true,
                PricingType = ContextPricingType.Multiplier,
                ContextTiers = new List<ModelPricingTier>
                {
                    new() { Threshold = 2000, Rate = 1.0m },
                    new() { Threshold = 8000, Rate = 1.5m },
                    new() { Threshold = -1, Rate = 2.0m }
                }
            }
        };

        // Act - 4000 token context should use 1.5x multiplier
        var cost = model.CalculatePromptCost(1000, 4000);

        // Assert
        Assert.Equal(1500m, cost); // 1000 * 1.0 * 1.5
    }

    [Fact]
    public void CalculatePromptCost_WithContextPricingReplacement_ShouldReplaceRate()
    {
        // Arrange
        var model = new ModelManager
        {
            PromptRate = 1.0m,
            ContextPricing = new ModelContextPricing
            {
                Enabled = true,
                PricingType = ContextPricingType.Replacement,
                ContextTiers = new List<ModelPricingTier>
                {
                    new() { Threshold = 2000, Rate = 0.8m },
                    new() { Threshold = 8000, Rate = 1.2m },
                    new() { Threshold = -1, Rate = 1.8m }
                }
            }
        };

        // Act - 4000 token context should use 1.2 rate
        var cost = model.CalculatePromptCost(1000, 4000);

        // Assert
        Assert.Equal(1200m, cost); // 1000 * 1.2
    }

    [Fact]
    public void CalculatePromptCost_WithTieredAndContextPricing_ShouldApplyBoth()
    {
        // Arrange
        var model = new ModelManager
        {
            PromptRate = 1.0m,
            TieredPricing = new ModelTieredPricing
            {
                Enabled = true,
                PromptTiers = new List<ModelPricingTier>
                {
                    new() { Threshold = 500, Rate = 1.0m },
                    new() { Threshold = -1, Rate = 1.25m }
                }
            },
            ContextPricing = new ModelContextPricing
            {
                Enabled = true,
                PricingType = ContextPricingType.Multiplier,
                ContextTiers = new List<ModelPricingTier>
                {
                    new() { Threshold = 2000, Rate = 1.0m },
                    new() { Threshold = -1, Rate = 1.5m }
                }
            }
        };

        // Act - 1000 tokens with 4000 context
        // Tiered: 500*1.0 + 500*1.25 = 1125
        // Context: 1125 * 1.5 = 1687.5
        var cost = model.CalculatePromptCost(1000, 4000);

        // Assert
        Assert.Equal(1687.5m, cost);
    }

    [Fact]
    public void GetPromptRate_WithContextPricing_ShouldReturnAdjustedRate()
    {
        // Arrange
        var model = new ModelManager
        {
            PromptRate = 1.0m,
            ContextPricing = new ModelContextPricing
            {
                Enabled = true,
                PricingType = ContextPricingType.Multiplier,
                ContextTiers = new List<ModelPricingTier>
                {
                    new() { Threshold = 2000, Rate = 1.0m },
                    new() { Threshold = -1, Rate = 2.0m }
                }
            }
        };

        // Act
        var rate = model.GetPromptRate(100, 4000);

        // Assert
        Assert.Equal(2.0m, rate); // 1.0 * 2.0
    }

    [Fact]
    public void CalculateCompletionCost_WithContextPricing_ShouldWork()
    {
        // Arrange
        var model = new ModelManager
        {
            PromptRate = 1.0m,
            CompletionRate = 1.5m,
            ContextPricing = new ModelContextPricing
            {
                Enabled = true,
                PricingType = ContextPricingType.Multiplier,
                ContextTiers = new List<ModelPricingTier>
                {
                    new() { Threshold = 2000, Rate = 1.0m },
                    new() { Threshold = -1, Rate = 1.2m }
                }
            }
        };

        // Act
        var cost = model.CalculateCompletionCost(500, 4000);

        // Assert
        Assert.Equal(900m, cost); // 500 * 1.5 * 1.2
    }

    [Fact]
    public void BackwardCompatibility_OldMethodsShouldWork()
    {
        // Arrange
        var model = new ModelManager
        {
            PromptRate = 1.0m,
            CompletionRate = 1.5m
        };

        // Act
        var promptCost = model.CalculatePromptCost(1000);
        var completionCost = model.CalculateCompletionCost(500);
        var promptRate = model.GetPromptRate(100);
        var completionRate = model.GetCompletionRate(200);

        // Assert
        Assert.Equal(1000m, promptCost);
        Assert.Equal(750m, completionCost);
        Assert.Equal(1.0m, promptRate);
        Assert.Equal(1.5m, completionRate);
    }
}