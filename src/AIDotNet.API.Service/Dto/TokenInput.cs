namespace AIDotNet.API.Service.Dto;

public sealed class TokenInput
{
    /// <summary>
    /// Token 名称
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// 使用额度
    /// </summary>
    public long UsedQuota { get; set; }
    
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

}