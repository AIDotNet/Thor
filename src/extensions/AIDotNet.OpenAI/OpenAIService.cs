using System.Net.Http.Json;
using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.Abstractions.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AIDotNet.OpenAI;

public class OpenAiService : IADNChatCompletionService
{
    private readonly OpenAIOptions openAiOptions;

    public OpenAiService(OpenAIOptions options)
    {
        openAiOptions = options;
        IADNChatCompletionService.ServiceNames.Add(OpenAIOptions.ServiceName);
    }

    public IReadOnlyDictionary<string, object?> Attributes { get; }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null, CancellationToken cancellationToken = new())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        var apiKey = string.Empty;
        var apiUrl = string.Empty;

        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            apiKey = key.ToString();
        }

        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_URL, out var url) == true)
        {
            apiUrl = url.ToString();
        }

        var dto = new OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput>()
        {
            Messages = chatHistory.Select(x => new OpenAIChatCompletionRequestInput()
            {
                Content = x.Content,
                Role = x.Role.ToString()
            }).ToList(),
            Model = settings.ModelId,
            Temperature = settings.Temperature,
            MaxTokens = settings.MaxTokens ?? 500,
            Stream = false,
            FrequencyPenalty = settings.FrequencyPenalty,
            TopP = settings.TopP
        };

        using var response = await openAiOptions.Client.PostAsync(
            apiUrl.TrimEnd('/') + "/v1/chat/completions", dto,
            apiKey);

        var result = await response.Content.ReadFromJsonAsync<OpenAIResultDto>(cancellationToken: cancellationToken);

        return new ChatMessageContent[]
        {
            new(AuthorRole.Assistant, result?.Choices?.FirstOrDefault()?.Message.Content)
        };
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

        var apiKey = string.Empty;
        var apiUrl = string.Empty;

        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
        {
            apiKey = key.ToString();
        }

        if (executionSettings?.ExtensionData?.TryGetValue(Constant.API_URL, out var url) == true)
        {
            apiUrl = url.ToString();
        }

        var dto = new OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput>()
        {
            Messages = chatHistory.Select(x => new OpenAIChatCompletionRequestInput()
            {
                Content = x.Content,
                Role = x.Role.ToString()
            }).ToList(),
            Model = settings.ModelId,
            Temperature = settings.Temperature,
            MaxTokens = settings.MaxTokens ?? 500,
            Stream = true,
            FrequencyPenalty = settings.FrequencyPenalty,
            TopP = settings.TopP
        };

        using var response = await openAiOptions.Client.HttpRequestRaw(
            apiUrl.TrimEnd('/') + "/v1/chat/completions", dto,
            apiKey);

        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith("data:"))
                line = line.Substring("data:".Length);

            line = line.Trim();

            if (line == "[DONE]")
            {
                break;
            }

            if (line.StartsWith(":"))
            {
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                var result = JsonSerializer.Deserialize<OpenAIResultDto>(line, new JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                });
                yield return new StreamingChatMessageContent(AuthorRole.Assistant, result?.Choices[0].Delta?.Content);
            }
        }
    }
}