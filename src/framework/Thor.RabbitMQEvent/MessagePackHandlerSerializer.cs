using MessagePack;
using MessagePack.Resolvers;

namespace Thor.RabbitMQEvent;

public class MessagePackHandlerSerializer : IHandlerSerializer
{
    public byte[] Serialize<TEvent>(TEvent eventEvent) where TEvent : class
    {
        return MessagePackSerializer.Serialize(eventEvent, ContractlessStandardResolver.Options);
    }

    public TEvent? Deserialize<TEvent>(ReadOnlyMemory<byte> data) where TEvent : class
    {
        return MessagePackSerializer.Deserialize<TEvent>(data, ContractlessStandardResolver.Options);
    }

    public object? Deserialize(byte[] data, Type type)
    {
        return MessagePackSerializer.Deserialize(type, data, ContractlessStandardResolver.Options);
    }

    public byte[] Serialize(object @event)
    {
        return MessagePackSerializer.Serialize(@event, ContractlessStandardResolver.Options);
    }
}