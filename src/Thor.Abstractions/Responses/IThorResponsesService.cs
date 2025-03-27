using Thor.Abstractions.Responses.Dto;

namespace Thor.Abstractions.Responses;

public interface IThorResponsesService
{
    /// <summary>
    /// 同步获取响应
    /// </summary>
    /// <param name="input"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResponsesDto> GetResponseAsync(ResponsesInput input,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ResponsesDto> GetResponsesAsync(ResponsesInput input,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}