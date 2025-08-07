using Thor.Core.DataAccess;
using Thor.Domain.Chats;

namespace Thor.Service.Service;

public static class TracingService
{
    /// <summary>
    /// 获取指定日志的Tracing
    /// </summary>
    /// <returns></returns>
    public static async Task<Tracing?> GetTracing(ILoggerDbContext loggerDbContext, string chatloggerId)
    {
        return await loggerDbContext.Tracings.FirstOrDefaultAsync(x => x.ChatLoggerId == chatloggerId);
    }

    /// <summary>
    /// 删除指定时间之前的Tracing记录
    /// </summary>
    /// <param name="loggerDbContext">数据库上下文</param>
    /// <param name="beforeDate">删除此时间之前的记录</param>
    /// <returns>删除的记录数量</returns>
    public static async Task<int> DeleteTracingsBefore(ILoggerDbContext loggerDbContext, DateTime beforeDate)
    {
        var tracings = loggerDbContext.Tracings.Where(x => x.CreatedAt < beforeDate);
        var count = await tracings.CountAsync();
        
        if (count > 0)
        {
            loggerDbContext.Tracings.RemoveRange(tracings);
            await loggerDbContext.SaveChangesAsync();
        }

        return count;
    }

    /// <summary>
    /// 删除指定ChatLoggerId相关的所有Tracing记录
    /// </summary>
    /// <param name="loggerDbContext">数据库上下文</param>
    /// <param name="chatLoggerId">聊天日志ID</param>
    /// <returns>删除的记录数量</returns>
    public static async Task<int> DeleteTracingsByChatLoggerId(ILoggerDbContext loggerDbContext, string chatLoggerId)
    {
        var tracings = loggerDbContext.Tracings.Where(x => x.ChatLoggerId == chatLoggerId);
        var count = await tracings.CountAsync();
        
        if (count > 0)
        {
            loggerDbContext.Tracings.RemoveRange(tracings);
            await loggerDbContext.SaveChangesAsync();
        }

        return count;
    }

    /// <summary>
    /// 删除指定TraceId的所有Tracing记录
    /// </summary>
    /// <param name="loggerDbContext">数据库上下文</param>
    /// <param name="traceId">链路跟踪ID</param>
    /// <returns>删除的记录数量</returns>
    public static async Task<int> DeleteTracingsByTraceId(ILoggerDbContext loggerDbContext, string traceId)
    {
        var tracings = loggerDbContext.Tracings.Where(x => x.TraceId == traceId);
        var count = await tracings.CountAsync();
        
        if (count > 0)
        {
            loggerDbContext.Tracings.RemoveRange(tracings);
            await loggerDbContext.SaveChangesAsync();
        }

        return count;
    }

    /// <summary>
    /// 批量删除Tracing记录（按天数保留）
    /// </summary>
    /// <param name="loggerDbContext">数据库上下文</param>
    /// <param name="retainDays">保留天数，默认30天</param>
    /// <returns>删除的记录数量</returns>
    public static async Task<int> CleanupTracings(ILoggerDbContext loggerDbContext, int retainDays = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-retainDays);
        return await DeleteTracingsBefore(loggerDbContext, cutoffDate);
    }

    /// <summary>
    /// 获取Tracing统计信息
    /// </summary>
    /// <param name="loggerDbContext">数据库上下文</param>
    /// <returns>统计信息</returns>
    public static async Task<TracingStatistics> GetTracingStatistics(ILoggerDbContext loggerDbContext)
    {
        var totalCount = await loggerDbContext.Tracings.CountAsync();
        var todayCount = await loggerDbContext.Tracings.CountAsync(x => x.CreatedAt >= DateTime.Today);
        var yesterdayCount = await loggerDbContext.Tracings.CountAsync(x => 
            x.CreatedAt >= DateTime.Today.AddDays(-1) && x.CreatedAt < DateTime.Today);
        var weekCount = await loggerDbContext.Tracings.CountAsync(x => 
            x.CreatedAt >= DateTime.Today.AddDays(-7));
        
        var oldestRecord = await loggerDbContext.Tracings
            .OrderBy(x => x.CreatedAt)
            .Select(x => x.CreatedAt)
            .FirstOrDefaultAsync();
        
        var newestRecord = await loggerDbContext.Tracings
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        return new TracingStatistics
        {
            TotalCount = totalCount,
            TodayCount = todayCount,
            YesterdayCount = yesterdayCount,
            WeekCount = weekCount,
            OldestRecord = oldestRecord,
            NewestRecord = newestRecord
        };
    }
}

/// <summary>
/// Tracing统计信息
/// </summary>
public class TracingStatistics
{
    /// <summary>
    /// 总记录数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 今日记录数
    /// </summary>
    public int TodayCount { get; set; }

    /// <summary>
    /// 昨日记录数
    /// </summary>
    public int YesterdayCount { get; set; }

    /// <summary>
    /// 本周记录数
    /// </summary>
    public int WeekCount { get; set; }

    /// <summary>
    /// 最早记录时间
    /// </summary>
    public DateTime? OldestRecord { get; set; }

    /// <summary>
    /// 最新记录时间
    /// </summary>
    public DateTime? NewestRecord { get; set; }
}