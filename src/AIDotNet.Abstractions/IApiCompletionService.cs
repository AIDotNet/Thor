using AIDotNet.Abstractions.Dto;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace AIDotNet.Abstractions;

public interface IApiCompletionService
{
    /// <summary>
    /// 给定一个提示，该模型将返回一个或多个预测的完成，并且还可以返回每个位置的替代标记的概率。
    /// </summary>
    /// <param name="createCompletionModel"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CompletionCreateResponse> CompletionAsync(CompletionCreateRequest createCompletionModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default);
}