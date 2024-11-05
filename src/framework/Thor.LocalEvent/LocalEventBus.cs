using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Thor.BuildingBlocks.Data;

namespace Thor.LocalEvent;

public sealed class LocalEventBus<TEvent> : IEventBus<TEvent>, IDisposable where TEvent : class
{
    private readonly Channel<TEvent> _loggerChannel = Channel.CreateUnbounded<TEvent>();

    private readonly CancellationTokenSource _cts = new();
    
    public LocalEventBus(IServiceProvider serviceProvider, ILogger<IEventHandler<TEvent>> logger)
    {
        Task.Run(async () =>
        {
            await using  var scope = serviceProvider.CreateAsyncScope();
            while (_cts.Token.IsCancellationRequested == false)
            {
                var eventHandler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
                
                await foreach (var @event in _loggerChannel.Reader.ReadAllAsync(_cts.Token))
                {
                    try
                    {
                        logger.LogInformation($"ChannelEventBus: {typeof(TEvent).Name} Event Received. Processing...");
                        await eventHandler.HandleAsync(@event);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"ChannelEventBus Error: {e.Message}");
                    }
                }
            }
        }, _cts.Token);
    }

    public async ValueTask PublishAsync(TEvent eventEvent)
    {
        await _loggerChannel.Writer.WriteAsync(eventEvent).ConfigureAwait(false);
    }

    public void Dispose()
    {
        _cts.Dispose();
    }
}