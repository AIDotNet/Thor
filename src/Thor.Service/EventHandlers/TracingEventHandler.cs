using Thor.BuildingBlocks.Event;
using Thor.Core.DataAccess;
using Thor.Domain.Chats;

namespace Thor.Service.EventBus;

public sealed class TracingEventHandler(
    ILogger<TracingEventHandler> logger,
    ILoggerDbContext loggerDbContext)
    : IEventHandler<Tracing>, IDisposable
{
    public void Dispose()
    {
        logger.LogInformation("TracingEventHandler 释放资源");
    }

    public async Task HandleAsync(Tracing @event)
    {
        logger.LogInformation("TracingEventHandler 处理事件: {Event}", @event);

        await loggerDbContext.Tracings.AddAsync(@event);
        await loggerDbContext.SaveChangesAsync();
    }
}