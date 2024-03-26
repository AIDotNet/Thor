using AIDotNet.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AIDotNet.Claudia;

public class ClaudiaService : IADNChatCompletionService
{
    static ClaudiaService()
    {
        IADNChatCompletionService.ServiceNames.Add("Claudia", ClaudiaOptions.ServiceName);
    }

    private readonly ClaudiaOptions _openAiOptions;

    public ClaudiaService(ClaudiaOptions openAiOptions)
    {
        _openAiOptions = openAiOptions;
    }

    public IReadOnlyDictionary<string, object?> Attributes { get; }

    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}