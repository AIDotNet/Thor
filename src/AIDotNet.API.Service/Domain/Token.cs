using AIDotNet.API.Service.Domina.Core;

namespace AIDotNet.API.Service.Domina;

public sealed class Token : Entity<long>, ISoftDeletion
{
    /// <summary>
    /// Api Key
    /// </summary>
    public string Key { get; set; } = null!;
    
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
    /// 最近访问时间
    /// </summary>
    public DateTime? AccessedTime { get; set; }
    
    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime? ExpiredTime { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public uint Status { get; set; }
    
    public bool IsDelete { get; set; }
    
    public DateTime? DeletedAt { get; set; }

}