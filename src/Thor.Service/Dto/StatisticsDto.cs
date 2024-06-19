namespace Thor.Service.Dto;

public class StatisticsDto
{
    public List<StatisticsNumberDto> Consumes { get; set; }  
    
    public List<StatisticsNumberDto> Requests { get; set; }
    
    public List<StatisticsNumberDto> Tokens { get; set; }
    
    public List<ModelStatisticsDto> Models { get; set; }

    /// <summary>
    /// 模式日期
    /// </summary>
    public List<string> ModelDate { get; set; }

    /// <summary>
    /// 当前剩余额度
    /// </summary>
    public long CurrentResidualCredit { get; set; }
    
    /// <summary>
    /// 当前消费额度
    /// </summary>
    public long CurrentConsumedCredit { get; set; }
    
    /// <summary>
    /// 总请求数量
    /// </summary>
    public long TotalRequestCount { get; set; }
    
    /// <summary>
    /// 总Token数量
    /// </summary>
    public long TotalTokenCount { get; set; }
}

public class StatisticsNumberDto
{
    public long Value { get; set; }

    public string DateTime { get; set; }
}

public class ModelStatisticsDto
{
    public DateTime CreatedAt { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// 消耗额度
    /// </summary>
    public List<int> Data { get; set; }

    /// <summary>
    /// token使用量
    /// </summary>
    public int TokenUsed { get; set; }
}