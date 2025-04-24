using Thor.Service.Domain.Core;

namespace Thor.Domain.Chats;

public sealed class Token : Entity<string>, ISoftDeletion
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
    /// 不过期
    /// </summary>
    public bool UnlimitedExpired { get; set; }
    
    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool Disabled { get; set; }

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
    
    public bool IsDelete { get; set; }
    
    public DateTime? DeletedAt { get; set; }

}