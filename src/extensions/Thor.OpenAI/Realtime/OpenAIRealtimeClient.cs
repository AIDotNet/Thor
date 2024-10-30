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
    private readonly ClientWebSocket _socket = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public event EventHandler<RealtimeResult>? OnMessage;
    public event EventHandler<(Memory<byte>, bool)>? OnBinaryMessage;

    public void Dispose()
    {
        _socket.Dispose();
        _cancellationTokenSource.Dispose();
    }

    public async Task OpenAsync(OpenRealtimeInput input, ThorPlatformOptions? options = null)
    {
        _socket.Options.AddSubProtocol("realtime");
        _socket.Options.AddSubProtocol("openai-insecure-api-key." + options!.ApiKey);
        _socket.Options.AddSubProtocol("openai-beta.realtime-v1");

        var uri = new Uri(options.Address);
        await _socket.ConnectAsync(
            new Uri(uri.Scheme == "http"
                ? "ws"
                : "wss" + "://" + uri.Host + "/v1/realtime?model=" + input.Model),
            _cancellationTokenSource.Token);

        _ = Task.Run(async () =>
        {
            var buffer = ArrayPool<byte>.Shared.Rent(1024 * 1024 * 2);
            try
            {
                while (_socket.State == WebSocketState.Open)
                {
                    var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                        Console.WriteLine("Connection closed.");
                    }
                    else
                    {
                        if (result.EndOfMessage)
                        {
                            var content = JsonSerializer.Deserialize<RealtimeResult>(buffer.AsSpan(0, result.Count),
                                ThorJsonSerializer.DefaultOptions);
                            OnMessage?.Invoke(this, content);
                        }
                        else
                        {
                            OnBinaryMessage?.Invoke(this, (buffer.AsMemory(0, result.Count), result.EndOfMessage));

                            while (!result.EndOfMessage)
                            {
                                result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer),
                                    CancellationToken.None);
                                OnBinaryMessage?.Invoke(this, (buffer.AsMemory(0, result.Count), result.EndOfMessage));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            ArrayPool<byte>.Shared.Return(buffer);
        });
    }

    public Task SendAsync(RealtimeInput input)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(input, ThorJsonSerializer.DefaultOptions);
        return _socket.SendAsync(new ArraySegment<byte>(json), WebSocketMessageType.Text, true,
            CancellationToken.None);
    }
}