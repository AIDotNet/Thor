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

    /// <summary>
    /// 模型费用排名
    /// </summary>
    /// <returns></returns>
    public List<ModelRankingDto> ModelRanking { get; set; }

    /// <summary>
    /// 用户新增数据
    /// </summary>
    /// <returns></returns>
    public List<StatisticsNumberDto> UserNewData { get; set; } = null;
    
    /// <summary>
    /// 充值最近数据
    /// </summary>
    public List<StatisticsNumberDto> RechargeData { get; set; } = null;
}



public class ModelRankingDto
{
    public string Name { get; set; }

    public long Value { get; set; }

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color => GetModelColor(Name);

    public string Icon => GetModelIcon(Name);

    public string GetModelIcon(string modelName)
    {
        if (modelName.StartsWith("gpt", StringComparison.OrdinalIgnoreCase))
        {
            return "OpenAI";
        }
        else if (modelName.StartsWith("claude", StringComparison.OrdinalIgnoreCase))
        {
            return "Claude";
        }
        else if (modelName.StartsWith("gemini", StringComparison.OrdinalIgnoreCase))
        {
            return "Google";
        }
        else if (modelName.StartsWith("ERNIE", StringComparison.OrdinalIgnoreCase))
        {
            return "ERNIE";
        }
        else if (modelName.StartsWith("glm", StringComparison.OrdinalIgnoreCase))
        {
            return "ChatGLM";
        }
        else if (modelName.StartsWith("qwen", StringComparison.OrdinalIgnoreCase))
        {
            return "Qwen";
        }
        else if (modelName.StartsWith("deepseek", StringComparison.OrdinalIgnoreCase))
        {
            return "DeepSeek";
        }

        return "gpt";
    }

    public string GetModelColor(string modelName)
    {
        if (modelName.StartsWith("gpt", StringComparison.OrdinalIgnoreCase))
        {
            return "#FFA500";
        }
        else if (modelName.StartsWith("claude", StringComparison.OrdinalIgnoreCase))
        {
            return "#FF0000";
        }
        else if (modelName.StartsWith("gemini", StringComparison.OrdinalIgnoreCase))
        {
            return "#00FF00";
        }
        else if (modelName.StartsWith("ERNIE", StringComparison.OrdinalIgnoreCase))
        {
            return "#0000FF";
        }
        else if (modelName.StartsWith("glm", StringComparison.OrdinalIgnoreCase))
        {
            return "#FF00FF";
        }
        else if (modelName.StartsWith("qwen", StringComparison.OrdinalIgnoreCase))
        {
            return "#00FFFF";
        }
        else if (modelName.StartsWith("deepseek", StringComparison.OrdinalIgnoreCase))
        {
            return "#FFFF00";
        }
        else if (modelName.StartsWith("doubao", StringComparison.OrdinalIgnoreCase))
        {
            return "#A52A2A";
        }

        // 随机颜色
        return "#" + new Random().Next(0, 0xFFFFFF).ToString("X6");
    }
}

public class StatisticsNumberDto
{
    public long Value { get; set; }

    public string DateTime { get; set; }

    public string Name { get; set; }
}

public class ModelStatisticsDto
{
    public string CreatedAt { get; set; }

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