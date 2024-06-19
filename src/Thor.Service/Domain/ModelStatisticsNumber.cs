using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

public class ModelStatisticsNumber : Entity<string>
{
    public int Year { get; set; }

    public int Month { get; set; }

    public int Day { get; set; }

    /// <summary>
    /// 模型名称
    /// </summary>
    public string ModelName { get; set; }

    /// <summary>
    /// 消耗额度
    /// </summary>
    public int Quota { get; set; }

    /// <summary>
    /// token使用量
    /// </summary>
    public int TokenUsed { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Count { get; set; }
}