using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Anthropic;
using Thor.Abstractions.Extensions;

namespace Thor.Claude.Chats;

public class AnthropicChatCompletionsService(ILogger<AnthropicChatCompletionsService> logger)
    : IAnthropicChatCompletionsService
{
    public async Task<ClaudeChatCompletionDto> ChatCompletionsAsync(AnthropicInput input,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("Claudia 对话补全");

        if (string.IsNullOrEmpty(options.Address))
        {
            options.Address = "https://api.anthropic.com/";
        }

        var client = HttpClientFactory.GetHttpClient(options.Address);

        var headers = new Dictionary<string, string>
        {
            { "x-api-key", options.ApiKey },
            { "authorization", "Bearer " + options.ApiKey },
            { "anthropic-version", "2023-06-01" }
        };


        bool isThink = input.Model.EndsWith("-thinking");
        input.Model = input.Model.Replace("-thinking", string.Empty);

        var budgetTokens = 1024;
        if (input.MaxTokens is < 2048)
        {
            input.MaxTokens = 2048;
        }

        if (input.MaxTokens != null && input.MaxTokens / 2 < 1024)
        {
            budgetTokens = input.MaxTokens.Value / (4 * 3);
        }

        var response =
            await client.PostJsonAsync(options.Address.TrimEnd('/') + "/v1/messages", input, string.Empty, headers);

        openai?.SetTag("Model", input.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new Exception("OpenAI对话异常" + response.StatusCode.ToString());
        }

        var value =
            await response.Content.ReadFromJsonAsync<ClaudeChatCompletionDto>(ThorJsonSerializer.DefaultOptions,
                cancellationToken: cancellationToken);

        return value;
    }

    public async IAsyncEnumerable<(string?, ClaudeStreamDto?)> StreamChatCompletionsAsync(AnthropicInput input,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("Claudia 对话补全");

        if (string.IsNullOrEmpty(options.Address))
        {
            options.Address = "https://api.anthropic.com/";
        }

        var client = HttpClientFactory.GetHttpClient(options.Address);

        var headers = new Dictionary<string, string>
        {
            { "x-api-key", options.ApiKey },
            { "authorization", options.ApiKey },
            { "anthropic-version", "2023-06-01" }
        };

        var isThinking = input.Model.EndsWith("thinking");
        input.Model = input.Model.Replace("-thinking", string.Empty);

        var response = await client.HttpRequestRaw(options.Address.TrimEnd('/') + "/v1/messages", input, string.Empty,
            headers);

        openai?.SetTag("Model", input.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new Exception("OpenAI对话异常" + response.StatusCode);
        }

        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        var first = true;
        var isThink = false;

        string? toolId = null;
        string? toolName = null;
        string? data = null;
        while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith('{'))
            {
                logger.LogInformation("OpenAI对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                    line);

                throw new Exception("OpenAI对话异常" + line);
            }

            if (line.StartsWith(OpenAIConstant.Data))
            {
                data = line[OpenAIConstant.Data.Length..].Trim();
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (data == OpenAIConstant.Done)
            {
                break;
            }

            if (line.StartsWith(':'))
            {
                yield return (line, null);
                continue;
            }

            if (string.IsNullOrWhiteSpace(data))
            {
                yield return (line, null);
                continue;
            }

            var result = JsonSerializer.Deserialize<ClaudeStreamDto>(data,
                ThorJsonSerializer.DefaultOptions);

            yield return (line, result);
        }
    }
}