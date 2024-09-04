using System.Threading.Channels;
using Thor.BuildingBlocks.Data;
using Thor.Service.Options;

namespace Thor.Service.EventBus;

public sealed class ChannelEventHandler : IEventHandler<ChatLogger>, IDisposable
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
        await _loggerDbContext.Loggers.AddAsync(@event);
        await _loggerDbContext.SaveChangesAsync();

        _logger.LogInformation("ChatLogger event received");
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}