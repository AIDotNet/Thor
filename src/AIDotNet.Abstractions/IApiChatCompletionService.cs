using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace AIDotNet.Abstractions;

public interface IApiChatCompletionService
{
    /// <summary>
    ///  The service Name
    /// </summary>
    public static Dictionary<string, string> ServiceNames { get; } = new();
    
    /// <summary>
    /// 渠道支持的模型列表
    /// </summary>
    public static Dictionary<string,List<string>> ModelNames { get; } = new();

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