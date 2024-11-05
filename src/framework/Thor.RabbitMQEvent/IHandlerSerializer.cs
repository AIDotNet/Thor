namespace Thor.RabbitMQEvent;

/// <summary>
/// Represents the interface for the handler serializer.
/// </summary>
public interface IHandlerSerializer
{
    byte[] Serialize<TEvent>(TEvent eventEvent) where TEvent : class;

    TEvent? Deserialize<TEvent>(ReadOnlyMemory<byte> data) where TEvent : class;

    object? Deserialize(byte[] data, Type type);

    byte[] Serialize(object @event);
}