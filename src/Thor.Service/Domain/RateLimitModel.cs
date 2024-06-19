using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

public class RateLimitModel : Entity<string>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 白名单
    /// </summary>
    public List<string> WhiteList { get; set; } = new List<string>();

    /// <summary>
    /// 黑名单
    /// </summary>
    public List<string> BlackList { get; set; }= new List<string>();

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 限流模型
    /// </summary>
    public string[] Model { get; set; } = [];

    /// <summary>
    /// 限流策略
    /// s|秒 m|分钟 h|小时 d|天
    /// </summary>
    public string Strategy { get; set; }

    /// <summary>
    /// 限流策略数量
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// 限流数量
    /// </summary>
    public int Value { get; set; }
}