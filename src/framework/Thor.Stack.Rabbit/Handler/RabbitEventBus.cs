using RabbitMQ.Client.Events;

namespace Thor.Rabbit.Handler;

public class RabbitEventBus : IRabbitEventBus
{
    private readonly IEnumerable<IRabbitHandler> _handlers;

    // ReSharper disable once ConvertToPrimaryConstructor
    public RabbitEventBus(IEnumerable<IRabbitHandler> handlers)
    {
        _handlers = handlers;
    }

    public async Task Trigger(IServiceProvider sp, BasicDeliverEventArgs args, ConsumeOptions options)
    {
        var handlers = _handlers.Where(q => q.Enable(options)).ToList();
        foreach (var handler in handlers)
        {
            await handler.Handle(sp, args, options);
        }
    }
}