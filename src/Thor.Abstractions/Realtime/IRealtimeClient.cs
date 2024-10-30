using System.Net.WebSockets;
using Thor.Abstractions.Realtime.Dto;

namespace Thor.Abstractions.Realtime;

public interface IRealtimeClient : IDisposable
{
    event EventHandler<RealtimeResult>? OnMessage;

    Task OpenAsync(OpenRealtimeInput input, ThorPlatformOptions? options = null);

    Task SendAsync(RealtimeInput input);
}