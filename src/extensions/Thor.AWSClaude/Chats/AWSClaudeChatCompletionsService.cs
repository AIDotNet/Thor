using System.Runtime.CompilerServices;
using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.Runtime.Documents;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using ThorChatCompletionsResponse =
    Thor.Abstractions.Chats.Dtos.ThorChatCompletionsResponse;

namespace Thor.AWSClaude.Chats
{
    public sealed class AwsClaudeChatCompletionsService : IThorChatCompletionsService
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

            var client = AwsClaudeFactory.CreateClient(awsAccessKeyId, awsSecretAccessKey, regionEndpoint);


            var messages = CreateMessage(input.Messages.Where(x => x.Role != "system").ToList(), options);

            var system = CreateSystemContentMessage(input.Messages.Where(x => x.Role == "system").ToList(), options);

            bool isThink = input.Model.EndsWith("-thinking");
            var model = input.Model.Replace("-thinking", string.Empty);

            var request = new ConverseRequest()
            {
                ModelId = model,
                Messages = messages,
            };


            var budgetTokens = 1024;
            if (input.MaxTokens is null or < 2048)
            {
                input.MaxTokens = 2048;
            }

            if (input.MaxTokens / 2 < 1024)
            {
                budgetTokens = input.MaxTokens.Value / (4 * 3);
            }

            // budgetTokens最大4096
            budgetTokens = Math.Min(budgetTokens, 4096);

            if (isThink)
            {
                request.AdditionalModelRequestFields = new Document
                {
                    {
                        "reasoning_config",
                        new Document
                        {
                            { "type", "enabled" },
                            { "budget_tokens", budgetTokens }
                        }
                    },
                };
            }

            if (input?.MaxTokens != null)
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    MaxTokens = input.MaxTokens,
                };
            }

            if (input?.Temperature != null)
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    Temperature = input.Temperature,
                };
            }

            if (input?.TopP != null)
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    TopP = input.TopP,
                };
            }

            if (!string.IsNullOrWhiteSpace(input?.Stop))
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    StopSequences = [..new[] { input.Stop }]
                };
            }

            if (system.Count != 0)
            {
                request.System = [];
                request.System.AddRange(system);
            }

            var response = await client.ChatAsync(request, cancellationToken);
            string responseText = response?.output?.message?.content?.Where(x => !string.IsNullOrWhiteSpace(x.text)).Select(x => x.text)
                .FirstOrDefault();

            var message = ThorChatMessage.CreateAssistantMessage(responseText);

            message.ReasoningContent = response?.output?.message?.content
                ?.FirstOrDefault(x => !string.IsNullOrEmpty(x.reasoningContent.reasoningText.text))?.reasoningContent
                .reasoningText.text;

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
                Usage = new ThorUsageResponse
                {
                    PromptTokens = response?.usage?.inputTokens ?? 0,
                    CompletionTokens = response?.usage?.outputTokens ?? 0,
                    TotalTokens = response?.usage?.totalTokens ?? 0,
                    PromptTokensDetails = new ThorUsageResponsePromptTokensDetails()
                    {
                        CachedTokens = response?.usage?.cacheReadInputTokenCount,
                    }
                },
                Model = model
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<Message> CreateMessage(List<ThorChatMessage> messages, ThorPlatformOptions options)
        {
            var awsMessage = new List<Message>();

            foreach (var chatMessage in messages)
            {
                if (chatMessage.ContentCalculated is IList<ThorChatMessageContent> contentCalculated)
                {
                    var item = new Message
                    {
                        Role = chatMessage.Role,
                        Content = []
                    };
                    item.Content.AddRange(contentCalculated.Select<ThorChatMessageContent, ContentBlock>(
                        x =>
                        {
                            if (x.Type == "text")
                            {
                                return new ContentBlock
                                {
                                    Text = x.Text,
                                };
                            }

                            return new ContentBlock
                            {
                                Image = new ImageBlock()
                                {
                                    Format = ImageFormat.Png,
                                    Source = new ImageSource()
                                    {
                                        Bytes = new MemoryStream(Convert.FromBase64String(x.ImageUrl?.Url)),
                                    }
                                }
                            };
                        }));
                    awsMessage.Add(item);
                }
                else
                {
                    var item = new Message();

                    item.Role = chatMessage.Role;

                    item.Content =
                    [
                        new ContentBlock()
                        {
                            Text = chatMessage.Content,
                        }
                    ];
                    awsMessage.Add(item);
                }
            }

            return awsMessage;
        }

        private List<SystemContentBlock> CreateSystemContentMessage(List<ThorChatMessage> messages,
            ThorPlatformOptions options)
        {
            return messages.Select(chatMessage => new SystemContentBlock { Text = chatMessage.Content }).ToList();
        }

        /// <summary>
        /// 流式对话补全
        /// </summary>
        /// <param name="input">对话补全请求参数对象</param>
        /// <param name="options">平台参数对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
            ThorChatCompletionsRequest input,
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

            var client = AwsClaudeFactory.CreateClient(awsAccessKeyId, awsSecretAccessKey, regionEndpoint);


            var messages = CreateMessage(input.Messages.Where(x => x.Role != "system").ToList(), options);

            var system = CreateSystemContentMessage(input.Messages.Where(x => x.Role == "system").ToList(), options);

            bool isThink = input.Model.EndsWith("-thinking");
            var model = input.Model.Replace("-thinking", string.Empty);

            var request = new ConverseStreamRequest
            {
                ModelId = model,
                Messages = messages,
            };


            var budgetTokens = 1024;
            if (input.MaxTokens is null or < 2048)
            {
                input.MaxTokens = 2048;
            }

            if (input.MaxTokens / 2 < 1024)
            {
                budgetTokens = input.MaxTokens.Value / (4 * 3);
            }

            // budgetTokens最大4096
            budgetTokens = Math.Min(budgetTokens, 4096);

            if (isThink)
            {
                request.AdditionalModelRequestFields = new Document
                {
                    {
                        "reasoning_config",
                        new Document
                        {
                            { "type", "enabled" },
                            { "budget_tokens", budgetTokens }
                        }
                    },
                };
            }

            if (input?.MaxTokens != null)
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    MaxTokens = input.MaxTokens,
                };
            }

            if (input?.Temperature != null)
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    Temperature = input.Temperature,
                };
            }

            if (input?.TopP != null)
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    TopP = input.TopP,
                };
            }

            if (!string.IsNullOrWhiteSpace(input?.Stop))
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    StopSequences = [..new[] { input.Stop }]
                };
            }

            if (system.Count != 0)
            {
                request.System = [];
                request.System.AddRange(system);
            }

            var result = await client.ChatStreamAsync(request, cancellationToken);

            foreach (var content in result.Stream.AsEnumerable())
            {
                if (content is ReasoningContentBlockDeltaEvent @event)
                {
                    if (@event.Delta.IsSetText())
                    {
                        yield return new ThorChatCompletionsResponse()
                        {
                            Choices =
                            [
                                new ThorChatChoiceResponse
                                {
                                    Delta = ThorChatMessage.CreateAssistantMessage(@event.Delta
                                        .Text),
                                    Message = ThorChatMessage.CreateAssistantMessage(@event.Delta
                                        .Text),
                                    FinishReason = "stop",
                                    Index = 0,
                                }
                            ],
                            Model = input?.Model
                        };
                    }
                    else
                    {
                        yield return new ThorChatCompletionsResponse()
                        {
                            Choices =
                            [
                                new ThorChatChoiceResponse
                                {
                                    Delta = new ThorChatMessage()
                                    {
                                        ReasoningContent = @event.Delta.ReasoningContent?.Text
                                    },
                                    Message = new ThorChatMessage()
                                    {
                                        ReasoningContent = @event.Delta.ReasoningContent?.Text
                                    },
                                    FinishReason = "stop",
                                    Index = 0,
                                }
                            ],
                            Model = input?.Model
                        };
                    }
                }
                else if (content is MessageStartEvent eventStreamEvent)
                {
                }
            }
        }
    }
}