using AIDotNet.Abstractions;
using AIDotNet.MetaGLM.Models.RequestModels;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AIDotNet.MetaGLM;

public class MetaGLMService : IADNChatCompletionService
{
    static MetaGLMService()
    {
        IADNChatCompletionService.ServiceNames.Add(MetaGLMOptions.ServiceName);
    }
    private readonly MetaGLMOptions _openAiOptions;
    public MetaGLMService(MetaGLMOptions openAiOptions)
    {
        _openAiOptions = openAiOptions;
    }

    public IReadOnlyDictionary<string, object?> Attributes { get; }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = new CancellationToken())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        var apiKey = string.Empty;

        if (settings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            apiKey = key.ToString();
        }


        var dto = new TextRequestBase();
        dto.SetRequestId(Guid.NewGuid().ToString());
        dto.SetMessages(chatHistory.Select(x => new MessageItem
        {
            content = x.Content,
            role = x.Role.ToString()
        }).ToArray());
        dto.SetModel(settings.ModelId);
        dto.SetTemperature(settings.Temperature);
        dto.SetTopP(settings.TopP);

        var result = await _openAiOptions.Client?.Chat.Completion(dto, apiKey);

        return new ChatMessageContent[]
        {
            new(AuthorRole.Assistant, result.choices.FirstOrDefault()?.message.content)
        };
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        var apiKey = string.Empty;

        if (settings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            apiKey = key.ToString();
        }


        var dto = new TextRequestBase();
        dto.SetRequestId(Guid.NewGuid().ToString());
        dto.SetMessages(chatHistory.Select(x => new MessageItem
        {
            content = x.Content,
            role = x.Role.ToString()
        }).ToArray());
        dto.SetModel(settings.ModelId);
        dto.SetTemperature(settings.Temperature);
        dto.SetTopP(settings.TopP);

        await foreach (var item in _openAiOptions.Client?.Chat.Stream(dto, apiKey))
        {
            yield return
                new StreamingChatMessageContent(AuthorRole.Assistant, item.choices.FirstOrDefault()?.delta.content);
        }
    }
}