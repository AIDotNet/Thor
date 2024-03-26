using AIDotNet.Abstractions;
using AIDotNet.MetaGLM.Models.RequestModels;
using AIDotNet.MetaGLM.Models.RequestModels.FunctionModels;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AIDotNet.MetaGLM;

public class MetaGLMService : IADNChatCompletionService
{
    static MetaGLMService()
    {
        IADNChatCompletionService.ServiceNames.Add("MetaGLM", MetaGLMOptions.ServiceName);
    }

    private readonly MetaGLMOptions _openAiOptions;

    public MetaGLMService(MetaGLMOptions openAiOptions)
    {
        _openAiOptions = openAiOptions;
    }

    public IReadOnlyDictionary<string, object?> Attributes { get; }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = new CancellationToken())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        var apiKey = string.Empty;
        var apiUrl = string.Empty;

        if (settings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            apiKey = key.ToString();
        }

        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_URL, out var url) == true)
        {
            apiUrl = url.ToString();
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

        var result = await _openAiOptions.Client?.Chat.Completion(dto, apiKey, apiUrl);

        return new ChatMessageContent[]
        {
            new(AuthorRole.Assistant, result.choices.FirstOrDefault()?.message.content)
        };
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        CancellationToken cancellationToken = new())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        var apiKey = string.Empty;
        var apiUrl = string.Empty;

        if (settings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            apiKey = key?.ToString();
        }

        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_URL, out var url) == true)
        {
            apiUrl = url.ToString();
        }

        var dto = new TextRequestBase();
        dto.SetRequestId(Guid.NewGuid().ToString());
        dto.SetMessages(chatHistory.Select(x => new MessageItem
        {
            content = x.Content ?? string.Empty,
            role = x.Role.ToString()
        }).Where(x => !string.IsNullOrEmpty(x.content)).ToArray());
        dto.SetModel(settings.ModelId);
        dto.SetTemperature(settings.Temperature);
        dto.SetTopP(settings.TopP);

        await foreach (var item in _openAiOptions.Client?.Chat.Stream(dto, apiKey, apiUrl))
        {
            yield return
                new StreamingChatMessageContent(AuthorRole.Assistant, item.choices.FirstOrDefault()?.delta.content);
        }
    }
}