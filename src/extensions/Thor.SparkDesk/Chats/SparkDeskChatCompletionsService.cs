using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.SparkDesk.API;

namespace Thor.SparkDesk.Chats;

/// <summary>
/// 讯飞星火对话补全服务
/// </summary>
/// <param name="logger"></param>
public sealed class SparkDeskChatCompletionsService(ILogger<SparkDeskChatCompletionsService> logger) : IThorChatCompletionsService
{
    /// <summary>
    /// 非流式对话补全
    /// </summary>
    /// <param name="request">对话补全请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Model))
        {
            throw new NotModelException(request.Model);
        }

        var client = SparkDeskFactory.GetSparkDeskChatClient(options!.ApiKey!, request.Model, null);

        request.TopP ??= 4;

        var results = request.Messages.Select(x => new XFSparkDeskChatAPIMessageRequest()
        {
            Role = x.Role.ToString(),
            Content = x.Content
        }).ToList();

        if (request.Temperature <= 0)
        {
            request.Temperature = (float?)0.5;
        }

        var functions = request.Tools?.Where(x => x.Type.Equals("function", StringComparison.CurrentCultureIgnoreCase) && x.Function != null)
            .Select(x => new XFSparkDeskChatAPIFunctionRequest()
            {
                Name = x.Function!.Name,
                Description = x.Function.Description ?? "",
                Required = x.Function.Parameters.Required?.ToList() ?? [],
                Parameters = new XFSparkDeskChatAPIFunctionParametersRequest()
                {
                    Type = x.Function.Parameters.Type,
                    Properties = x.Function.Parameters.Properties?.ToDictionary(x2 => x2.Key, x2 => new XFSparkDeskChatAPIFunctionParametersPropertieRequest()
                    {
                        Type = x2.Value.Type,
                        Description = x2.Value.Description ?? ""
                    }) ?? []
                }
            }).ToList();

        var result = client.SendChat(new XFSparkDeskChatAPIRequest()
        {
            Messages = results,
            Functions = functions,
            MaxTokens = request.MaxTokens ?? 2048,
            Temperature = request.Temperature ?? 0.5,
            TopK = (int)(4)
        }, cancellationToken: cancellationToken);

        var retMessage = ThorChatMessage.CreateAssistantMessage(string.Empty);
        var ret = new ThorChatCompletionsResponse()
        {
            Model = request.Model,
            Choices = new List<ThorChatChoiceResponse>()
            {
                new()
                {
                    Delta = retMessage,
                    FinishReason = "stop",
                    Index = 0,
                }
            },
            Usage = new ThorUsageResponse()
        };

        await foreach (var chatMsg in result)
        {
            ret.Usage.CompletionTokens += chatMsg?.Payload?.Usage?.Text.CompletionTokens;
            ret.Usage.PromptTokens += chatMsg?.Payload?.Usage?.Text.PromptTokens ?? 0;
            ret.Usage.TotalTokens += chatMsg?.Payload?.Usage?.Text.TotalTokens ?? 0;

            var retContent = chatMsg?.Payload?.Choices?.Text.FirstOrDefault();
            if (retContent == null)
            {
                logger.LogInformation("AddHandleMsg(Chat): retContent is null");
                continue;
            }

            if (!string.IsNullOrEmpty(retContent.Content))
            {
                logger.LogInformation($"AddHandleMsg(Chat): {retContent.Content}");
                retMessage.Content += retContent.Content;
                continue;
            }

            if (retContent.FunctionCall != null)
            {
                logger.LogInformation($"AddHandleMsg(Chat): Function Call, {retContent.FunctionCall.Name}, {retContent.FunctionCall.Arguments}");
                retMessage.ToolCalls = [
                                new ThorToolCall() {
                                    Id=retContent.FunctionCall.Name,
                                    Index=0,
                                    Type="function",
                                    Function=new ThorChatMessageFunction(){
                                        Name=retContent.FunctionCall.Name,
                                        Arguments=retContent.FunctionCall.Arguments
                                    }
                                }
                            ];
                retMessage.FunctionCall = new()
                {
                    Name = retContent.FunctionCall.Name,
                    Arguments = retContent.FunctionCall.Arguments
                };
                ret.Choices.First().FinishReason = "tool_calls";
                continue;
            }
        }

        return ret;
    }

    /// <summary>
    /// 流式对话补全
    /// </summary>
    /// <param name="request">对话补全请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(ThorChatCompletionsRequest request,
        ThorPlatformOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Model))
            throw new NotModelException(request.Model);

        var client = SparkDeskFactory.GetSparkDeskChatClient(options!.ApiKey!, request.Model, string.IsNullOrWhiteSpace(options?.Address) ? null : options?.Address);

        if (request.TopP == null)
        {
            request.TopP = 4;
        }

        var results = request.Messages.Select(x => new XFSparkDeskChatAPIMessageRequest()
        {
            Role = x.Role.ToString(),
            Content = x.Content
        }).ToList();

        if (request.Temperature <= 0)
        {
            request.Temperature = (float?)0.5;
        }

        var functions = request.Tools?.Where(x => x.Type.Equals("function", StringComparison.CurrentCultureIgnoreCase) && x.Function != null)
            .Select(x => new XFSparkDeskChatAPIFunctionRequest()
            {
                Name = x.Function!.Name,
                Description = x.Function.Description ?? "",
                Required = x.Function.Parameters.Required?.ToList() ?? [],
                Parameters = new XFSparkDeskChatAPIFunctionParametersRequest()
                {
                    Type = x.Function.Parameters.Type,
                    Properties = x.Function.Parameters.Properties?.ToDictionary(x2 => x2.Key, x2 => new XFSparkDeskChatAPIFunctionParametersPropertieRequest()
                    {
                        Type = x2.Value.Type,
                        Description = x2.Value.Description ?? ""
                    }) ?? []
                }
            }).ToList();

        var result = client.SendChat(new XFSparkDeskChatAPIRequest()
        {
            Messages = results,
            Functions = functions,
            MaxTokens = request.MaxTokens ?? 2048,
            Temperature = request.Temperature ?? 0.5,
            TopK = (int)(request.TopP ?? 4)
        }, cancellationToken: cancellationToken);

        await foreach (var chatMsg in result)
        {
            var retContent = chatMsg?.Payload?.Choices?.Text.FirstOrDefault();
            if (retContent == null)
            {
                logger.LogInformation("AddHandleMsg(Chat): retContent is null");
                yield break;
            }

            var retMessage = ThorChatMessage.CreateAssistantMessage(string.Empty);
            var ret = new ThorChatCompletionsResponse()
            {
                Model = request.Model,
                Choices = new List<ThorChatChoiceResponse>()
                {
                    new ThorChatChoiceResponse()
                    {
                        Delta = retMessage,
                        FinishReason = "stop",
                        Index = 0,
                    }
                },
                Usage = new ThorUsageResponse()
                {
                    CompletionTokens = chatMsg?.Payload?.Usage?.Text.CompletionTokens,
                    PromptTokens = chatMsg?.Payload?.Usage?.Text.PromptTokens ?? 0,
                    TotalTokens = chatMsg?.Payload?.Usage?.Text.TotalTokens ?? 0
                }
            };


            if (!string.IsNullOrEmpty(retContent.Content))
            {
                logger.LogInformation($"AddHandleMsg(Chat): {retContent.Content}");
                retMessage.Content = retContent.Content;
                yield return ret;
            }

            if (retContent.FunctionCall != null)
            {
                logger.LogInformation($"AddHandleMsg(Chat): Function Call, {retContent.FunctionCall.Name}, {retContent.FunctionCall.Arguments}");
                retMessage.ToolCalls = [
                                new ThorToolCall() {
                                    Id=retContent.FunctionCall.Name,
                                    Index=0,
                                    Type="function",
                                    Function=new(){
                                        Name=retContent.FunctionCall.Name,
                                        Arguments=retContent.FunctionCall.Arguments
                                    }
                                }
                            ];
                retMessage.FunctionCall = new()
                {
                    Name = retContent.FunctionCall.Name,
                    Arguments = retContent.FunctionCall.Arguments
                };
                ret.Choices.First().FinishReason = "tool_calls";
                yield return ret;
            }
        }
    }

}