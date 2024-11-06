using Thor.BuildingBlocks.Data;
using Thor.Core.DataAccess;

namespace Thor.Service.EventBus;

[Registration(typeof(IEventHandler<ChatLogger>))]
public sealed class ChatLoggerEventHandler(
    ILogger<ChatLoggerEventHandler> logger,
    ILoggerDbContext loggerDbContext)
    : IEventHandler<ChatLogger>, IDisposable, IScopeDependency
{
    public async Task HandleAsync(ChatLogger @event)
    {
        @event.Id = Guid.NewGuid().ToString("N");
        @event.CreatedAt = DateTime.Now;
        await loggerDbContext.Loggers.AddAsync(@event);
        await loggerDbContext.SaveChangesAsync();

        logger.LogInformation("ChatLogger event received");
    }

    public void Dispose()
    {
        logger.LogInformation("渠道事件处理器已释放");
    }
}