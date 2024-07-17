using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.SparkDesk.API;
using Thor.SparkDesk.Chats.Dtos;
using Thor.SparkDesk.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Thor.SparkDesk.Chats;

/// <summary>
/// 讯飞星火对话补全服务
/// </summary>
/// <param name="logger"></param>
public sealed class SparkDeskChatCompletionsService(
    ILogger<SparkDeskChatCompletionsService> logger,
    IHttpClientFactory httpClientFactory)
    : IThorChatCompletionsService
{
    /// <summary>
    /// http 客户端
    /// </summary>
    private HttpClient HttpClient => httpClientFactory.CreateClient(nameof(SparkDeskPlatformOptions.PlatformCode));

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
    };

    /// <summary>
    /// 非流式对话补全
    /// </summary>
    /// <param name="request">对话补全请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(
        ThorChatCompletionsRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Model))
        {
            throw new NotModelException(request.Model);
        }

        // https://spark-api-open.xf-yun.com/v1/chat/completions
        var requestUri = new Uri($"{options.Address.TrimEnd('/')}/v1/chat/completions");
        (string appId, string apiKey, string apiSecret) = SparkDeskApiKeyHelper.ParseThorApiKey(options.ApiKey);


        var spardDeskRequest = SparkDeskChatCompletionsRequest.CreateByThorChatCompletionsRequest(request);

        var json = JsonSerializer.Serialize(spardDeskRequest, JsonOptions);

        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage()
        {
            RequestUri = requestUri,
            Method = HttpMethod.Post,
            Content = requestContent,
        };

        // 添加 Bearer 认证
        requestMessage.Headers.Add("Authorization", $"Bearer {apiKey}:{apiSecret}");

        var responseMessage = await HttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
        //var content = await responseMessage.Content.ReadAsStringAsync();
        using var responseContentStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
        var sparkDeskResponse = await JsonSerializer.DeserializeAsync<SparkDeskChatCompletionsResponse>(responseContentStream);

        var thorResponse = sparkDeskResponse.ToThorChatCompletionsResponse(false, spardDeskRequest.Model);
        return thorResponse;
    }

    /// <summary>
    /// 流式对话补全
    /// </summary>
    /// <param name="request">对话补全请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(ThorChatCompletionsRequest request,
        ThorPlatformOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Model))
        {
            throw new NotModelException(request.Model);
        }

        // https://spark-api-open.xf-yun.com/v1/chat/completions
        var requestUri = new Uri($"{options.Address.TrimEnd('/')}/v1/chat/completions");
        (string appId, string apiKey, string apiSecret) = SparkDeskApiKeyHelper.ParseThorApiKey(options.ApiKey);


        var spardDeskRequest = SparkDeskChatCompletionsRequest.CreateByThorChatCompletionsRequest(request);

        var json = JsonSerializer.Serialize(spardDeskRequest, JsonOptions);

        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage()
        {
            RequestUri = requestUri,
            Method = HttpMethod.Post,
            Content = requestContent,
        };

        // 添加 Bearer 认证
        requestMessage.Headers.Add("Authorization", $"Bearer {apiKey}:{apiSecret}");

        var responseMessage = await HttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        using StreamReader reader = new(await responseMessage.Content.ReadAsStreamAsync(cancellationToken));

        while (reader.EndOfStream == false)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            line = SparkDeskEventStreamHelper.RemovePrefix(line);

            if (SparkDeskEventStreamHelper.IsStreamEndText(line))
            {
                continue;
            }

            var sparkDeskResponse = JsonSerializer.Deserialize<SparkDeskChatCompletionsResponse>(line);

            yield return sparkDeskResponse.ToThorChatCompletionsResponse(true, spardDeskRequest.Model);
        }
    }

}