namespace Thor.Rabbit;

public interface IDeclaration
{
    Task QueueDeclareAsync(string queue, bool durable, bool autoDelete = false, bool exclusive = false,
        IDictionary<string, object> arguments = null);

    Task ExchangeDeclareAsync(string exchange, string type, bool durable, bool autoDelete = false,
        IDictionary<string, object> arguments = null);

    Task QueueBindAsync(string queue, string exchange, string routingKey,
        IDictionary<string, object> arguments = null);
}

public class DefaultDeclaration : IDeclaration
{
    private readonly IChannel _channel;

    // ReSharper disable once ConvertToPrimaryConstructor
    public DefaultDeclaration(IChannel channel)
    {
        _channel = channel;
    }

    public async Task  QueueDeclareAsync(string queue, bool durable, bool autoDelete = false, bool exclusive = false,
        IDictionary<string, object> arguments = null)
    {
        await _channel.QueueDeclareAsync(
            queue: queue,
            durable: durable,
            autoDelete: autoDelete,
            exclusive: exclusive,
            arguments: arguments);
    }

    public async Task ExchangeDeclareAsync(string exchange, string type, bool durable, bool autoDelete = false,
        IDictionary<string, object> arguments = null)
    {
        await _channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: type,
            durable: durable,
            autoDelete: autoDelete,
            arguments: arguments);
    }

    public async Task QueueBindAsync(string queue, string exchange, string routingKey,
        IDictionary<string, object> arguments)
    {
        await _channel.QueueBindAsync(
            queue: queue,
            exchange: exchange,
            routingKey: routingKey,
            arguments: arguments);
    }
}