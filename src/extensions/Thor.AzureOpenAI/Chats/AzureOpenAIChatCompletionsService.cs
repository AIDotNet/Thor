using System.Net.Http.Json;
using System.Text.Json;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Extensions;

namespace Thor.AzureOpenAI.Chats;

public class AzureOpenAIChatCompletionsService : IThorChatCompletionsService
{
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest chatCompletionCreate,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var url = AzureOpenAIFactory.GetAddress(options, chatCompletionCreate.Model);

        chatCompletionCreate.Model = null;

        var response = await HttpClientFactory.HttpClient.PostJsonAsync(url, chatCompletionCreate, options.ApiKey, "Api-Key");

        var result = await response.Content
            .ReadFromJsonAsync<ThorChatCompletionsResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result;
    }

    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
        ThorChatCompletionsRequest chatCompletionCreate, ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var url = AzureOpenAIFactory.GetAddress(options, chatCompletionCreate.Model);

        chatCompletionCreate.Model = null;

        var response = await HttpClientFactory.HttpClient.HttpRequestRaw(url,
            chatCompletionCreate, options.ApiKey, "Api-Key");

        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith('{'))
            {
                // 如果是json数据则直接返回
                yield return JsonSerializer.Deserialize<ThorChatCompletionsResponse>(line,
                    ThorJsonSerializer.DefaultOptions);

                break;
            }

            if (line.StartsWith("data:"))
                line = line["data:".Length..];

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

            var result = JsonSerializer.Deserialize<ThorChatCompletionsResponse>(line,
                ThorJsonSerializer.DefaultOptions);
            yield return result;
        }
    }
}