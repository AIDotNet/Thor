namespace Thor.BuildingBlocks.Data;

/// <summary>
/// Represents the event bus.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventBus<in TEvent> where TEvent : class
{
    ValueTask PublishAsync(TEvent eventEvent);
}