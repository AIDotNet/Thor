using System.Text.Json;
using Raccoon.Stack.Rabbit;
using Thor.BuildingBlocks.Data;

namespace Thor.RabbitMQEvent;

public class RabbitMQEventBus<TEvent>(RabbitClient rabbitClient) : IEventBus<TEvent> where TEvent : class
{
    public async ValueTask PublishAsync(TEvent @event)
    {
        var eto = new EventEto(@event.GetType().FullName, JsonSerializer.SerializeToUtf8Bytes(@event));

        await rabbitClient.PublishAsync("Thor:EventBus:exchange", "Thor:EventBus:key",
            JsonSerializer.SerializeToUtf8Bytes(eto));
    }
}