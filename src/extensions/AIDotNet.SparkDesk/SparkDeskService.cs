using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using Sdcb.SparkDesk;
using TokenApi.Service.Exceptions;
using IChatCompletionService = AIDotNet.Abstractions.IChatCompletionService;

namespace AIDotNet.SparkDesk;

public class SparkDeskService : IChatCompletionService
{

    public IReadOnlyDictionary<string, object?> Attributes { get; set; }

    public async Task<OpenAIResultDto> CompleteChatAsync(
        OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        SparkDeskClient client;

        // appId|appKey|appSecret
        var parts = options.Key.ToString().Split('|');
        if (parts.Length == 3)
        {
            client = new SparkDeskClient(parts[0], parts[1], parts[2]);
        }
        else
        {
            throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
        }

        ModelVersion modelVersion;
        if (input?.Model == "SparkDesk-v3.5")
        {
            modelVersion = ModelVersion.V3_5;
        }
        else if (input?.Model == "SparkDesk-v3.1")
        {
            modelVersion = ModelVersion.V3;
        }
        else if (input?.Model == "SparkDesk-v1.5")
        {
            modelVersion = ModelVersion.V1_5;
        }
        else if (input?.Model == "SparkDesk-v2.1")
        {
            modelVersion = ModelVersion.V2;
        }
        else
        {
            throw new NotModelException(input?.Model);
        }

        var topK = Convert.ToInt32(Math.Round(input.TopP + 1));

        var results = input.Messages.Select(x => new ChatMessage(x.Role.ToString(), x.Content)).ToArray();

        var msg = await client.ChatAsync(modelVersion,
            results, new ChatRequestParameters
            {
                ChatId = Guid.NewGuid().ToString("N"),
                MaxTokens = (int)input.MaxTokens,
                Temperature = (float)input.Temperature,
                TopK = topK,
            }, cancellationToken: cancellationToken);

        var openAIResultDto = new OpenAIResultDto()
        {
            Model = input.Model,
            Choices = new[]
            {
                new OpenAIChoiceDto()
                {
                    Delta = new OpenAIMessageDto()
                    {
                        Content = msg.Text,
                        Role = "assistant"
                    }
                }
            }
        };


        return openAIResultDto;
    }

    public async IAsyncEnumerable<OpenAIResultDto> StreamChatAsync(
        OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        SparkDeskClient client;

        // appId|appKey|appSecret
        var parts = options.Key.ToString().Split('|');
        if (parts.Length == 3)
        {
            client = new SparkDeskClient(parts[0], parts[1], parts[2]);
        }
        else
        {
            throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
        }

        ModelVersion modelVersion;
        if (input?.Model == "SparkDesk-v3.5")
        {
            modelVersion = ModelVersion.V3_5;
        }
        else if (input?.Model == "SparkDesk-v3.1")
        {
            modelVersion = ModelVersion.V3;
        }
        else if (input?.Model == "SparkDesk-v1.5")
        {
            modelVersion = ModelVersion.V1_5;
        }
        else if (input?.Model == "SparkDesk-v2.1")
        {
            modelVersion = ModelVersion.V2;
        }
        else
        {
            throw new NotModelException(input?.Model);
        }

        var topK = Convert.ToInt32(Math.Round(input.TopP + 1));

        var results = input.Messages.Select(x => new ChatMessage(x.Role.ToString(), x.Content)).ToArray();

        if (input.Temperature <= 0)
        {
            input.Temperature = 0.1;
        }

        var msg = client.ChatAsStreamAsync(modelVersion,
            results, new ChatRequestParameters
            {
                ChatId = Guid.NewGuid().ToString("N"),
                MaxTokens = (int)input.MaxTokens,
                Temperature = (float)input.Temperature,
                TopK = topK,
            }, cancellationToken: cancellationToken);

        await foreach (var item in msg)
        {
            yield return new OpenAIResultDto()
            {
                Model = input.Model,
                Choices =
                [
                    new OpenAIChoiceDto()
                    {
                        Delta = new OpenAIMessageDto()
                        {
                            Content = item.Text,
                            Role = "assistant"
                        }
                    }
                ]
            };
        }
    }

    public async Task<OpenAIResultDto> FunctionCompleteChatAsync(
        OpenAIToolsFunctionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        SparkDeskClient client;

        // appId|appKey|appSecret
        var parts = options.Key.ToString().Split('|');
        if (parts.Length == 3)
        {
            client = new SparkDeskClient(parts[0], parts[1], parts[2]);
        }
        else
        {
            throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
        }

        ModelVersion modelVersion;
        if (input?.Model == "SparkDesk-v3.5")
        {
            modelVersion = ModelVersion.V3_5;
        }
        else if (input?.Model == "SparkDesk-v3.1")
        {
            modelVersion = ModelVersion.V3;
        }
        else if (input?.Model == "SparkDesk-v1.5")
        {
            modelVersion = ModelVersion.V1_5;
        }
        else if (input?.Model == "SparkDesk-v2.1")
        {
            modelVersion = ModelVersion.V2;
        }
        else
        {
            throw new NotModelException(input?.Model);
        }

        var topK = Convert.ToInt32(Math.Round(input.TopP + 1));

        var results = input.Messages.Select(x => new ChatMessage(x.Role.ToString(), x.Content)).ToArray();

        if (input.Temperature <= 0)
        {
            input.Temperature = 0.1;
        }

        var function = input.Tools.Select(x =>
        {
            return new FunctionDef(x.Function.name, x.Function.description,
                x.Function.parameters.properties.Select(property => new FunctionParametersDef(property.Key,
                    property.Value.Type, property.Value.Description,
                    x.Function.parameters.required.Contains(property.Key))).ToArray());
        }).ToArray();

        var msg = await client.ChatAsync(modelVersion,
            results, new ChatRequestParameters
            {
                ChatId = Guid.NewGuid().ToString("N"),
                MaxTokens = (int)input.MaxTokens,
                Temperature = (float)input.Temperature,
                TopK = topK,
            }, functions: function, cancellationToken: cancellationToken);

        var openAIResultDto = new OpenAIResultDto()
        {
            Model = input.Model,
            Choices = new[]
            {
                new OpenAIChoiceDto()
                {
                    Delta = new OpenAIMessageDto()
                    {
                        Content = msg.Text,
                        ToolCalls =
                        [
                            new()
                            {
                                id = Guid.NewGuid().ToString("N"),
                                type = "function",
                                function = new OpenAIToolFunction()
                                {
                                    name = msg.FunctionCall?.Name,
                                    arguments = msg.FunctionCall.Arguments
                                }
                            }
                        ],
                        Role = "assistant"
                    },
                    FinishReason = "tool_calls"
                }
            }
        };

        return openAIResultDto;
    }
}