using Amazon;
using Amazon.BedrockRuntime.Model;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using ThorChatCompletionsResponse =
    Thor.Abstractions.Chats.Dtos.ThorChatCompletionsResponse;
using Thor.Claudia;
using Amazon.Util.Internal;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Thor.AWSClaude.Chats
{
    public sealed class AWSClaudeChatCompletionsService : IThorChatCompletionsService
    {
        /// <summary>
        /// 非流式对话补全
        /// </summary>
        /// <param name="input">对话补全请求参数对象</param>
        /// <param name="options">平台参数对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest input,
            ThorPlatformOptions? options = null,
            CancellationToken cancellationToken = default)
        {

            if (options != null && string.IsNullOrWhiteSpace(options.Other))
                throw new Exception("Other is Required.place select regionEndpoint");
            var regionEndpoint = RegionEndpoint.GetBySystemName(options.Other);
            if (regionEndpoint == null)
                throw new Exception($"regionEndpoint {options.Other} not found");

            var keys = options!.ApiKey!.Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (keys.Length != 2)
                throw new Exception("Key is invalid format, expected awsAccessKeyId|awsSecretAccessKey");

            var awsAccessKeyId = keys[0];
            var awsSecretAccessKey = keys[1];

            var client = AWSClaudeFactory.CreateClient(awsAccessKeyId, awsSecretAccessKey, regionEndpoint);


            var messages = input.Messages.Where(x => x.Role != "system").Select(x => new Message
            {
                Role = x.Role,
                Content = new List<ContentBlock> { new ContentBlock { Text = x.Content } }
            }).ToList();
            var system = input.Messages.FirstOrDefault(x => x.Role == "system")?.Content ?? "";

            var request = new ConverseRequest
            {
                ModelId = input.Model,
                Messages = messages,
                InferenceConfig = new InferenceConfiguration()
                {
                    MaxTokens = input.MaxTokens ?? 2000,
                    Temperature = input.Temperature ?? 0,
                    TopP = input.TopP ?? 0,
                }
            };
            if (!string.IsNullOrEmpty(system))
            {
                request.System = new List<SystemContentBlock>
                {
                     new SystemContentBlock
                     {
                         Text=system,
                     }
                }; ;
            }
            
            var response = await client.ConverseAsync(request, cancellationToken);
            string responseText = response?.Output?.Message?.Content?[0]?.Text ?? "";

            var message = ThorChatMessage.CreateAssistantMessage(responseText);
            return new ThorChatCompletionsResponse()
            {
                Choices =
                 [
                        new()
                        {
                            Delta = message,
                            Message = message,
                            FinishReason = "stop",
                            Index = 0,

                        }
                 ],
                Usage = new Abstractions.Dtos.ThorUsageResponse
                {
                    PromptTokens = response?.Usage?.InputTokens ?? 0,
                    CompletionTokens = response?.Usage?.OutputTokens ?? 0,
                    TotalTokens = response?.Usage?.TotalTokens ?? 0,
                },
                Model = input.Model
            };
        }

        /// <summary>
        /// 流式对话补全
        /// </summary>
        /// <param name="request">对话补全请求参数对象</param>
        /// <param name="options">平台参数对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(ThorChatCompletionsRequest input,
            ThorPlatformOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (options != null && string.IsNullOrWhiteSpace(options.Other))
                throw new Exception("Other is Required.place select regionEndpoint");
            var regionEndpoint = RegionEndpoint.GetBySystemName(options.Other);
            if (regionEndpoint == null)
                throw new Exception($"regionEndpoint {options.Other} not found");

            var keys = options!.ApiKey!.Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (keys.Length != 2)
                throw new Exception("Key is invalid format, expected awsAccessKeyId|awsSecretAccessKey");

            var awsAccessKeyId = keys[0];
            var awsSecretAccessKey = keys[1];

            var client = AWSClaudeFactory.CreateClient(awsAccessKeyId, awsSecretAccessKey, regionEndpoint);


            var messages = input.Messages.Where(x => x.Role != "system").Select(x => new Message
            {
                Role = x.Role,
                Content = new List<ContentBlock> { new ContentBlock { Text = x.Content } }
            }).ToList();
            var system = input.Messages.Where(x => x.Role == "system").FirstOrDefault()?.Content ?? "";

            var request = new ConverseStreamRequest
            {
                ModelId = input.Model,
                Messages = messages,
                InferenceConfig = new InferenceConfiguration()
                {
                    MaxTokens = input.MaxTokens ?? 2000,
                    Temperature = input.Temperature ?? 0,
                    TopP = input.TopP ?? 0,
                }
            };
            if (!string.IsNullOrEmpty(system))
            {
                request.System = new List<SystemContentBlock>
                {
                     new SystemContentBlock
                     {
                         Text=system,
                     }
                }; ;
            }
            var result = await client.ConverseStreamAsync(request, cancellationToken);
            foreach (var content in result.Stream.AsEnumerable())
            {
                if (content is ContentBlockDeltaEvent)
                {
                    yield return new ThorChatCompletionsResponse()
                    {
                        Choices =
                                [
                                    new()
                                    {
                                        Delta = ThorChatMessage.CreateAssistantMessage((content as ContentBlockDeltaEvent).Delta.Text),
                                        FinishReason = "stop",
                                        Index = 0,
                                    }
                                ],
                        Model = input.Model
                    };
                }
            }
        }
    }
}
