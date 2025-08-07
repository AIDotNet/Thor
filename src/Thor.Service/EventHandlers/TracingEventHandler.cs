using Microsoft.Extensions.Options;
using Thor.BuildingBlocks.Event;
using Thor.Core.DataAccess;
using Thor.Domain.Chats;
using Thor.Service.Options;

namespace Thor.Service.EventBus;

public sealed class TracingEventHandler(
    ILogger<TracingEventHandler> logger,
    ILoggerDbContext loggerDbContext,
    IOptions<TracingOptions> tracingOptions)
    : IEventHandler<Tracing>, IDisposable
{
    public void Dispose()
    {
        logger.LogInformation("TracingEventHandler 释放资源");
    }

    public async Task HandleAsync(Tracing @event)
    {
        // 检查是否启用EventHandler
        if (!tracingOptions.Value.Enable || !tracingOptions.Value.EnableEventHandler)
        {
            logger.LogDebug("TracingEventHandler 已禁用，跳过处理事件");
            return;
        }

        logger.LogInformation("TracingEventHandler 处理事件: {Event}", @event);

        @event.Id = Guid.NewGuid().ToString();
        await loggerDbContext.Tracings.AddAsync(@event);
        await loggerDbContext.SaveChangesAsync();
    }
}