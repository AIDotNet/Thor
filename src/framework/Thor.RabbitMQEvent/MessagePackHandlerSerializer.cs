using MessagePack;
using MessagePack.Resolvers;

namespace Thor.RabbitMQEvent;

public class MessagePackHandlerSerializer : IHandlerSerializer
{
    private readonly MessagePackSerializerOptions _options = MessagePackSerializer.DefaultOptions.WithResolver(
        CompositeResolver.Create(StandardResolver.Instance, ContractlessStandardResolver.Instance));


    public byte[] Serialize<TEvent>(TEvent eventEvent) where TEvent : class
    {
        return MessagePackSerializer.Serialize(eventEvent, _options);
    }

    public TEvent? Deserialize<TEvent>(ReadOnlyMemory<byte> data) where TEvent : class
    {
        return MessagePackSerializer.Deserialize<TEvent>(data, _options);
    }

    public object? Deserialize(byte[] data, Type type)
    {
        return MessagePackSerializer.Deserialize(type, data, _options);
    }

    public byte[] Serialize(object @event)
    {
        return MessagePackSerializer.Serialize(@event, _options);
    }
}