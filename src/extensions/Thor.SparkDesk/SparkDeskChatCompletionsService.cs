using System.Runtime.CompilerServices;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;
using Microsoft.Extensions.Logging;
using Thor.SparkDesk.API;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions;

namespace Thor.SparkDesk;

public sealed class SparkDeskChatCompletionsService(ILogger<SparkDeskChatCompletionsService> logger) : IThorChatCompletionsService
{
    public async Task<ChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest input,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.Model))
        {
            throw new NotModelException(input.Model);
        }

        var client = SparkDeskFactory.GetSparkDeskChatClient(options!.ApiKey!, input.Model, string.IsNullOrWhiteSpace(options?.Address) ? null : options?.Address);

        input.TopP ??= 4;

        var results = input.Messages.Select(x => new XFSparkDeskChatAPIMessageRequest()
        {
            Role = x.Role.ToString(),
            Content = x.Content
        }).ToList();

        if (input.Temperature <= 0)
        {
            input.Temperature = (float?)0.5;
        }

        var functions = input.Tools?.Where(x => x.Type.Equals("function", StringComparison.CurrentCultureIgnoreCase) && x.Function != null)
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
            MaxTokens = input.MaxTokens ?? 2048,
            Temperature = input.Temperature ?? 0.5,
            TopK = (int)(input.TopP ?? 4)
        }, cancellationToken: cancellationToken);

        var retMessage = ThorChatMessage.CreateAssistantMessage(string.Empty);
        var ret = new ChatCompletionsResponse()
        {
            Model = input.Model,
            Choices = new List<ChatChoiceResponse>()
            {
                new()
                {
                    Delta = retMessage,
                    FinishReason = "stop",
                    Index = 0,
                }
            },
            Usage = new UsageResponse()
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


    public async IAsyncEnumerable<ChatCompletionsResponse> StreamChatCompletionsAsync(ThorChatCompletionsRequest input,
        ThorPlatformOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.Model))
            throw new NotModelException(input.Model);

        var client = SparkDeskFactory.GetSparkDeskChatClient(options!.ApiKey!, input.Model, string.IsNullOrWhiteSpace(options?.Address) ? null : options?.Address);

        if (input.TopP == null)
        {
            input.TopP = 4;
        }

        var results = input.Messages.Select(x => new XFSparkDeskChatAPIMessageRequest()
        {
            Role = x.Role.ToString(),
            Content = x.Content
        }).ToList();

        if (input.Temperature <= 0)
        {
            input.Temperature = (float?)0.5;
        }

        var functions = input.Tools?.Where(x => x.Type.Equals("function", StringComparison.CurrentCultureIgnoreCase) && x.Function != null)
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
            MaxTokens = input.MaxTokens ?? 2048,
            Temperature = input.Temperature ?? 0.5,
            TopK = (int)(input.TopP ?? 4)
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
            var ret = new ChatCompletionsResponse()
            {
                Model = input.Model,
                Choices = new List<ChatChoiceResponse>()
                {
                    new ChatChoiceResponse()
                    {
                        Delta = retMessage,
                        FinishReason = "stop",
                        Index = 0,
                    }
                },
                Usage = new UsageResponse()
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