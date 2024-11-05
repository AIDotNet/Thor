namespace Thor.Rabbit.Handler;

public interface IRabbitHandler
{
    bool Enable(ConsumeOptions options);

    Task Handle(IServiceProvider sp, BasicDeliverEventArgs args, ConsumeOptions options);
}