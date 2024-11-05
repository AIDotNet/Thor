using System.Text.Json;
using Thor.BuildingBlocks.Data;
using Thor.Rabbit;

namespace Thor.RabbitMQEvent;

public class RabbitMQEventBus<TEvent>(RabbitClient rabbitClient,IHandlerSerializer handlerSerializer) : IEventBus<TEvent> where TEvent : class
{
    public async ValueTask PublishAsync(TEvent eventEvent)
    {
        ArgumentNullException.ThrowIfNull(eventEvent);

        var eto = new EventEto(eventEvent.GetType().FullName, handlerSerializer.Serialize(eventEvent));

        await rabbitClient.PublishAsync("Thor:EventBus:exchange", "Thor:EventBus:key",
            handlerSerializer.Serialize(eto));
    }
}