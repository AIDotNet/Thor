namespace Thor.Service.Dto;

public sealed class TokenInput
{
    /// <summary>
    /// Token 名称
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// 无限额度
    /// </summary>
    public bool UnlimitedQuota { get; set; }
    
    /// <summary>
    /// 额度
    /// </summary>
    public long RemainQuota { get; set; }
    
    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime? ExpiredTime { get; set; }

    /// <summary>
    /// 不过期
    /// </summary>
    public bool UnlimitedExpired { get; set; }

    /// <summary>
    /// 限制使用的模型
    /// </summary>
    public List<string> LimitModels { get; set; } = new();
    
    /// <summary>
    /// IP白名单
    /// </summary>
    public List<string> WhiteIpList { get; set; } = new();
    
    /// <summary>
    /// token所属分组
    /// </summary>
    public string[] Groups { get; set; } = null!;
}