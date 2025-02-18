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
            await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(url, chatCompletionCreate, options.ApiKey, "Api-Key");

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

        var response = await HttpClientFactory.GetHttpClient(options.Address).HttpRequestRaw(url,
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
        var first = true;
        var isThink = false;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith('{'))
            {
                logger.LogInformation("AzureOpenAI对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                    line);

                throw new BusinessException("AzureOpenAI对话异常", line);
            }

            if (line.StartsWith(OpenAIConstant.Data))
                line = line[OpenAIConstant.Data.Length..];

            line = line.Trim();

            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line == OpenAIConstant.Done)
            {
                break;
            }

            if (line.StartsWith(':'))
            {
                continue;
            }


            var result = JsonSerializer.Deserialize<ThorChatCompletionsResponse>(line,
                ThorJsonSerializer.DefaultOptions);

            var content = result?.Choices?.FirstOrDefault()?.Delta;

            if (first && string.IsNullOrWhiteSpace(content?.Content) && string.IsNullOrEmpty(content?.ReasoningContent))
            {
                continue;
            }

            if (first && content.Content == OpenAIConstant.ThinkStart)
            {
                isThink = true;
                continue;
                // 需要将content的内容转换到其他字段
            }

            if (isThink && content.Content.Contains(OpenAIConstant.ThinkEnd))
            {
                isThink = false;
                // 需要将content的内容转换到其他字段
                continue;
            }

            if (isThink)
            {
                // 需要将content的内容转换到其他字段
                foreach (var choice in result.Choices)
                {
                    choice.Delta.ReasoningContent = choice.Delta.Content;
                    choice.Delta.Content = string.Empty;
                }
            }

            first = false;

            yield return result;
        }
    }
}