using AIDotNet.Abstractions.Dto;

namespace AIDotNet.Abstractions;

public interface IChatCompletionService
{
    /// <summary>
    ///  The service Name
    /// </summary>
    public static Dictionary<string, string> ServiceNames { get; } = new();

    /// <summary>
    /// 单次对话
    /// </summary>
    /// <param Name="input"></param>
    /// <param Name="options"></param>
    /// <param Name="cancellationToken"></param>
    /// <returns></returns>
    Task<OpenAIResultDto> CompleteChatAsync(OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// stream式对话
    /// </summary>
    /// <param Name="input"></param>
    /// <param Name="options"></param>
    /// <param Name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<OpenAIResultDto> StreamChatAsync(OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input,
        ChatOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Function Complete Chat
    /// </summary>
    /// <param Name="input"></param>
    /// <param Name="options"></param>
    /// <param Name="cancellationToken"></param>
    /// <returns></returns>
    Task<OpenAIResultDto> FunctionCompleteChatAsync(OpenAIToolsFunctionInput<OpenAIChatCompletionRequestInput> input,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 图片AI对话
    /// </summary>
    /// <param name="input"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OpenAIResultDto> ImageCompleteChatAsync(
        OpenAIChatCompletionInput<OpenAIChatVisionCompletionRequestInput> input, ChatOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 图片AI对话
    /// </summary>
    /// <param name="input"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<OpenAIResultDto> ImageStreamChatAsync(
               OpenAIChatCompletionInput<OpenAIChatVisionCompletionRequestInput> input, ChatOptions options,
                      CancellationToken cancellationToken = default);
}