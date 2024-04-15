using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Extensions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace AIDotNet.OpenAI;

public sealed class OpenAiService(IHttpClientFactory httpClientFactory) : IApiChatCompletionService
{
    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(nameof(OpenAIServiceOptions.ServiceName));

        var response = await client.PostJsonAsync(options?.Address.TrimEnd('/') + "/v1/chat/completions",
            chatCompletionCreate, options.Key);

        var result =
            await response.Content.ReadFromJsonAsync<ChatCompletionCreateResponse>(
                cancellationToken: cancellationToken);

        return result;
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(
        ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(nameof(OpenAIServiceOptions.ServiceName));

        var response = await client.HttpRequestRaw(options?.Address.TrimEnd('/') + "/v1/chat/completions",
            chatCompletionCreate, options.Key);

        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith("data:"))
                line = line.Substring("data:".Length);

            line = line.Trim();

            if (line == "[DONE]")
            {
                break;
            }

            if (line.StartsWith(":"))
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(line)) continue;
            
            var result =
                JsonSerializer.Deserialize<ChatCompletionCreateResponse>(line,
                    AIDtoNetJsonSerializer.DefaultOptions);
            yield return result;
        }
    }
}