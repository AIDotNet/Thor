using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using Thor.BuildingBlocks.Data;
using Thor.Rabbit;
using Thor.Rabbit.Handler;

namespace Thor.RabbitMQEvent;

public class RabbitMQEventHandler(IHandlerSerializer handlerSerializer) : IRabbitHandler
{
    private readonly ConcurrentDictionary<string, Type> _types = new();

    public bool Enable(ConsumeOptions options)
    {
        return options.Queue.Equals("Thor:EventBus", StringComparison.OrdinalIgnoreCase);
    }

    public async Task Handle(IServiceProvider sp, BasicDeliverEventArgs args, ConsumeOptions options)
    {
        var eto = handlerSerializer.Deserialize<EventEto>(args.Body);

        var type = _types.GetOrAdd(eto.FullName, ((s) =>
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName?.Contains("Thor") == true && x.GetType(eto.FullName) != null)
                .Select(x => x).FirstOrDefault();

            var type = assembly?.GetType(eto.FullName);
            
            return type;
        }));

        if (type == null)
        {
            return;
        }

        var @event = handlerSerializer.Deserialize(eto.Data, type);

        // IEventHandler<ChatLogger>
        var handlerType = typeof(IEventHandler<>).MakeGenericType(type);

        var handler = sp.GetRequiredService(handlerType);

        var method = handlerType.GetMethod("HandleAsync");

        await (Task)method.Invoke(handler, new object[] { @event });
    }
}