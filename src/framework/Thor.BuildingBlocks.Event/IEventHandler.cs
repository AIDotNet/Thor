namespace Thor.BuildingBlocks.Event;

/// <summary>
/// Represents the event bus.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventHandler<in TEvent> where TEvent : class
{
    Task HandleAsync(TEvent @event);
}