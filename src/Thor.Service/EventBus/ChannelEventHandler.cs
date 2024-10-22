using Thor.BuildingBlocks.Data;

namespace Thor.Service.EventBus;

[Registration(typeof(IEventHandler<ChatLogger>))]
public sealed class ChannelEventHandler : IEventHandler<ChatLogger>, IDisposable, ISingletonDependency
{
    private readonly ILogger<ChannelEventHandler> _logger;
    private readonly IServiceScope _scope;
    private readonly LoggerDbContext _loggerDbContext;

    public ChannelEventHandler(IServiceProvider serviceProvider, ILogger<ChannelEventHandler> logger)
    {
        _logger = logger;
        _scope = serviceProvider.CreateScope();
        _loggerDbContext = _scope.ServiceProvider.GetRequiredService<LoggerDbContext>();
    }

    public async Task HandleAsync(ChatLogger @event)
    {
        @event.Id = Guid.NewGuid().ToString("N");
        await _loggerDbContext.Loggers.AddAsync(@event);
        await _loggerDbContext.SaveChangesAsync();

        _logger.LogInformation("ChatLogger event received");
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}