using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using TokenApi.Service.Exceptions;
using AuthorRole = Microsoft.SemanticKernel.ChatCompletion.AuthorRole;
using ChatHistory = Microsoft.SemanticKernel.ChatCompletion.ChatHistory;

namespace AIDotNet.API.Service.Service;

public sealed class ChatService(
    IServiceProvider serviceProvider,
    ChannelService channelService,
    UserService userService,
    LoggerService loggerService)
    : ApplicationService(serviceProvider)
{
    private const string ConsumerTemplate = "模型倍率：{0} 补全倍率：{1}";

    private static readonly Dictionary<string, decimal> PromptRate = new();
    private static readonly Dictionary<string, decimal> CompletionRate = new();

    static ChatService()
    {
        if (File.Exists("prompt-rate.json"))
        {
            PromptRate = JsonSerializer.Deserialize<Dictionary<string, decimal>>(File.ReadAllText("prompt-rate.json"));

            CompletionRate = new Dictionary<string, decimal>();
        }
    }

    public async Task CompletionsAsync(HttpContext context)
    {
        #region 校验tokne

        var key = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "").Trim();

        var token = await DbContext.Tokens.AsNoTracking().FirstOrDefaultAsync(x => x.Key == key);

        if (token == null)
        {
            context.Response.StatusCode = 401;
            return;
        }

        // token过期
        if (token.ExpiredTime < DateTimeOffset.Now)
        {
            context.Response.StatusCode = 401;
            return;
        }

        // 余额不足
        if (token is { UnlimitedQuota: false, RemainQuota: < 0 })
        {
            context.Response.StatusCode = 402;
            return;
        }


        var user = await DbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == token.Creator);

        if (user == null)
        {
            context.Response.StatusCode = 401;
            return;
        }

        // 判断额度是否足够
        if (user.ResidualCredit < 10000)
        {
            context.Response.StatusCode = 402;
            return;
        }

        #endregion

        using var body = new MemoryStream();
        await context.Request.Body.CopyToAsync(body);

        var module = JsonSerializer.Deserialize<OpenAICompletionInput>(body.ToArray());

        if (module == null)
        {
            throw new Exception("模型校验异常");
        }

        // 获取渠道 通过算法计算权重
        var channel = CalculateWeight((await channelService.GetChannelsAsync())
            .Where(x => x.Models.Contains(module.Model)));

        if (channel == null)
        {
            throw new NotModelException(module.Model);
        }

        // 获取渠道指定的实现类型的服务
        var openService = GetKeyedService<IADNChatCompletionService>(channel.Type);

        if (openService == null)
        {
            await WriteEndAsync(context, "渠道服务不存在");
            return;
        }

        if (PromptRate.TryGetValue(module.Model, out var rate))
        {
            int requestToken;
            int responseToken = 0;

            var tools =
                JsonSerializer.Deserialize<OpenAIToolsFunctionInput<OpenAIChatCompletionRequestInput>>(body.ToArray());

            if (tools.ToolChoice == "auto")
            {
                var message = JsonSerializer.Deserialize<OpenAIToolsFunctionInput<OpenAIChatCompletionRequestInput>>(
                    body.ToArray());

                requestToken = TokenHelper.GetTotalTokens(message?.Messages.Select(x => x.Content).ToArray());

                var id = "chatcmpl-" + StringHelper.GenerateRandomString(29);
                var systemFingerprint = "fp_" + StringHelper.GenerateRandomString(10);
                var responseMessage = new StringBuilder();

                var chatHistory = new ChatHistory();
                chatHistory.AddRange(message.Messages.Select(x => new ChatMessageContent(
                    new AuthorRole(x.Role), x.Content)));

                var setting = new PromptExecutionSettings
                {
                    ExtensionData = new Dictionary<string, object>(),
                };
                setting.ExtensionData.Add("API_KEY", channel.Key);
                setting.ExtensionData.Add("API_URL", channel.Address);
                setting.ExtensionData.Add("ToolChoice", message.Tools);

                await foreach (var item in openService.GetStreamingChatMessageContentsAsync(chatHistory, setting))
                {
                    responseMessage.Append(item);
                    await WriteOpenAiResultAsync(context, item.Content, module.Model, systemFingerprint, id);
                }

                await WriteEndAsync(context);

                responseToken = TokenHelper.GetTokens(responseMessage.ToString());
            }
            else if (module.Model == "gpt-4-vision-preview")
            {
                requestToken = 0;
            }
            else
            {
                var message = JsonSerializer.Deserialize<OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput>>(
                    body.ToArray());

                requestToken = TokenHelper.GetTotalTokens(message?.Messages.Select(x => x.Content).ToArray());

                var id = "chatcmpl-" + StringHelper.GenerateRandomString(29);
                var systemFingerprint = "fp_" + StringHelper.GenerateRandomString(10);
                var responseMessage = new StringBuilder();

                var chatHistory = new ChatHistory();

                chatHistory.AddRange(message.Messages.Select(x => new ChatMessageContent(
                    new AuthorRole(x.Role), x.Content)));

                var setting = new OpenAIPromptExecutionSettings
                {
                    ExtensionData = new Dictionary<string, object>(),
                    ModelId = module.Model,
                    Temperature = module.Temperature,
                    MaxTokens = module.MaxTokens,
                    FrequencyPenalty = module.FrequencyPenalty,
                    TopP = module.TopP
                };
                setting.ExtensionData.Add("API_KEY", channel.Key);
                setting.ExtensionData.Add("API_URL", channel.Address);

                await foreach (var item in openService.GetStreamingChatMessageContentsAsync(chatHistory, setting))
                {
                    responseMessage.Append(item);
                    await WriteOpenAiResultAsync(context, item.Content, module.Model, systemFingerprint, id);
                }

                await WriteEndAsync(context);

                responseToken = TokenHelper.GetTokens(responseMessage.ToString());
            }

            var quota = requestToken * rate;

            var completionRatio = GetCompletionRatio(module.Model);
            quota += responseToken * (rate * completionRatio);

            // 将quota 四舍五入
            quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);


            await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate, completionRatio), module.Model,
                requestToken, responseToken, (int)quota, token.Name, user?.UserName, token.Creator, channel.Id,
                channel.Name);

            await userService.ConsumeAsync(token.Creator, (long)quota, requestToken);

            await DbContext.SaveChangesAsync();
        }
    }

    private static async ValueTask WriteOpenAiResultAsync(HttpContext context, string content, string model,
        string systemFingerprint, string id)
    {
        var openAiResult = new OpenAIResultDto()
        {
            Id = id,
            _object = "chat.completion.chunk",
            Created = DateTimeOffset.Now.ToUnixTimeSeconds(),
            Model = model,
            SystemFingerprint = systemFingerprint,
            Choices =
            [
                new OpenAIChoiceDto()
                {
                    Index = 0,
                    Delta = new()
                    {
                        Content = content,
                        Role = "assistant"
                    },
                    FinishReason = null
                }
            ]
        };

        await context.Response.WriteAsync("data: " + JsonSerializer.Serialize(openAiResult, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        }) + "\n\n", Encoding.UTF8);
        await context.Response.Body.FlushAsync();
    }

    private static async ValueTask WriteOpenAiResultAsync(HttpContext context, string content)
    {
        var openAiResult = new OpenAIResultDto()
        {
            Id = Guid.NewGuid().ToString("N"),
            _object = "chat.completion.chunk",
            Created = DateTimeOffset.Now.ToUnixTimeSeconds(),
            SystemFingerprint = Guid.NewGuid().ToString("N"),
            Choices =
            [
                new OpenAIChoiceDto()
                {
                    Index = 0,
                    Delta = new()
                    {
                        Content = content,
                        Role = "assistant"
                    },
                    FinishReason = null
                }
            ]
        };

        await context.Response.WriteAsync("data: " + JsonSerializer.Serialize(openAiResult, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        }) + "\n\n", Encoding.UTF8);
        await context.Response.Body.FlushAsync();
    }

    /// <summary>
    /// 权重算法
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    private static ChatChannel CalculateWeight(IEnumerable<ChatChannel> channel)
    {
        // order越大，权重越大，order越小，权重越小，然后随机一个
        var chatChannels = channel as ChatChannel[] ?? channel.ToArray();
        var total = chatChannels.Sum(x => x.Order);

        var random = new Random();

        var value = random.Next(0, total);

        var result = chatChannels.First(x =>
        {
            value -= x.Order;
            return value <= 0;
        });

        return result;
    }

    private static async Task WriteEndAsync(HttpContext context)
    {
        await context.Response.WriteAsync("data: [DONE]\n\n");
        await context.Response.Body.FlushAsync();
    }

    public static async ValueTask WriteEndAsync(HttpContext context, string content)
    {
        await WriteOpenAiResultAsync(context, content);
        await WriteEndAsync(context);
    }

    private static decimal GetCompletionRatio(string name)
    {
        if (CompletionRate.TryGetValue(name, out var ratio))
        {
            return ratio;
        }

        if (name.StartsWith("gpt-3.5"))
        {
            if (name.EndsWith("0125"))
            {
                // https://openai.com/blog/new-embedding-models-and-api-updates
                // Updated GPT-3.5 Turbo model and lower pricing
                return 3;
            }

            if (name.EndsWith("1106"))
            {
                return 2;
            }

            if (name == "gpt-3.5-turbo" || name == "gpt-3.5-turbo-16k")
            {
                // TODO: clear this after 2023-12-11
                DateTime now = DateTime.UtcNow;
                DateTime cutOffDate = new DateTime(2023, 12, 11, 0, 0, 0, DateTimeKind.Utc);

                // 如果当前日期在2023年12月11日之后，返回2
                if (now > cutOffDate)
                {
                    return 2;
                }
            }

            return 1.333333m;
        }

        if (name.StartsWith("gpt-4"))
        {
            if (name.EndsWith("preview"))
            {
                return 3;
            }

            return 2;
        }

        if (name.StartsWith("claude-instant-1"))
        {
            return 3.38m;
        }

        if (name.StartsWith("claude-2"))
        {
            return 2.965517m;
        }

        if (name.StartsWith("mistral-"))
        {
            return 3;
        }

        return 1;
    }
}