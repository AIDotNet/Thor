using System.Net.WebSockets;
using Thor.Abstractions.Realtime.Dto;

namespace Thor.Abstractions.Realtime;

public interface IRealtimeClient : IDisposable
{
    Task OpenAsync(OpenRealtimeInput input, ThorPlatformOptions? options = null);

    Task SendAsync(RealtimeInput input);

    Task OnMessageAsync(WebSocket webSocket);
}