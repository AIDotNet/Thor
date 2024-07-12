using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.Abstractions;

/// <summary>
/// 对话补全服务
/// </summary>
public interface IChatCompletionsService
{
    /// <summary>
    /// 非流式对话补全
    /// </summary>
    /// <param name="chatCompletionCreate"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ChatCompletionCreateResponse> ChatCompletionsAsync(
        ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 流式对话补全
    /// </summary>
    /// <param name="chatCompletionCreate"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatCompletionsAsync(ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null, CancellationToken cancellationToken = default);
}