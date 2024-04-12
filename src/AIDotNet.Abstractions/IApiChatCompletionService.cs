using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace AIDotNet.Abstractions;

public interface IApiChatCompletionService
{
    /// <summary>
    ///  The service Name
    /// </summary>
    public static Dictionary<string, string> ServiceNames { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatCompletionCreate"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ChatCompletionCreateResponse> CompleteChatAsync(
        ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatCompletionCreate"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null, CancellationToken cancellationToken = default);
}