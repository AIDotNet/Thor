namespace Thor.Abstractions.Dtos;

public class UsageResponse
{
    /// <summary>
    /// 总支出
    /// </summary>
    /// <returns></returns>
    public long TotalCost { get; set; }
    
    /// <summary>
    /// 总请求数
    /// </summary>
    /// <returns></returns>
    public long TotalRequestCount { get; set; }
    
    /// <summary>
    /// 总Token数
    /// </summary>
    /// <returns></returns>
    public long TotalTokenCount { get; set; }
    
    /// <summary>
    /// 每日消费记录列表
    /// </summary>
    public List<DailyUsageDto> DailyUsage { get; set; } = new List<DailyUsageDto>();
    
    /// <summary>
    /// 请求服务列表
    /// </summary>
    public List<ServiceRequestDto> ServiceRequests { get; set; } = new List<ServiceRequestDto>();
}
