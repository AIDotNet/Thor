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
    /// <param name="request">请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<ChatCompletionsResponse> ChatCompletionsAsync(
        ChatCompletionsRequest request,
        ChatPlatformOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 流式对话补全
    /// </summary>
    /// <param name="request">请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    IAsyncEnumerable<ChatCompletionsResponse> StreamChatCompletionsAsync(
        ChatCompletionsRequest request,
        ChatPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}