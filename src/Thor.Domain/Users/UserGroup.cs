using Thor.Service.Domain.Core;

namespace Thor.Domain.Users;

/// <summary>
/// 分组
/// </summary>
public sealed class UserGroup : Entity<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 唯一编码
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// 分组倍率
    /// </summary>
    /// <returns></returns>
    public double Rate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    public int Order { get; set; }
}