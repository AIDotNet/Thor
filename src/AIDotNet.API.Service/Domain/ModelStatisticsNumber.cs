using AIDotNet.API.Service.Domain.Core;

namespace AIDotNet.API.Service.Domain;

public class ModelStatisticsNumber : Entity<long>
{
    public ushort Year { get; set; }

    public ushort Month { get; set; }

    public ushort Day { get; set; }

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