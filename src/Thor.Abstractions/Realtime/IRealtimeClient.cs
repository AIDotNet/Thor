using System.Buffers;
using System.Net.WebSockets;
using Thor.Abstractions.Realtime.Dto;

namespace Thor.Abstractions.Realtime;

public interface IRealtimeClient : IDisposable
{
    event EventHandler<RealtimeResult>? OnMessage;
    
    event EventHandler<(Memory<byte>, bool)> OnBinaryMessage;

    Task OpenAsync(OpenRealtimeInput input, ThorPlatformOptions? options = null);

    Task SendAsync(RealtimeInput input);
}