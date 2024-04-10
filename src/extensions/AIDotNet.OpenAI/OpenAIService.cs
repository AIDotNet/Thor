using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using IChatCompletionService = AIDotNet.Abstractions.IChatCompletionService;

namespace AIDotNet.OpenAI;

public sealed class OpenAiService : IChatCompletionService
{
    private static readonly HttpClient HttpClient = new();

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
                Choices =
                [
                    new OpenAIChoiceDto()
                    {
                        Delta = new OpenAIMessageDto()
                        {
                            Content = item.Choices.FirstOrDefault()?.Message.Content,
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
        var str = JsonSerializer.Serialize(input, AIDtoNetJsonSerializer.DefaultOptions);

        var content = new StringContent(str,
            Encoding.UTF8, "application/json");
        var requestMessage =
            new HttpRequestMessage(HttpMethod.Post, options.Address.TrimEnd('/') + "/v1/chat/completions")
            {
                Content = content,
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", options.Key)
                }
            };

        var response = await HttpClient.SendAsync(requestMessage, cancellationToken);


        var result = await response.Content.ReadFromJsonAsync<OpenAIResultDto>(cancellationToken);

        return result;
    }

    public async Task<OpenAIResultDto> ImageCompleteChatAsync(OpenAIChatCompletionInput<OpenAIChatVisionCompletionRequestInput> input, ChatOptions options,
        CancellationToken cancellationToken = default)
    {
        var str = JsonSerializer.Serialize(input, AIDtoNetJsonSerializer.DefaultOptions);

        var content = new StringContent(str,
            Encoding.UTF8, "application/json");
        var requestMessage =
            new HttpRequestMessage(HttpMethod.Post, options.Address.TrimEnd('/') + "/v1/chat/completions")
            {
                Content = content,
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", options.Key)
                }
            };

        var response = await HttpClient.SendAsync(requestMessage, cancellationToken);


        var result = await response.Content.ReadFromJsonAsync<OpenAIResultDto>(cancellationToken);

        return result;
    }

    public async IAsyncEnumerable<OpenAIResultDto> ImageStreamChatAsync(OpenAIChatCompletionInput<OpenAIChatVisionCompletionRequestInput> input, ChatOptions options,
        CancellationToken cancellationToken = default)
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
                Contents = x.content.Select(x => new MessageContent()
                {
                    Text = x.text,
                    Type = x.type,
                    ImageUrl = new VisionImageUrl()
                    {
                        Url = x.ImageUrl.Url,
                        Detail = x.ImageUrl.Detail

                    }
                }).ToArray()
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
                Choices =
                [
                    new OpenAIChoiceDto()
                    {
                        Delta = new OpenAIMessageDto()
                        {
                            Content = item.Choices.FirstOrDefault()?.Message.Content,
                            Role = "assistant"
                        }
                    }
                ]
            };
        }

    }
}