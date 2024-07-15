using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.Abstractions.Chats;

/// <summary>
/// 文本补全服务，不支持对话，单次的一问一答，不建议使用，改用IThorChatCompletionsService 对话接口形式最好
/// </summary>
public interface IThorCompletionsService
{
    /// <summary>
    /// 给定一个提示，该模型将返回一个或多个预测的完成，并且还可以返回每个位置的替代标记的概率。
    /// </summary>
    /// <param name="request"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CompletionCreateResponse> CompletionAsync(
        CompletionCreateRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}