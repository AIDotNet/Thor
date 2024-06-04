using System.Threading.Channels;
using AIDotNet.Abstractions;

namespace AIDotNet.API.Service.EventBus;

public class ChannelEventBus<TEvent> : IEventBus<TEvent>, IDisposable where TEvent : class
{
    public ChannelEventBus(IEventHandler<TEvent> eventHandler, ILogger<IEventHandler<TEvent>> logger)
    {
        Task.Run(async () =>
        {
            while (_cts.Token.IsCancellationRequested == false)
            {
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


    private readonly CancellationTokenSource _cts = new();

    private readonly Channel<TEvent> _loggerChannel = Channel.CreateUnbounded<TEvent>();

    public async Task PublishAsync(TEvent @event)
    {
        await _loggerChannel.Writer.WriteAsync(@event).ConfigureAwait(false);
    }

    public void Dispose()
    {
        _cts.Dispose();
    }
}