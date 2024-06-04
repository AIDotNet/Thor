namespace AIDotNet.Abstractions;

public interface IEventHandler<in TEvent> where TEvent : class
{
    Task HandleAsync(TEvent @event);
}