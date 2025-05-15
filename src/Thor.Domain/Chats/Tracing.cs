using Thor.Service.Domain.Core;

namespace Thor.Domain.Chats;

public class Tracing : Entity<string>
{
    /// <summary>
    /// 链路ID，用于唯一标识一个完整的请求链路
    /// </summary>
    public string TraceId { get; set; } = string.Empty;

    /// <summary>
    /// 日志Id，关联到ChatLogger
    /// </summary>
    public string ChatLoggerId { get; set; } = string.Empty;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 节点类型
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 执行时长(毫秒)
    /// </summary>
    public long Duration { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 节点深度，从0开始
    /// </summary>
    public int Depth { get; set; } = 0;

    /// <summary>
    /// 服务名称
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// 扩展属性
    /// </summary>
    public Dictionary<string, string>? Attributes { get; set; } = new();

    /// <summary>
    /// 子节点集合，不映射到数据库
    /// </summary>
    public List<Tracing> Children { get; set; } = new();
}