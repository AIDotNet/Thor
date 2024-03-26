using AIDotNet.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Sdcb.DashScope;
using Sdcb.DashScope.TextGeneration;

namespace AIDotNet.Qiansail
{
    public class QiansailService : IADNChatCompletionService
    {
        public QiansailService(QiansailOptions qiansailOptions)
        {
            IADNChatCompletionService.ServiceNames.Add("通义千问（阿里云）", QiansailOptions.ServiceName);
        }

        public IReadOnlyDictionary<string, object?> Attributes { get; }

        public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory,
            PromptExecutionSettings? executionSettings = null,
            Kernel? kernel = null, CancellationToken cancellationToken = new CancellationToken())
        {
            if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

            string apiKey = string.Empty;

            if (settings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
            {
                apiKey = key?.ToString();
            }


            using DashScopeClient client = new(apiKey);

            var result = await client.TextGeneration.Chat(settings.ModelId,
                chatHistory.Select(x => new ChatMessage(x.Role.Label, x.Content)).ToArray(), new ChatParameters()
                {
                    MaxTokens = settings.MaxTokens,
                    Temperature = (float?)settings.Temperature,
                    TopP = (float?)settings.TopP,
                    Stop = settings.StopSequences,
                }, cancellationToken);

            return new List<ChatMessageContent>()
            {
                new()
                {
                    Content = result.Output.Text,
                    ModelId = settings.ModelId,
                    Role = AuthorRole.Assistant
                }
            };
        }

        public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
            ChatHistory chatHistory,
            PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
            CancellationToken cancellationToken = new())
        {
            if (executionSettings is not OpenAIPromptExecutionSettings settings) throw new NotImplementedException();

            string apiKey = string.Empty;

            if (settings?.ExtensionData?.TryGetValue(Constant.API_KEY, out var key) == true)
            {
                apiKey = key?.ToString();
            }

            using DashScopeClient client = new(apiKey);

            if (settings.TopP >= 1)
            {
                settings.TopP = 0.9;
            }
            else if (settings.TopP <= 0)
            {
                settings.TopP = 0.1;
            }

            await foreach (var item in client.TextGeneration.ChatStreamed(settings.ModelId,
                               chatHistory.Select(x => new ChatMessage(x.Role.Label, x.Content)).ToArray(),
                               new ChatParameters()
                               {
                                   MaxTokens = settings.MaxTokens,
                                   Temperature = (float?)settings.Temperature,
                                   TopP = (float?)settings.TopP,
                                   Stop = settings.StopSequences,
                               }, cancellationToken))
            {
                Console.WriteLine(item.Output.Text);
                yield return new StreamingChatMessageContent(AuthorRole.Assistant,
                    item.Output.Text);
            }
        }
    }
}