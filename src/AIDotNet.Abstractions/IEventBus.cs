namespace AIDotNet.Abstractions;

public interface IEventBus<in TEvent> where TEvent : class
{
    Task PublishAsync(TEvent @event);
}