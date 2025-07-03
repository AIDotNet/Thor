# Thor Advanced Pricing System

This document explains the comprehensive pricing system in Thor, including both token-based tiered pricing and context-based pricing.

## Overview

Thor supports multiple pricing models:
1. **Fixed Pricing** - Traditional fixed rates (backward compatible)
2. **Tiered Pricing** - Variable rates based on token count ranges
3. **Context Pricing** - Variable rates based on conversation context length

## Tiered Pricing

### How It Works
Tiered pricing allows you to set different rates for different token count ranges, similar to Gemini's pricing model.

### Example Configuration
```json
{
  "model": "gemini-2.5-pro",
  "tieredPricing": {
    "enabled": true,
    "promptTiers": [
      {"threshold": 200000, "rate": 1.25},
      {"threshold": -1, "rate": 2.50}
    ],
    "completionTiers": [
      {"threshold": 200000, "rate": 10.00},
      {"threshold": -1, "rate": 15.00}
    ]
  }
}
```

### Calculation Example
For 300K prompt tokens:
- First 200K tokens: 200,000 × 1.25 = 250,000
- Remaining 100K tokens: 100,000 × 2.50 = 250,000
- **Total: 500,000 units**

## Context Pricing

### How It Works
Context pricing applies different rates based on the conversation context length, allowing for cost optimization based on context usage.

### Pricing Modes

#### 1. Multiplier Mode
Applies a multiplier to the base rate:
```json
{
  "contextPricing": {
    "enabled": true,
    "pricingType": "Multiplier",
    "contextTiers": [
      {"threshold": 4000, "rate": 1.0, "description": "Short context ≤4K tokens"},
      {"threshold": 16000, "rate": 1.2, "description": "Medium context 4K-16K tokens"},
      {"threshold": 32000, "rate": 1.5, "description": "Long context 16K-32K tokens"},
      {"threshold": -1, "rate": 2.0, "description": "Ultra-long context >32K tokens"}
    ]
  }
}
```

#### 2. Replacement Mode
Completely replaces the base rate:
```json
{
  "contextPricing": {
    "enabled": true,
    "pricingType": "Replacement",
    "contextTiers": [
      {"threshold": 4000, "rate": 0.8},
      {"threshold": 16000, "rate": 1.2},
      {"threshold": -1, "rate": 1.8}
    ]
  }
}
```

### Calculation Examples

#### Multiplier Mode
- Base cost: 1000 tokens × 1.0 = 1000 units
- Context: 8000 tokens (falls in 4K-16K range, 1.2x multiplier)
- **Final cost: 1000 × 1.2 = 1200 units**

#### Replacement Mode
- Token count: 1000 tokens
- Context: 8000 tokens (falls in 4K-16K range, 1.2 rate)
- **Final cost: 1000 × 1.2 = 1200 units**

## Combined Pricing

You can use both tiered and context pricing together:

```json
{
  "model": "gpt-4-turbo",
  "tieredPricing": {
    "enabled": true,
    "promptTiers": [
      {"threshold": 500, "rate": 1.0},
      {"threshold": -1, "rate": 1.25}
    ]
  },
  "contextPricing": {
    "enabled": true,
    "pricingType": "Multiplier",
    "contextTiers": [
      {"threshold": 8000, "rate": 1.0},
      {"threshold": -1, "rate": 1.5}
    ]
  }
}
```

### Combined Calculation
For 1000 tokens with 16000 context:
1. **Tiered calculation**: 500×1.0 + 500×1.25 = 1125 units
2. **Context multiplier**: 1125 × 1.5 = **1687.5 units**

## API Endpoints

### Get Context Pricing Template
```http
GET /api/v1/model-manager/context-pricing/template
```
Returns a ready-to-use context pricing configuration template.

### Get Gemini Tiered Pricing Template
```http
GET /api/v1/model-manager/tiered-pricing/gemini-template
```
Returns Gemini-style tiered pricing configuration.

### Create Model with Pricing
```http
POST /api/v1/model-manager
{
  "model": "my-model",
  "promptRate": 1.0,
  "tieredPricing": { /* tiered config */ },
  "contextPricing": { /* context config */ }
}
```

## Backward Compatibility

- All existing models continue to work with fixed pricing
- New methods have overloads that maintain backward compatibility
- Context length parameter is optional (defaults to 0, disabling context pricing)

## Best Practices

1. **Start Simple**: Begin with fixed pricing, then add tiered pricing for high-volume models
2. **Monitor Usage**: Use context pricing for models with variable context requirements
3. **Test Thoroughly**: Validate pricing calculations with your expected usage patterns
4. **Document Settings**: Clearly document your pricing configuration for your team

## Implementation Details

### Extension Methods
```csharp
// New methods with context support
model.CalculatePromptCost(tokenCount, contextLength)
model.CalculateCompletionCost(tokenCount, contextLength)

// Backward compatible methods
model.CalculatePromptCost(tokenCount) // contextLength = 0
model.CalculateCompletionCost(tokenCount) // contextLength = 0
```

### Database Storage
Both tiered and context pricing configurations are stored as JSON in the database, providing flexibility for complex pricing structures.

### Performance
- Minimal overhead compared to fixed pricing
- Calculations are performed only when pricing configurations are enabled
- Efficient tier lookup using sorted lists

## 中文说明

### 上下文定价功能
上下文定价允许根据对话上下文长度动态调整价格，特别适用于：
- 长上下文模型的成本优化
- 不同场景下的差异化定价
- 基于使用复杂度的灵活计费

### 定价模式
1. **倍率模式**：在基础费率上应用倍率
2. **替换模式**：完全替换基础费率

### 配置示例
```json
{
  "contextPricing": {
    "enabled": true,
    "pricingType": "Multiplier",
    "contextTiers": [
      {"threshold": 4000, "rate": 1.0, "description": "短上下文 ≤4K tokens"},
      {"threshold": 16000, "rate": 1.2, "description": "中等上下文 4K-16K tokens"},
      {"threshold": 32000, "rate": 1.5, "description": "长上下文 16K-32K tokens"},
      {"threshold": -1, "rate": 2.0, "description": "超长上下文 >32K tokens"}
    ]
  }
}
```