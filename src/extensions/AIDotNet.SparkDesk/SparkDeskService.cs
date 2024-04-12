using System.Runtime.CompilerServices;
using AIDotNet.Abstractions;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using Sdcb.SparkDesk;
using TokenApi.Service.Exceptions;
using ChatMessage = Sdcb.SparkDesk.ChatMessage;

namespace AIDotNet.SparkDesk;

public sealed class SparkDeskService : IApiChatCompletionService
{
    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest input,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        SparkDeskClient client = SparkDeskHelper.GetSparkDeskClient(options!.Key!);
        
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

        if (input.TopP == null)
        {
            input.TopP = 1;
        }
        else
        {
            input.TopP = Convert.ToInt32(Math.Round((double)+1));
        }

        var results = input.Messages.Select(x => new ChatMessage(x.Role.ToString(), x.Content)).ToArray();

        if (input.Temperature <= 0)
        {
            input.Temperature = (float?)0.1;
        }

        var result = await client.ChatAsync(modelVersion,
            results, new ChatRequestParameters
            {
                ChatId = Guid.NewGuid().ToString("N"),
                MaxTokens = (int)input.MaxTokens,
                Temperature = (float)input.Temperature,
                TopK = (int)(input.TopP ?? 1),
            }, cancellationToken: cancellationToken);

        return new ChatCompletionCreateResponse()
        {
            Choices =
            [
                new()
                {
                    Delta = new OpenAI.ObjectModels.RequestModels.ChatMessage("assistant", result.Text),
                    FinishReason = "stop",
                    Index = 0,
                }
            ],
            Model = input.Model,
            Usage = new UsageResponse()
            {
                CompletionTokens = result.Usage.CompletionTokens,
                PromptTokens = result.Usage.PromptTokens,
                TotalTokens = result.Usage.TotalTokens
            }
        };
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(ChatCompletionCreateRequest input,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        SparkDeskClient client = SparkDeskHelper.GetSparkDeskClient(options!.Key!);

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

        if (input.TopP == null)
        {
            input.TopP = 1;
        }
        else
        {
            input.TopP = Convert.ToInt32(Math.Round((double)+1));
        }

        var results = input.Messages.Select(x => new ChatMessage(x.Role.ToString(), x.Content)).ToArray();

        if (input.Temperature <= 0)
        {
            input.Temperature = (float?)0.1;
        }

        await foreach (var result in client.ChatAsStreamAsync(modelVersion,
                           results, new ChatRequestParameters
                           {
                               ChatId = Guid.NewGuid().ToString("N"),
                               MaxTokens = (int)input.MaxTokens,
                               Temperature = (float)input.Temperature,
                               TopK = (int)(input.TopP ?? 1),
                           }, cancellationToken: cancellationToken))
        {
            yield
                return new ChatCompletionCreateResponse()
                {
                    Choices =
                    [
                        new()
                        {
                            Delta = new OpenAI.ObjectModels.RequestModels.ChatMessage("assistant", result.Text),
                            FinishReason = "stop",
                            Index = 0,
                        }
                    ],
                    Model = input.Model
                };
            ;
        }
    }

}