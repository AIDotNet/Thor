using System.Text.Json;
using System.Text.Json.Serialization;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AIDotNet.AliyunFC;

public class AliyunFCService : IADNChatCompletionService
{
    static AliyunFCService()
    {
        IADNChatCompletionService.ServiceNames.Add(AliyunFCOptions.ServiceName);
    }
    public AliyunFCOptions options { get; }

    public AliyunFCService(AliyunFCOptions options)
    {
        this.options = options;
    }

    public IReadOnlyDictionary<string, object?> Attributes { get; set; }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = new CancellationToken())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        var input = new AliyunFCDto()
        {
            input = new AliyunFCInput()
            {
                messages = chatHistory.Select(x => new AliyunFCMessages()
                {
                    content = x.Content,
                    role = x.Role.ToString()
                }).ToArray()
            },
            parameters = new AliyunFCParameters()
            {
                max_length = settings.MaxTokens ?? 500,
                do_sample = true,
            }
        };

        var apiKey = string.Empty;
        var apiUrl = string.Empty;
        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            apiKey = key.ToString();
        }
        else
        {
            apiKey = options.ApiKey;
        }

        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_URL, out var url) == true)
        {
            apiUrl = url.ToString();
        }
        else
        {
            apiUrl = options.ApiUrl;
        }


        var response = await options.HttpClient?.PostAsync(apiUrl, input, apiKey);

        var reader = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));


        var res = JsonSerializer.Deserialize<AliyunFCOutputDto>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

        var content = res?.Data.message.content;

        return new[]
        {
            new ChatMessageContent(AuthorRole.Assistant, content)
        };
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        var input = new AliyunFCDto()
        {
            input = new AliyunFCInput()
            {
                messages = chatHistory.Select(x => new AliyunFCMessages()
                {
                    content = x.Content,
                    role = x.Role.ToString()
                }).ToArray()
            },
            parameters = new AliyunFCParameters()
            {
                max_length = settings.MaxTokens ?? 500,
                do_sample = true,
            }
        };

        var apiKey = string.Empty;
        var apiUrl = string.Empty;
        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            apiKey = key.ToString();
        }
        else
        {
            apiKey = options.ApiKey;
        }

        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_URL, out var url) == true)
        {
            apiUrl = url.ToString();
        }
        else
        {
            apiUrl = options.ApiUrl;
        }


        var response = await options.HttpClient?.HttpRequestRaw(apiUrl, input, apiKey);

        var reader = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            var res = JsonSerializer.Deserialize<AliyunFCOutputDto>(line, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var content = res?.Data.message.content;

            if (string.IsNullOrEmpty(content)) continue;

            yield return new StreamingChatMessageContent(AuthorRole.Assistant, content);
        }
    }
}