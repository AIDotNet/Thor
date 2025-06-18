using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.Responses;
using Thor.Abstractions.Responses.Dto;

namespace Thor.CustomOpenAI.Responses;

public sealed class OpenAIResponsesService(ILogger<OpenAIResponsesService> logger) : IThorResponsesService
{
    public async Task<ResponsesDto> GetResponseAsync(ResponsesInput input, ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("CustomeOpenAI 对话补全");

        var response = await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(
            options?.Address.TrimEnd('/') + "/responses",
            input, options.ApiKey).ConfigureAwait(false);

        openai?.SetTag("Model", input.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new BusinessException("渠道未登录,请联系管理人员", "401");
        }

        // 如果限流则抛出限流异常
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new ThorRateLimitException();
        }

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
        }

        var result =
            await response.Content.ReadFromJsonAsync<ResponsesDto>(
                cancellationToken: cancellationToken).ConfigureAwait(false);

        return result;
    }

    public async IAsyncEnumerable<(string @event, ResponsesSSEDto<ResponsesDto> responses)> GetResponsesAsync(ResponsesInput input,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("CustomeOpenAI 对话补全");

        var response = await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(
            options?.Address.TrimEnd('/') + "/responses",
            input, options.ApiKey).ConfigureAwait(false);

        openai?.SetTag("Model", input.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new BusinessException("渠道未登录,请联系管理人员", "401");
        }

        // 如果限流则抛出限流异常
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new ThorRateLimitException();
        }

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
        }


        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        var first = true;
        var isThink = false;

        var @event = string.Empty;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            // 新接口返回格式
            // event: response.created
            // data: {"type":"response.created","response":{"id":"resp_67c9fdcecf488190bdd9a0409de3a1ec07b8b0ad4e5eb654","object":"response","created_at":1741290958,"status":"in_progress","error":null,"incomplete_details":null,"instructions":"You are a helpful assistant.","max_output_tokens":null,"model":"gpt-4.1-2025-04-14","output":[],"parallel_tool_calls":true,"previous_response_id":null,"reasoning":{"effort":null,"summary":null},"store":true,"temperature":1.0,"text":{"format":{"type":"text"}},"tool_choice":"auto","tools":[],"top_p":1.0,"truncation":"disabled","usage":null,"user":null,"metadata":{}}}

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("event: "))
            {
                @event = line[6..].Trim();
                continue;
            }

            if (line.StartsWith("data: "))
            {
                line = line[6..].Trim();
            }
            

            var result = JsonSerializer.Deserialize<ResponsesSSEDto<ResponsesDto>>(line,
                ThorJsonSerializer.DefaultOptions);

            yield return (@event, result);
        }
    }
}