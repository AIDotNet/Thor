using System.Runtime.CompilerServices;
using System.Text.Json;
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


            var messages = await CreateMessage(input.Messages.Where(x => x.Role != "system").ToList(), options);

            var system = CreateSystemContentMessage(input.Messages.Where(x => x.Role == "system").ToList(), options);

            bool isThink = input.Model.EndsWith("-thinking");
            var model = input.Model.Replace("-thinking", string.Empty);

            var request = new ConverseRequest()
            {
                ModelId = model,
                Messages = messages,
            };

            if (input.Tools != null)
            {
                request.ToolConfig = new ToolConfiguration();

                if (input.ToolChoice != null)
                {
                    if (input.ToolChoice.Type == "auto")
                    {
                        request.ToolConfig.ToolChoice = new ToolChoice()
                        {
                            Auto = new AutoToolChoice()
                        };
                    }
                    else if (input.ToolChoice.Type == "required")
                    {
                        request.ToolConfig.ToolChoice = new ToolChoice()
                        {
                            Any = new AnyToolChoice(),
                        };
                    }
                    else if (input.ToolChoice.Type == "function")
                    {
                        request.ToolConfig.ToolChoice = new ToolChoice()
                        {
                            Any = new AnyToolChoice(),
                            Tool = new SpecificToolChoice()
                            {
                                Name = input.ToolChoice.Function?.Name
                            }
                        };
                    }
                }

                foreach (var tool in input.Tools)
                {
                    var properties = new Document();

                    if (tool.Function?.Parameters?.Properties != null)
                    {
                        foreach (var property in tool.Function.Parameters.Properties)
                        {
                            var propertyValue = new Document
                            {
                                { "type", property.Value.Type }
                            };

                            if (!string.IsNullOrEmpty(property.Value.Description))
                            {
                                propertyValue.Add("description", property.Value.Description);
                            }

                            if (property.Value.Enum is { Count: > 0 })
                            {
                                var enums = property.Value.Enum.Select(x => new Document(x));

                                propertyValue.Add("enum", new Document(enums.ToList()));
                            }

                            properties.Add(property.Key, propertyValue);
                        }
                    }

                    var required = new Document();
                    if (tool.Function?.Parameters?.Required != null)
                    {
                        foreach (var item in tool.Function.Parameters.Required)
                        {
                            required.Add(item);
                        }
                    }

                    request.ToolConfig.Tools ??= [];
                    request.ToolConfig.Tools.Add(new Tool()
                    {
                        ToolSpec = new ToolSpecification()
                        {
                            Description = tool.Function?.Description,
                            Name = tool.Function?.Name,
                            InputSchema = new ToolInputSchema()
                            {
                                Json = new Document
                                {
                                    { "type", tool.Function?.Parameters?.Type },
                                    {
                                        "properties", properties
                                    },
                                    { "required", required }
                                }
                            }
                        }
                    });
                }
            }

            var budgetTokens = 1024;
            if (input.MaxTokens is < 2048)
            {
                input.MaxTokens = 2048;
            }

            if (input.MaxTokens != null && input.MaxTokens / 2 < 1024)
            {
                budgetTokens = input.MaxTokens.Value / (4 * 3);
            }

            // budgetTokens最大4096
            budgetTokens = Math.Min(budgetTokens, 4096);

            if (isThink)
            {
                if (input.MaxTokens == null)
                {
                    // 设置200K
                    input.MaxTokens = 64000;
                    budgetTokens = 63999;
                }

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

            if (!isThink && input?.Temperature != null)
            {
                request.InferenceConfig ??= new InferenceConfiguration()
                {
                    Temperature = input.Temperature,
                };
            }

            if (!isThink && input?.TopP != null)
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
            string responseText = response?.output?.message?.content?.Where(x => !string.IsNullOrWhiteSpace(x.Text))
                .Select(x => x.Text)
                .FirstOrDefault() ?? string.Empty;

            var message = ThorChatMessage.CreateAssistantMessage(responseText);

            message.ReasoningContent = response?.output?.message?.content
                ?.FirstOrDefault(x => !string.IsNullOrEmpty(x.ReasoningContent?.reasoningText?.text))?.ReasoningContent
                ?.reasoningText?.text;

            if (response?.output?.message?.content?.Any(x => x.ToolUse != null) == true)
            {
                var tool = response?.output?.message?.content.FirstOrDefault(x => x.ToolUse != null)?.ToolUse;

                message.ToolCallId = tool?.Name;

                message.ToolCalls ??= [];
                message.ToolCalls.Add(new ThorToolCall()
                {
                    Id = tool?.ToolUseId,
                    Function = new ThorChatMessageFunction()
                    {
                        Name = tool?.Name,
                        Arguments = JsonSerializer.Serialize(tool?.Input, ThorJsonSerializer.DefaultOptions)
                    }
                });
            }

            var chatCompletionsResponse = new ThorChatCompletionsResponse()
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

            return chatCompletionsResponse;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async ValueTask<List<Message>> CreateMessage(List<ThorChatMessage> messages,
            ThorPlatformOptions options)
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
                    var tasks = contentCalculated.Select<ThorChatMessageContent, Task<ContentBlock>>(async x =>
                    {
                        if (x.Type == "text" && !string.IsNullOrEmpty(x.Text))
                        {
                            return new ContentBlock
                            {
                                Text = x.Text,
                            };
                        }

                        if (x.ImageUrl?.Url.StartsWith("http") == false)
                        {
                            var base64 = x.ImageUrl?.Url.Split(',')[1];
                            var suffix = x.ImageUrl?.Url.Split(';').First().Split('/').Last();

                            return new ContentBlock
                            {
                                Image = new ImageBlock()
                                {
                                    Format = suffix,
                                    Source = new ImageSource()
                                    {
                                        Bytes = new MemoryStream(
                                            Convert.FromBase64String(base64)),
                                    }
                                }
                            };
                        }
                        else
                        {
                            // 获取后缀名
                            var suffix = x.ImageUrl.Url.Split('.').Last();

                            var response = await HttpClientFactory.GetHttpClient(x.ImageUrl?.Url)
                                .GetAsync(x.ImageUrl?.Url);

                            return new ContentBlock
                            {
                                Image = new ImageBlock()
                                {
                                    Format = suffix,
                                    Source = new ImageSource()
                                    {
                                        Bytes = new MemoryStream(await response.Content.ReadAsByteArrayAsync())
                                    }
                                }
                            };
                        }
                    });
                    await Task.WhenAll(tasks);
                    item.Content.AddRange(tasks.Select(x => x.Result));
                    awsMessage.Add(item);
                }
                else
                {
                    if (string.IsNullOrEmpty(chatMessage.Content))
                    {
                        continue;
                    }

                    var item = new Message
                    {
                        Role = chatMessage.Role
                    };

                    if (chatMessage.Role == "tool")
                    {
                        item.Role = "user";
                        
                        item.Content =
                        [
                            new ContentBlock()
                            {
                                ToolResult = new ToolResultBlock()
                                {
                                    ToolUseId = chatMessage.ToolCallId,
                                    Status = ToolResultStatus.Success,
                                    Content =
                                    [
                                        new ToolResultContentBlock()
                                        {
                                            Text = chatMessage.Content
                                        }
                                    ]
                                }
                            }
                        ];
                    }
                    else
                    {
                        var block = new ContentBlock()
                        {
                            Text = chatMessage.Content,
                        };

                        item.Content =
                        [
                            block
                        ];
                        
                        if(chatMessage.ToolCalls?.Count > 0)
                        {
                            foreach (var toolCall in chatMessage.ToolCalls)
                            {
                                item.Content.Add(new ContentBlock()
                                {
                                    ToolUse = new ToolUseBlock()
                                    {
                                        Name = toolCall.Function.Name,
                                        Input = Document.FromObject(JsonSerializer.Deserialize<object>(toolCall.Function.Arguments)),
                                        ToolUseId = toolCall.Id
                                    },
                                });
                            }
                        }
                    }

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


            var messages = await CreateMessage(input.Messages.Where(x => x.Role != "system").ToList(), options);

            var system = CreateSystemContentMessage(input.Messages.Where(x => x.Role == "system").ToList(), options);

            bool isThink = input.Model.EndsWith("-thinking");
            var model = input.Model.Replace("-thinking", string.Empty);

            var request = new ConverseStreamRequest
            {
                ModelId = model,
                Messages = messages,
            };


            if (input.Tools != null)
            {
                request.ToolConfig = new ToolConfiguration();

                if (input.ToolChoice != null)
                {
                    if (input.ToolChoice.Type == "auto")
                    {
                        request.ToolConfig.ToolChoice = new ToolChoice()
                        {
                            Auto = new AutoToolChoice()
                        };
                    }
                    else if (input.ToolChoice.Type == "required")
                    {
                        request.ToolConfig.ToolChoice = new ToolChoice()
                        {
                            Any = new AnyToolChoice(),
                        };
                    }
                    else if (input.ToolChoice.Type == "function")
                    {
                        request.ToolConfig.ToolChoice = new ToolChoice()
                        {
                            Any = new AnyToolChoice(),
                            Tool = new SpecificToolChoice()
                            {
                                Name = input.ToolChoice.Function?.Name
                            }
                        };
                    }
                }

                foreach (var tool in input.Tools)
                {
                    var properties = new Document();

                    if (tool.Function?.Parameters?.Properties != null)
                    {
                        foreach (var property in tool.Function.Parameters.Properties)
                        {
                            var propertyValue = new Document
                            {
                                { "type", property.Value.Type }
                            };

                            if (!string.IsNullOrEmpty(property.Value.Description))
                            {
                                propertyValue.Add("description", property.Value.Description);
                            }

                            if (property.Value.Enum is { Count: > 0 })
                            {
                                var enums = property.Value.Enum.Select(x => new Document(x));

                                propertyValue.Add("enum", new Document(enums.ToList()));
                            }

                            properties.Add(property.Key, propertyValue);
                        }
                    }

                    var required = new Document();
                    if (tool.Function?.Parameters?.Required != null)
                    {
                        foreach (var item in tool.Function.Parameters.Required)
                        {
                            required.Add(item);
                        }
                    }

                    request.ToolConfig.Tools ??= [];
                    request.ToolConfig.Tools.Add(new Tool()
                    {
                        ToolSpec = new ToolSpecification()
                        {
                            Description = tool.Function?.Description,
                            Name = tool.Function?.Name,
                            InputSchema = new ToolInputSchema()
                            {
                                Json = new Document
                                {
                                    { "type", tool.Function?.Parameters?.Type },
                                    {
                                        "properties", properties
                                    },
                                    { "required", required }
                                }
                            }
                        }
                    });
                }
            }

            var budgetTokens = 1024;
            if (input.MaxTokens is < 2048)
            {
                input.MaxTokens = 2048;
            }

            if (input.MaxTokens != null && input.MaxTokens / 2 < 1024)
            {
                budgetTokens = input.MaxTokens.Value / (4 * 3);
            }

            // budgetTokens最大4096
            budgetTokens = Math.Min(budgetTokens, 4096);

            if (isThink)
            {
                if (input.MaxTokens == null)
                {
                    // 设置200K
                    input.MaxTokens = 131072;
                    budgetTokens = 4096;
                }

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
                    else if (@event.Delta.ToolUse != null)
                    {
                        yield return new ThorChatCompletionsResponse()
                        {
                            Choices =
                            [
                                new ThorChatChoiceResponse
                                {
                                    Delta = new ThorChatMessage()
                                    {
                                        ToolCalls =
                                        [
                                            new ThorToolCall()
                                            {
                                                Function = new ThorChatMessageFunction()
                                                {
                                                    Arguments = @event.Delta.ToolUse.Input,
                                                    Name = @event.Delta.ToolUse.Name,
                                                },
                                                Id = @event.Delta.ToolUse.ToolUseId
                                            }
                                        ]
                                    }
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
                else if (content is ContentBlockStartEvent contentBlock)
                {
                    yield return new ThorChatCompletionsResponse()
                    {
                        Choices =
                        [
                            new ThorChatChoiceResponse
                            {
                                Delta = new ThorChatMessage()
                                {
                                    ToolCalls =
                                    [
                                        new ThorToolCall()
                                        {
                                            Function = new ThorChatMessageFunction()
                                            {
                                                Name = contentBlock.Start.ToolUse.Name
                                            },
                                            Id = contentBlock.Start.ToolUse.ToolUseId
                                        }
                                    ]
                                }
                            }
                        ],
                        Model = input?.Model
                    };
                }
                else if (content is ConverseStreamMetadataEvent metadataEvent)
                {
                    yield return new ThorChatCompletionsResponse()
                    {
                        Choices =
                        [
                            new ThorChatChoiceResponse
                            {
                                Delta = new ThorChatMessage()
                                {
                                    Content = null,
                                },
                                Message = new ThorChatMessage()
                                {
                                    Content = null,
                                },
                                FinishReason = "stop",
                                Index = 0,
                            }
                        ],
                        Usage = new ThorUsageResponse()
                        {
                            PromptTokens = metadataEvent.Usage.InputTokens,
                            CompletionTokens = metadataEvent.Usage.OutputTokens,
                            TotalTokens = metadataEvent.Usage.TotalTokens ?? 0
                        },
                        Model = input?.Model
                    };
                }
            }
        }
    }
}