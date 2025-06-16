using Microsoft.EntityFrameworkCore;
using Thor.Core.DataAccess;
using Thor.Service.Domain;

namespace Thor.Service.Data;

/// <summary>
/// 种子数据初始化
/// </summary>
public static class SeedData
{
    /// <summary>
    /// 初始化公告数据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task SeedAnnouncementsAsync(IThorContext context)
    {
        // 检查是否已有公告数据
        if (await context.Announcements.AnyAsync())
        {
            return; // 已有数据，不需要种子数据
        }

        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = Guid.NewGuid().ToString(),
                Title = "🎉 欢迎使用 Thor AI 平台！",
                Content = @"# 欢迎来到 Thor AI 平台！

感谢您选择 Thor AI 平台。我们为您提供了强大的 AI 服务能力。

## 🚀 主要功能

- **多模型支持**：支持 OpenAI、Claude、Gemini 等多种 AI 模型
- **实时对话**：流式对话体验，响应迅速
- **文件处理**：支持图片、文档等多种文件类型
- **用户管理**：完善的用户权限管理系统

## 📝 使用指南

1. 在左侧导航栏选择相应功能
2. 配置您的 AI 模型参数
3. 开始享受 AI 助手服务

如有任何问题，请联系管理员。祝您使用愉快！ 🎈",
                Type = "info",
                Enabled = true,
                Pinned = true,
                Order = 100,
                ExpireTime = DateTime.UtcNow.AddDays(30),
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Announcement
            {
                Id = Guid.NewGuid().ToString(),
                Title = "🔧 系统维护通知",
                Content = @"# 系统维护通知

## 📅 维护时间
**2024年12月31日 02:00 - 04:00 (UTC+8)**

## 🛠 维护内容
- 数据库性能优化
- 新功能部署
- 安全补丁更新

## ⚠️ 影响范围
维护期间可能出现短暂的服务中断，请提前保存您的工作内容。

## 📞 技术支持
如有紧急问题，请联系：
- 邮箱：support@thor.ai
- 电话：400-123-4567

感谢您的理解与配合！",
                Type = "warning",
                Enabled = true,
                Pinned = false,
                Order = 90,
                ExpireTime = DateTime.UtcNow.AddDays(7),
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow.AddHours(-2),
                UpdatedAt = DateTime.UtcNow.AddHours(-2)
            },
            new Announcement
            {
                Id = Guid.NewGuid().ToString(),
                Title = "✨ 新功能发布",
                Content = @"# 🎊 新功能发布 v2.1.0

我们很高兴地宣布 Thor AI 平台 v2.1.0 版本正式发布！

## 🆕 新增功能

### 1. 智能公告系统
- 支持 **Markdown** 格式
- 美观的弹窗设计
- 智能推送机制

### 2. 多语言支持
- 新增英语界面
- 语言切换更流畅
- 国际化体验

### 3. 性能优化
- 响应速度提升 **50%**
- 内存使用优化 **30%**
- 更稳定的服务

## 🔗 相关链接
- [更新日志](https://thor.ai/changelog)
- [使用文档](https://docs.thor.ai)
- [反馈建议](https://feedback.thor.ai)

---
*更新时间：2024年12月25日*",
                Type = "success",
                Enabled = true,
                Pinned = false,
                Order = 80,
                ExpireTime = null, // 永不过期
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        await context.Announcements.AddRangeAsync(announcements);
        await context.SaveChangesAsync();
    }
} 