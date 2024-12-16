using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.Abstractions;
using Thor.MetaGLM.Models.RequestModels;
using Thor.MetaGLM.Models.ResponseModels;

namespace Thor.MetaGLM.Modules;

public class Chat
{
    /// <summary>
    /// 
    /// </summary>
    private const int API_TOKEN_TTL_SECONDS = 60 * 5;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters =
        {
            new MessageItemConverter()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
    };

    private static async IAsyncEnumerable<string> CompletionBase(TextRequestBase textRequestBody, string apiKey,
        string? baseAddress = "https://open.bigmodel.cn/")
    {
        var requestUri = !string.IsNullOrWhiteSpace(baseAddress)
            ? new Uri(baseAddress.TrimEnd('/') + "/api/paas/v4/chat/completions")
            : new Uri("https://open.bigmodel.cn/api/paas/v4/chat/completions");


        var json = JsonSerializer.Serialize(textRequestBody, JsonOptions);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        // 参考文档：智谱AI开放平台:https://open.bigmodel.cn/dev/api#http_auth

        // 直接使用 api key 方式
        //var token = apiKey;

        // 通过 jwt 方式
        var token = string.IsNullOrEmpty(apiKey)
            ? string.Empty
            : AuthenticationUtils.GenerateToken(apiKey, API_TOKEN_TTL_SECONDS);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = requestUri,
            Content = data,
        };

        if (!string.IsNullOrEmpty(apiKey))
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        var response = await HttpClientFactory.GetHttpClient(requestUri.AbsoluteUri)
            .SendAsync(request, HttpCompletionOption.ResponseContentRead);
        var stream = await response.Content.ReadAsStreamAsync();
        var buffer = new byte[8192];
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
        {
            yield return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
    }

    public async Task<ResponseBase> Completion(TextRequestBase textRequestBody, string apiKey, string? baseAddress)
    {
        textRequestBody.stream = false;
        var sb = new StringBuilder();
        await foreach (var str in CompletionBase(textRequestBody, apiKey, baseAddress))
        {
            sb.Append(str);
        }

        return ResponseBase.FromJson(sb.ToString());
    }

    public async IAsyncEnumerable<ResponseBase> Stream(TextRequestBase textRequestBody, string apiKey,
        string? baseAddress = null)
    {
        textRequestBody.stream = true;
        var buffer = string.Empty;
        await foreach (var chunk in CompletionBase(textRequestBody, apiKey, baseAddress))
        {
            buffer += chunk;

            while (true)
            {
                int startPos = buffer.IndexOf("data: ", StringComparison.Ordinal);
                if (startPos == -1)
                {
                    break;
                }

                int endPos = buffer.IndexOf("\n\n", startPos, StringComparison.Ordinal);

                if (endPos == -1)
                {
                    break;
                }

                startPos += "data: ".Length;

                string jsonString = buffer.Substring(startPos, endPos - startPos);
                if (jsonString.Equals("[DONE]"))
                {
                    break;
                }

                var response = ResponseBase.FromJson(jsonString);
                if (response != null)
                {
                    yield return response;
                }

                buffer = buffer.Substring(endPos + "\n\n".Length);
            }
        }

        var finalResponse = ResponseBase.FromJson(buffer.Trim());
        if (finalResponse != null)
        {
            yield return finalResponse;
        }
    }
}