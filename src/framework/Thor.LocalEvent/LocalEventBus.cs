﻿using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Thor.BuildingBlocks.Data;

namespace Thor.LocalEvent;

public sealed class LocalEventBus<TEvent> : IEventBus<TEvent>, IDisposable where TEvent : class
{
    private readonly Channel<TEvent> _loggerChannel = Channel.CreateUnbounded<TEvent>();

    private readonly CancellationTokenSource _cts = new();
    
    public LocalEventBus(IEventHandler<TEvent> eventHandler, ILogger<IEventHandler<TEvent>> logger)
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

    public async ValueTask PublishAsync(TEvent @event)
    {
        await _loggerChannel.Writer.WriteAsync(@event).ConfigureAwait(false);
    }

    public void Dispose()
    {
        _cts.Dispose();
    }
}