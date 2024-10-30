using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Thor.Abstractions;
using Thor.Abstractions.Realtime;
using Thor.Abstractions.Realtime.Dto;

namespace Thor.OpenAI.Realtime;

public class OpenAIRealtimeClient : IRealtimeClient
{
    private ClientWebSocket _clientWebSocket;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }

    public async Task OpenAsync(OpenRealtimeInput input, ThorPlatformOptions? options = null)
    {
        var url = new Uri(options?.Address.TrimEnd('/'));

        var wss = new UriBuilder(url)
        {
            Scheme = url.Scheme == "http" ? "ws" : "wss",
            Path = "/v1/realtime",
            Query = $"model={input.Model}"
        }.Uri;

        _clientWebSocket = new ClientWebSocket();

        _clientWebSocket.Options.AddSubProtocol("realtime");
        _clientWebSocket.Options.AddSubProtocol("openai-insecure-api-key." + options?.ApiKey);
        _clientWebSocket.Options.AddSubProtocol("openai-beta.realtime-v1");
        // _clientWebSocket.Options.SetRequestHeader("Authorization", $"Bearer {options?.ApiKey}");

        await _clientWebSocket.ConnectAsync(wss, CancellationToken.None);
    }

    public async Task SendAsync(RealtimeInput input)
    {
        await _clientWebSocket.SendAsync(new ArraySegment<byte>(JsonSerializer.SerializeToUtf8Bytes(input)),
            WebSocketMessageType.Text, true,
            _cancellationTokenSource.Token);
    }

    public async Task OnMessageAsync(WebSocket webSocket)
    {
        _ = Task.Run(async () =>
        {
            // 接收消息
            var buffer = ArrayPool<byte>.Shared.Rent(1024 * 1024 * 4);

            try
            {
                while (_cancellationTokenSource.Token.IsCancellationRequested == false)
                {
                    var result =
                        await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                            _cancellationTokenSource.Token);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    await webSocket.SendAsync(new ArraySegment<byte>(buffer.AsSpan(0, result.Count).ToArray()),
                        WebSocketMessageType.Text, false, _cancellationTokenSource.Token);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        });
    }
}