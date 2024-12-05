using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;

namespace Thor.AzureOpenAI.Chats;

public class AzureOpenAIChatCompletionsService(ILogger<AzureOpenAIChatCompletionsService> logger)
    : IThorChatCompletionsService
{
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest chatCompletionCreate,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("Azure OpenAI 对话补全");
        var url = AzureOpenAIFactory.GetAddress(options, chatCompletionCreate.Model);

        var response =
            await HttpClientFactory.HttpClient.PostJsonAsync(url, chatCompletionCreate, options.ApiKey, "Api-Key");

        openai?.SetTag("Address", options?.Address.TrimEnd('/') + "/v1/chat/completions");
        openai?.SetTag("Model", chatCompletionCreate.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());
        // 如果限流则抛出限流异常
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new ThorRateLimitException();
        }

        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            logger.LogError("Azure对话异常 , StatusCode: {StatusCode} Response: {Response} Url:{Url}", response.StatusCode,
                await response.Content.ReadAsStringAsync(cancellationToken), url);
        }

        var result = await response.Content
            .ReadFromJsonAsync<ThorChatCompletionsResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);


        return result;
    }

    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
        ThorChatCompletionsRequest chatCompletionCreate, ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("Azure OpenAI 对话流式补全");
        var url = AzureOpenAIFactory.GetAddress(options, chatCompletionCreate.Model);

        var response = await HttpClientFactory.HttpClient.HttpRequestRaw(url,
            chatCompletionCreate, options.ApiKey, "Api-Key");

        openai?.SetTag("Address", options?.Address.TrimEnd('/') + "/v1/chat/completions");
        openai?.SetTag("Model", chatCompletionCreate.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            logger.LogError("Azure对话异常 , StatusCode: {StatusCode} ", response.StatusCode);
        }

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith('{'))
            {
                logger.LogDebug("Azure对话异常返回数据: {Data}", line);

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