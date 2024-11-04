using Thor.BuildingBlocks.Data;
using Thor.Core;
using Thor.Core.DataAccess;

namespace Thor.Service.EventBus;

[Registration(typeof(IEventHandler<ChatLogger>))]
public sealed class ChannelEventHandler(IServiceProvider serviceProvider, ILogger<ChannelEventHandler> logger)
    : IEventHandler<ChatLogger>, IDisposable, IScopeDependency
{
    private readonly ILoggerDbContext _loggerDbContext = serviceProvider.GetRequiredService<ILoggerDbContext>();

    public async Task HandleAsync(ChatLogger @event)
    {
        @event.Id = Guid.NewGuid().ToString("N");
        await _loggerDbContext.Loggers.AddAsync(@event);
        await _loggerDbContext.SaveChangesAsync();

        logger.LogInformation("ChatLogger event received");
    }

    public void Dispose()
    {
        logger.LogInformation("ChannelEventHandler disposed");
    }
}