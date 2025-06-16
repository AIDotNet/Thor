using Microsoft.EntityFrameworkCore;
using Thor.Infrastructure;
using Thor.Service.Domain;

namespace Thor.Service.Service;

/// <summary>
/// 公告服务
/// </summary>
/// <param name="serviceProvider"></param>
public sealed class AnnouncementService(IServiceProvider serviceProvider) : ApplicationService(serviceProvider)
{
    /// <summary>
    /// 创建公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<string> CreateAsync(CreateAnnouncementInput input)
    {
        var announcement = new Announcement
        {
            Id = Guid.NewGuid().ToString(),
            Title = input.Title,
            Content = input.Content,
            Type = input.Type ?? "info",
            Enabled = input.Enabled,
            Pinned = input.Pinned,
            Order = input.Order,
            ExpireTime = input.ExpireTime,
            CreatedBy = UserContext.CurrentUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await DbContext.Announcements.AddAsync(announcement);
        await DbContext.SaveChangesAsync();

        return announcement.Id;
    }

    /// <summary>
    /// 获取公告列表（管理员）
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public async Task<object> GetListAsync(int page = 1, int pageSize = 10, string? keyword = null)
    {
        var query = DbContext.Announcements.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Title.Contains(keyword) || x.Content.Contains(keyword));
        }

        var total = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(x => x.Pinned)
            .ThenByDescending(x => x.Order)
            .ThenByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            items,
            total,
            page,
            pageSize
        };
    }

    /// <summary>
    /// 获取有效公告列表（用户端）
    /// </summary>
    /// <returns></returns>
    public async Task<List<Announcement>> GetActiveAnnouncementsAsync()
    {
        var now = DateTime.UtcNow;
        
        return await DbContext.Announcements
            .Where(x => x.Enabled && (x.ExpireTime == null || x.ExpireTime > now))
            .OrderByDescending(x => x.Pinned)
            .ThenByDescending(x => x.Order)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 获取公告详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Announcement?> GetAsync(string id)
    {
        return await DbContext.Announcements.FindAsync(id);
    }

    /// <summary>
    /// 更新公告
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(string id, UpdateAnnouncementInput input)
    {
        var announcement = await DbContext.Announcements.FindAsync(id);
        if (announcement == null)
            throw new Exception("公告不存在");

        announcement.Title = input.Title;
        announcement.Content = input.Content;
        announcement.Type = input.Type ?? "info";
        announcement.Enabled = input.Enabled;
        announcement.Pinned = input.Pinned;
        announcement.Order = input.Order;
        announcement.ExpireTime = input.ExpireTime;
        announcement.UpdatedAt = DateTime.UtcNow;

        DbContext.Announcements.Update(announcement);
        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 删除公告
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(string id)
    {
        var announcement = await DbContext.Announcements.FindAsync(id);
        if (announcement == null)
            throw new Exception("公告不存在");

        DbContext.Announcements.Remove(announcement);
        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 启用/禁用公告
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    /// <returns></returns>
    public async Task ToggleEnabledAsync(string id, bool enabled)
    {
        var announcement = await DbContext.Announcements.FindAsync(id);
        if (announcement == null)
            throw new Exception("公告不存在");

        announcement.Enabled = enabled;
        announcement.UpdatedAt = DateTime.UtcNow;

        DbContext.Announcements.Update(announcement);
        await DbContext.SaveChangesAsync();
    }
}

/// <summary>
/// 创建公告输入
/// </summary>
public class CreateAnnouncementInput
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
    /// 公告类型
    /// </summary>
    public string? Type { get; set; } = "info";

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
    /// 过期时间
    /// </summary>
    public DateTime? ExpireTime { get; set; }
}

/// <summary>
/// 更新公告输入
/// </summary>
public class UpdateAnnouncementInput
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
    /// 公告类型
    /// </summary>
    public string? Type { get; set; } = "info";

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
    /// 过期时间
    /// </summary>
    public DateTime? ExpireTime { get; set; }
} 