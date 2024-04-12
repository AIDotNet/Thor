using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace AIDotNet.OpenAI;

public sealed class OpenAiService : IApiChatCompletionService
{
    private static readonly HttpClient HttpClient = new();

    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address
        });

        var result =
            await openAiService.ChatCompletion.CreateCompletion(chatCompletionCreate,
                cancellationToken: cancellationToken);

        return result;
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(
        ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address
        });
        
        await foreach (var item in openAiService.ChatCompletion.CreateCompletionAsStream(chatCompletionCreate,
                           cancellationToken: cancellationToken))
        {
            yield return item;
        }
    }

}