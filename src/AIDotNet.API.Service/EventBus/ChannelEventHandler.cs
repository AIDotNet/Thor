using AIDotNet.Abstractions;
using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Domain;

namespace AIDotNet.API.Service.EventBus;

public sealed class ChannelEventHandler : IEventHandler<ChatLogger>, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;

    public ChannelEventHandler(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
        _serviceProvider = _scope.ServiceProvider;
    }

    public async Task HandleAsync(ChatLogger @event)
    {
        var loggerDbContext = _serviceProvider.GetRequiredService<LoggerDbContext>();
        await loggerDbContext.Loggers.AddAsync(@event);
        
        await loggerDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}