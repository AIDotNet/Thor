using AIDotNet.API.Service.Domain.Core;
using AIDotNet.API.Service.Domina.Core;

namespace AIDotNet.API.Service.Domain;

public sealed class ChatChannel : Entity<string>
{
    /// <summary>
    /// 优先级
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// AI类型
    /// </summary>
    public string Type { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// 根地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 响应时间 (ms)
    /// </summary>
    public long? ResponseTime { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 模型
    /// </summary>
    public List<string> Models { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Other { get; set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool Disable { get; set; }

    /// <summary>
    /// 扩展字段
    /// </summary>
    public Dictionary<string, string> Extension { get; set; } = new();

    /// <summary>
    /// 消耗token
    /// </summary>
    public int Quota { get; set; }

    /// <summary>
    /// 额度
    /// </summary>
    public long RemainQuota { get; set; }

    /// <summary>
    /// 自动监控渠道
    /// </summary>
    public bool ControlAutomatically { get; set; }
}