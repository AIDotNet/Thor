using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using Raccoon.Stack.Rabbit;
using Raccoon.Stack.Rabbit.Handler;
using Thor.BuildingBlocks.Data;

namespace Thor.RabbitMQEvent;

public class RabbitMQEventHandler : IRabbitHandler
{
    public bool Enable(ConsumeOptions options)
    {
        return options.Queue.Equals("Thor:EventBus", StringComparison.OrdinalIgnoreCase);
    }

    public async Task Handle(IServiceProvider sp, BasicDeliverEventArgs args, ConsumeOptions options)
    {
        var eto = JsonSerializer.Deserialize<EventEto>(args.Body.ToArray());

        // type : Thor.Service.Domain.ChatLogger

        var type = Assembly.GetEntryAssembly()?.GetType(eto.FullName);
        
        if (type == null)
        {
            return;
        }

        var @event = JsonSerializer.Deserialize(eto.Data, type);

        // IEventHandler<ChatLogger>
        var handlerType = typeof(IEventHandler<>).MakeGenericType(type);

        var handler = sp.GetRequiredService(handlerType);

        var method = handlerType.GetMethod("HandleAsync");

        await (Task)method.Invoke(handler, new object[] { @event });
    }
}