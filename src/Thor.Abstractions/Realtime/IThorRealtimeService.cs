namespace Thor.Abstractions.Realtime;

public interface IThorRealtimeService
{
    IRealtimeClient CreateClient();
}