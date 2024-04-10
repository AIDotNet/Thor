using System.Runtime.CompilerServices;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using IChatCompletionService = AIDotNet.Abstractions.IChatCompletionService;

namespace AIDotNet.OpenAI;

public class OpenAiService : IChatCompletionService
{
    public IReadOnlyDictionary<string, object?> Attributes { get; }

    public async Task<OpenAIResultDto> CompleteChatAsync(
        OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address
        });

        var result = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = input.Messages.Select(x => new ChatMessage()
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray(),
            Model = input.Model,
            MaxTokens = input.MaxTokens,
            Temperature = (float?)input.Temperature,
            TopP = (float?)input.TopP,
            FrequencyPenalty = (float?)input.FrequencyPenalty
        }, cancellationToken: cancellationToken);

        return new OpenAIResultDto()
        {
            Model = input.Model,
            Choices = new[]
            {
                new OpenAIChoiceDto()
                {
                    Delta = new OpenAIMessageDto()
                    {
                        Content = result.Choices.FirstOrDefault()?.Message.Content,
                        Role = "assistant"
                    }
                }
            }
        };
    }

    public async IAsyncEnumerable<OpenAIResultDto> StreamChatAsync(
        OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address
        });

        await foreach (var item in openAiService.ChatCompletion.CreateCompletionAsStream(new ChatCompletionCreateRequest
                       {
                           Messages = input.Messages.Select(x => new ChatMessage()
                           {
                               Content = x.Content,
                               Role = x.Role
                           }).ToArray(),
                           Model = input.Model,
                           MaxTokens = input.MaxTokens,
                           Temperature = (float?)input.Temperature,
                           TopP = (float?)input.TopP,
                           FrequencyPenalty = (float?)input.FrequencyPenalty
                       }, cancellationToken: cancellationToken))
        {
            yield return new OpenAIResultDto()
            {
                Model = input.Model,
                Choices = new[]
                {
                    new OpenAIChoiceDto()
                    {
                        Delta = new OpenAIMessageDto()
                        {
                            Content = item.Choices.FirstOrDefault()?.Message.Content,
                            Role = "assistant"
                        }
                    }
                }
            };
        }
    }

    public async Task<OpenAIResultDto> FunctionCompleteChatAsync(
        OpenAIToolsFunctionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address.TrimEnd('/')+"/"
        });


        var tools = input.Tools.Select(x => new ToolDefinition()
        {
            Type = x.Type,
            Function = new FunctionDefinition()
            {
                Name = x.Function.name,
                Description = x.Function.description,
                Parameters = new PropertyDefinition()
                {
                    Type = x.Function.parameters.type,
                    Description = x.Function.parameters.description ?? "测试",
                    Required = x.Function.parameters.required,
                    Properties = x.Function.parameters.properties.ToDictionary(y => y.Key, y => new PropertyDefinition()
                    {
                        Type = y.Value.Type,
                        Description = y.Value.Description,
                    })
                }
            }
        }).ToList();

        var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = input.Messages.Select(x => new ChatMessage()
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray(),
            Model = input.Model,
            MaxTokens = input.MaxTokens,
            Temperature = (float?)input.Temperature,
            TopP = (float?)input.TopP,
            FrequencyPenalty = (float?)input.FrequencyPenalty,
            Tools = tools,
            Stream = false,
            ToolChoice = ToolChoice.FunctionChoice(input.ToolChoice)
        }, cancellationToken: cancellationToken);

        return new OpenAIResultDto()
        {
            Model = input.Model,
            Choices = new[]
            {
                new OpenAIChoiceDto()
                {
                    Delta = new OpenAIMessageDto()
                    {
                        Role = "assistant",
                        ToolCalls = completionResult?.Choices?.FirstOrDefault()?.Message?.ToolCalls?.Select(x =>
                            new OpenAIToolCalls()
                            {
                                id = x.Id,
                                type = x.Type,
                                function = new OpenAIToolFunction()
                                {
                                    name = x.FunctionCall.Name,
                                    arguments = x.FunctionCall.Arguments
                                }
                            }).ToArray()
                    }
                }
            }
        };
    }
}