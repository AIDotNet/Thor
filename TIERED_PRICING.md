# Gemini 分层定价支持

本文档说明如何在 Thor AI 网关中配置和使用 Gemini 风格的分层定价功能。

## 概述

分层定价允许根据 token 使用量的不同区间设置不同的费率，这样可以更精确地控制成本，特别是对于像 Gemini 这样按 token 数量分级计费的模型。

## 功能特性

- 支持基于 token 数量阈值的分层费率
- 分别支持提示词、完成词、缓存、音频等不同类型的分层定价
- 向后兼容现有的固定费率系统
- 提供 Gemini 风格的预设模板
- 支持无限制层级（threshold = -1）

## 数据模型

### ModelPricingTier
```json
{
  "threshold": 200000,    // token 阈值，-1 表示无限制
  "rate": 1.25,          // 该层级的费率
  "description": "≤200K tokens"  // 层级描述
}
```

### ModelTieredPricing
```json
{
  "enabled": true,
  "promptTiers": [        // 提示词分层
    {
      "threshold": 200000,
      "rate": 1.25,
      "description": "≤200K tokens"
    },
    {
      "threshold": -1,
      "rate": 2.50,
      "description": ">200K tokens"
    }
  ],
  "completionTiers": [    // 完成词分层
    {
      "threshold": 200000,
      "rate": 10.00,
      "description": "≤200K tokens"
    },
    {
      "threshold": -1,
      "rate": 15.00,
      "description": ">200K tokens"
    }
  ],
  "cacheTiers": [         // 缓存分层
    {
      "threshold": 200000,
      "rate": 0.31,
      "description": "≤200K tokens"
    },
    {
      "threshold": -1,
      "rate": 0.625,
      "description": ">200K tokens"
    }
  ]
}
```

## API 使用

### 获取 Gemini 分层定价模板
```http
GET /api/v1/model-manager/tiered-pricing/gemini-template
```

响应示例：
```json
{
  "enabled": true,
  "promptTiers": [...],
  "completionTiers": [...],
  "cacheTiers": [...]
}
```

### 创建支持分层定价的模型
```http
POST /api/v1/model-manager
Content-Type: application/json

{
  "model": "gemini-2.5-pro",
  "description": "Gemini 2.5 Pro with tiered pricing",
  "promptRate": 1.25,
  "completionRate": 10.00,
  "tieredPricing": {
    "enabled": true,
    "promptTiers": [
      { "threshold": 200000, "rate": 1.25, "description": "≤200K tokens" },
      { "threshold": -1, "rate": 2.50, "description": ">200K tokens" }
    ],
    "completionTiers": [
      { "threshold": 200000, "rate": 10.00, "description": "≤200K tokens" },
      { "threshold": -1, "rate": 15.00, "description": ">200K tokens" }
    ]
  }
}
```

## 定价计算示例

### 场景 1：小量使用（100K tokens）
- 提示词：100,000 tokens × 1.25 = 125,000
- 完成词：50,000 tokens × 10.00 = 500,000
- 总计：625,000

### 场景 2：大量使用（300K tokens）
- 提示词：
  - 前 200K：200,000 × 1.25 = 250,000
  - 后 100K：100,000 × 2.50 = 250,000
  - 小计：500,000
- 完成词：250,000 tokens
  - 前 200K：200,000 × 10.00 = 2,000,000
  - 后 50K：50,000 × 15.00 = 750,000
  - 小计：2,750,000
- 总计：3,250,000

## 配置说明

1. **启用分层定价**：设置 `tieredPricing.enabled = true`
2. **配置层级**：每个层级指定阈值和费率
3. **无限制层级**：使用 `threshold = -1` 表示该层级适用于所有超过前面阈值的 token
4. **向后兼容**：如果未配置分层定价或 `enabled = false`，系统会使用原有的固定费率

## 实现细节

### 计算逻辑
分层定价按以下逻辑计算：
1. 将层级按阈值升序排序
2. 依次计算每个层级的 token 数量和费用
3. 累加得到总费用

### 代码示例
```csharp
// 使用扩展方法计算费用
var promptCost = modelManager.CalculatePromptCost(tokenCount);
var completionCost = modelManager.CalculateCompletionCost(responseTokens);

// 获取适用费率
var rate = modelManager.GetPromptRate(tokenCount);
```

## 注意事项

1. **性能**：分层定价计算比固定费率略复杂，但对性能影响微乎其微
2. **精度**：费用计算使用 decimal 类型确保精度
3. **兼容性**：现有的固定费率配置继续有效
4. **日志**：系统会在日志中记录是否使用了分层定价

## 故障排除

### 分层定价未生效
- 检查 `tieredPricing.enabled` 是否为 `true`
- 确认层级配置正确，至少有一个层级
- 验证数据库中的配置已正确保存

### 费用计算异常
- 检查层级的阈值设置是否合理
- 确认费率为正数
- 验证 JSON 序列化/反序列化正常