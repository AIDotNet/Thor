namespace Thor.Service.Domain.Core;

/// <summary>
/// 模型计费类型
/// </summary>
public enum ModelQuotaType
{
    /// <summary>
    /// 按量计费
    /// </summary>
    OnDemand = 1,
    
    /// <summary>
    /// 按次计费
    /// </summary>
    ByCount = 2
}