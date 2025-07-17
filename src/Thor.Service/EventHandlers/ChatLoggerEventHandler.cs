using Thor.BuildingBlocks.Event;
using Thor.Core.DataAccess;
using Thor.Domain.Chats;

namespace Thor.Service.EventBus;

public sealed class ChatLoggerEventHandler(
    ILogger<ChatLoggerEventHandler> logger,
    ILoggerDbContext loggerDbContext)
    : IEventHandler<ChatLogger>, IDisposable
{
    public async Task HandleAsync(ChatLogger @event)
    {
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