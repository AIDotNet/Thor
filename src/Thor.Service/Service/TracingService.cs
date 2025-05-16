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
}