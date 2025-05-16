namespace Thor.Abstractions.Dtos;

/// <summary>
/// 每日消费记录
/// </summary>
public class DailyUsageDto
{
    /// <summary>
    /// 消费日期 (精确到天)
    /// </summary>
    public DateTime Date { get; set; }
    
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
    
    /// <summary>
    /// 不同模型的消费记录
    /// </summary>
    public List<ModelUsageDto> ModelUsage { get; set; } = new List<ModelUsageDto>();
}
