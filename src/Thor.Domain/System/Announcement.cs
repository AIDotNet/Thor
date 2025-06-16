using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

/// <summary>
/// 公告实体
/// </summary>
public class Announcement : Entity<string>
{
    /// <summary>
    /// 公告标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 公告内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 公告类型 (info, warning, error, success)
    /// </summary>
    public string Type { get; set; } = "info";

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool Pinned { get; set; } = false;

    /// <summary>
    /// 排序权重
    /// </summary>
    public int Order { get; set; } = 0;

    /// <summary>
    /// 过期时间（可为空，表示永不过期）
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
} 