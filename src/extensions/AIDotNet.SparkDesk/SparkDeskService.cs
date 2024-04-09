using AIDotNet.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Sdcb.SparkDesk;
using TokenApi.Service.Exceptions;

namespace AIDotNet.SparkDesk;

public class SparkDeskService : IADNChatCompletionService
{
    private readonly SparkDeskOptions options;
    public SparkDeskService(SparkDeskOptions options)
    {
        this.options = options;
    }

    public IReadOnlyDictionary<string, object?> Attributes { get; set; }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = new())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        SparkDeskClient client;
        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            // appId|appKey|appSecret
            var parts = key.ToString().Split('|');
            if (parts.Length == 3)
            {
                client = new SparkDeskClient(parts[0], parts[1], parts[2]);
            }
            else
            {
                throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
            }
        }
        else
        {
            throw new InvalidOperationException("Client is not initialized");
        }

        ModelVersion modelVersion;
        if (executionSettings?.ModelId == "SparkDesk-v3.5")
        {
            modelVersion = ModelVersion.V3_5;
        }
        else if (executionSettings?.ModelId == "SparkDesk-v3.1")
        {
            modelVersion = ModelVersion.V3;
        }
        else if (executionSettings?.ModelId == "SparkDesk-v1.5")
        {
            modelVersion = ModelVersion.V1_5;
        }
        else if (executionSettings?.ModelId == "SparkDesk-v2.1")
        {
            modelVersion = ModelVersion.V2;
        }
        else
        {
            throw new NotModelException(executionSettings?.ModelId);
        }

        var topK = Convert.ToInt32(Math.Round(settings.TopP + 1));

        var results = chatHistory.Select(x => new ChatMessage(x.Role.ToString(), x.Content)).ToArray();

        var msg = await client.ChatAsync(modelVersion,
            results, new ChatRequestParameters
            {
                ChatId = Guid.NewGuid().ToString("N"),
                MaxTokens = (int)settings.MaxTokens,
                Temperature = (float)settings.Temperature,
                TopK = topK,
            }, cancellationToken: cancellationToken);

        return new List<ChatMessageContent>()
        {
            new(AuthorRole.Assistant, msg.Text, settings.ModelId)
        };
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        SparkDeskClient client;
        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            // appId|appKey|appSecret
            var parts = key.ToString().Split('|');
            if (parts.Length == 3)
            {
                client = new SparkDeskClient(parts[0], parts[1], parts[2]);
            }
            else
            {
                Console.WriteLine("Invalid API Key format, expected appId|appKey|appSecret");
                throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
            }
        }
        else
        {
            throw new InvalidOperationException("未找到 APIKEY配置");
        }


        ModelVersion modelVersion;
        if (executionSettings?.ModelId == "SparkDesk-v3.5")
        {
            modelVersion = ModelVersion.V3_5;
        }
        else if (executionSettings?.ModelId == "SparkDesk-v3.1")
        {
            modelVersion = ModelVersion.V3;
        }
        else if (executionSettings?.ModelId == "SparkDesk-v1.5")
        {
            modelVersion = ModelVersion.V1_5;
        }
        else if (executionSettings?.ModelId == "SparkDesk-v2.1")
        {
            modelVersion = ModelVersion.V2;
        }
        else
        {
            throw new NotModelException(executionSettings?.ModelId);
        }

        var topK = Convert.ToInt32(Math.Round(settings.TopP + 1));

        var results = chatHistory.Select(x => new ChatMessage(x.Role.ToString(), x.Content)).ToArray();

        if (settings.Temperature <= 0)
        {
            settings.Temperature = 0.1;
        }

        var msg = client.ChatAsStreamAsync(modelVersion,
            results, new ChatRequestParameters
            {
                ChatId = Guid.NewGuid().ToString("N"),
                MaxTokens = (int)settings.MaxTokens,
                Temperature = (float)settings.Temperature,
                TopK = topK,
            }, cancellationToken: cancellationToken);

        await foreach (var item in msg)
        {
            yield return new StreamingChatMessageContent(AuthorRole.Assistant, item.Text, settings.ModelId);
        }
    }
}