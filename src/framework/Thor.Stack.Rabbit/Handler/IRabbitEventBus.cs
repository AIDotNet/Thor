using RabbitMQ.Client.Events;

namespace Thor.Rabbit.Handler;

public interface IRabbitEventBus
{
    Task Trigger(IServiceProvider sp, BasicDeliverEventArgs args, ConsumeOptions options);
}