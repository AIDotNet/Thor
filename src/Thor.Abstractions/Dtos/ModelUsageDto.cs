namespace Thor.Abstractions.Dtos;

/// <summary>
/// 模型使用记录
/// </summary>
public class ModelUsageDto
{
    /// <summary>
    /// 模型名称
    /// </summary>
    public string ModelName { get; set; }
    
    /// <summary>
    /// 消费额度
    /// </summary>
    public long Cost { get; set; }
    
    /// <summary>
    /// 请求数量
    /// </summary>
    public long RequestCount { get; set; }
    
    /// <summary>
    /// 消耗Token数量
    /// </summary>
    public long TokenCount { get; set; }
}
