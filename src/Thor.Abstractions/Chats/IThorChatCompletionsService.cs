using Thor.Abstractions.Chats.Dtos;

namespace Thor.Abstractions.Chats;

/// <summary>
/// 对话补全服务
/// </summary>
public interface IThorChatCompletionsService
{
    /// <summary>
    /// 非流式对话补全
    /// </summary>
    /// <param name="request">对话补全请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<ThorChatCompletionsResponse> ChatCompletionsAsync(
        ThorChatCompletionsRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 流式对话补全
    /// </summary>
    /// <param name="request">对话补全请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
        ThorChatCompletionsRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}