using System.Net.Http.Json;
using System.Text.Json;
using Thor.Abstractions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.AzureOpenAI;

public class AzureOpenAiService(IHttpClientFactory httpClientFactory) : IApiChatCompletionService
{
    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(AzureOpenAIPlatformOptions.PlatformCode);

        var url = AzureOpenAIFactory.GetAddress(options, chatCompletionCreate.Model);

        chatCompletionCreate.Model = null;

        var response = await client.PostJsonAsync(url, chatCompletionCreate, options.ApiKey, "Api-Key");

        var result = await response.Content
            .ReadFromJsonAsync<ChatCompletionCreateResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result;
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(
        ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(AzureOpenAIPlatformOptions.PlatformCode);

        var url = AzureOpenAIFactory.GetAddress(options, chatCompletionCreate.Model);

        chatCompletionCreate.Model = null;
        
        var response = await client.HttpRequestRaw(url,
            chatCompletionCreate, options.ApiKey,"Api-Key");

        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith('{'))
            {
                // 如果是json数据则直接返回
                yield return JsonSerializer.Deserialize<ChatCompletionCreateResponse>(line,
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

            var result = JsonSerializer.Deserialize<ChatCompletionCreateResponse>(line,
                ThorJsonSerializer.DefaultOptions);
            yield return result;
        }
    }
}