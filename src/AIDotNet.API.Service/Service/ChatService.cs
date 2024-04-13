using System.Text;
using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Exceptions;
using AIDotNet.API.Service.Infrastructure;
using AIDotNet.API.Service.Infrastructure.Helper;
using MapsterMapper;
using OpenAI.ObjectModels.RequestModels;
using TokenApi.Service.Exceptions;

namespace AIDotNet.API.Service.Service;

public sealed class ChatService(
    IServiceProvider serviceProvider,
    ChannelService channelService,
    TokenService tokenService,
    UserService userService,
    IMapper mapper,
    LoggerService loggerService)
    : ApplicationService(serviceProvider)
{
    private const string ConsumerTemplate = "模型倍率：{0} 补全倍率：{1}";


    static readonly Dictionary<string, Dictionary<string, double>> ImageSizeRatios = new()
    {
        {
            "dall-e-2", new Dictionary<string, double>
            {
                { "256x256", 1 },
                { "512x512", 1.125 },
                { "1024x1024", 1.25 }
            }
        },
        {
            "dall-e-3", new Dictionary<string, double>
            {
                { "1024x1024", 1 },
                { "1024x1792", 2 },
                { "1792x1024", 2 }
            }
        },
        {
            "ali-stable-diffusion-xl", new Dictionary<string, double>
            {
                { "512x1024", 1 },
                { "1024x768", 1 },
                { "1024x1024", 1 },
                { "576x1024", 1 },
                { "1024x576", 1 }
            }
        },
        {
            "ali-stable-diffusion-v1.5", new Dictionary<string, double>
            {
                { "512x1024", 1 },
                { "1024x768", 1 },
                { "1024x1024", 1 },
                { "576x1024", 1 },
                { "1024x576", 1 }
            }
        },
        {
            "wanx-v1", new Dictionary<string, double>
            {
                { "1024x1024", 1 },
                { "720x1280", 1 },
                { "1280x720", 1 }
            }
        }
    };

    public async ValueTask ImageAsync(HttpContext context)
    {
        var (token, user) = await tokenService.CheckTokenAsync(context);

        using var body = new MemoryStream();
        await context.Request.Body.CopyToAsync(body);

        var module = JsonSerializer.Deserialize<ImageCreateRequest>(body.ToArray());


        var imageCostRatio = GetImageCostRatio(module);

        var rate = SettingService.PromptRate[module.Model];

        var quota = (int)(rate * imageCostRatio * 1000) * module.N;

        if (module == null)
        {
            throw new Exception("模型校验异常");
        }

        if (quota > user.ResidualCredit)
        {
            throw new InsufficientQuotaException("额度不足");
        }

        // 获取渠道 通过算法计算权重
        var channel = CalculateWeight((await channelService.GetChannelsAsync())
            .Where(x => x.Models.Contains(module.Model)));

        if (channel == null)
        {
            throw new NotModelException(module.Model);
        }


        // 获取渠道指定的实现类型的服务
        var openService = GetKeyedService<IApiImageService>(channel.Type);

        if (openService == null)
        {
            await context.WriteEndAsync("渠道服务不存在");
            return;
        }

        var response = await openService.CreateImage(module, new ChatOptions()
        {
            Key = channel.Key,
            Address = channel.Address,
        }, context.RequestAborted);

        await context.Response.WriteAsJsonAsync(new AIDotNetImageCreateResponse()
        {
            data = response.Results,
            created = response.CreatedAt,
            successful = response.Successful
        });


        await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate, 0), module.Model,
            0, 0, (int)quota, token.Name, user?.UserName, token.Creator, channel.Id,
            channel.Name);

        await userService.ConsumeAsync(user!.Id, (long)quota, 0, token.Key);

        await DbContext.SaveChangesAsync();
    }

    public async ValueTask EmbeddingAsync(HttpContext context)
    {
        var (token, user) = await tokenService.CheckTokenAsync(context);

        using var body = new MemoryStream();
        await context.Request.Body.CopyToAsync(body);

        var module = JsonSerializer.Deserialize<EmbeddingInput>(body.ToArray());

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
        var openService = GetKeyedService<IApiTextEmbeddingGeneration>(channel.Type);

        if (openService == null)
        {
            await context.WriteEndAsync("渠道服务不存在");
            return;
        }

        var embeddingCreateRequest = new EmbeddingCreateRequest()
        {
            Model = module.Model,
            EncodingFormat = module.EncodingFormat,
        };

        int requestToken;
        if (module.Input is JsonElement str)
        {
            if (str.ValueKind == JsonValueKind.String)
            {
                embeddingCreateRequest.Input = str.ToString();
                requestToken = TokenHelper.GetTotalTokens(str.ToString());
            }
            else if (str.ValueKind == JsonValueKind.Array)
            {
                var inputString = str.EnumerateArray().Select(x => x.ToString()).ToArray();
                embeddingCreateRequest.InputAsList = inputString.ToList();
                requestToken = TokenHelper.GetTotalTokens(inputString);
            }
            else
            {
                throw new Exception("输入格式错误");
            }
        }
        else
        {
            throw new Exception("输入格式错误");
        }

        var stream = await openService.EmbeddingAsync(embeddingCreateRequest, new ChatOptions()
        {
            Key = channel.Key,
            Address = channel.Address,
        }, context.RequestAborted);

        if (SettingService.PromptRate.TryGetValue(module.Model, out var rate))
        {
            var quota = requestToken * rate;

            var completionRatio = GetCompletionRatio(module.Model);
            quota += (rate * completionRatio);

            // 将quota 四舍五入
            quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

            await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate, completionRatio), module.Model,
                requestToken, 0, (int)quota, token.Name, user?.UserName, token.Creator, channel.Id,
                channel.Name);

            await userService.ConsumeAsync(user!.Id, (long)quota, requestToken, token.Key);

            await DbContext.SaveChangesAsync();
        }

        await context.Response.WriteAsJsonAsync(stream);
    }

    public async ValueTask CompletionsAsync(HttpContext context)
    {
        var (token, user) = await tokenService.CheckTokenAsync(context);

        using var body = new MemoryStream();
        await context.Request.Body.CopyToAsync(body);

        var module = JsonSerializer.Deserialize<ChatCompletionCreateRequest>(body.ToArray());

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
        var openService = GetKeyedService<IApiChatCompletionService>(channel.Type);

        if (openService == null)
        {
            await context.WriteEndAsync("渠道服务不存在");
            return;
        }

        if (SettingService.PromptRate.TryGetValue(module.Model, out var rate))
        {
            int requestToken;
            int responseToken = 0;

            if (module.Stream == true)
            {
                (requestToken, responseToken) = await StreamHandlerAsync(context, module, channel, openService);
            }
            else
            {
                (requestToken, responseToken) = await ChatHandlerAsync(context, module, channel, openService);
            }

            var quota = requestToken * rate;

            var completionRatio = GetCompletionRatio(module.Model);
            quota += responseToken * (rate * completionRatio);

            // 将quota 四舍五入
            quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

            await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate, completionRatio), module.Model,
                requestToken, responseToken, (int)quota, token.Name, user?.UserName, token.Creator, channel.Id,
                channel.Name);

            await userService.ConsumeAsync(user!.Id, (long)quota, requestToken, token.Key);

            await DbContext.SaveChangesAsync();
        }
    }

    private async ValueTask<(int, int)> ChatHandlerAsync(HttpContext context, ChatCompletionCreateRequest input,
        ChatChannel channel, IApiChatCompletionService openService)
    {
        int requestToken;
        int responseToken = 0;
        var responseMessage = new StringBuilder();

        var setting = new ChatOptions()
        {
            Key = channel.Key,
            Address = channel.Address,
        };

        if (input.Model?.Contains("vision") == true)
        {
            requestToken = TokenHelper.GetTotalTokens(input?.Messages.SelectMany(x => x.Contents)
                .Where(x => x.Type == "text").Select(x => x.Text).ToArray());

            var result = await openService.CompleteChatAsync(input, setting);

            await context.Response.WriteAsJsonAsync(mapper.Map<CompletionCreateResponse>(result));

            responseToken = TokenHelper.GetTokens(result.Choices.FirstOrDefault()?.Delta.Content ?? string.Empty);
        }
        else
        {
            requestToken = TokenHelper.GetTotalTokens(input?.Messages.Select(x => x.Content).ToArray());


            var result = await openService.CompleteChatAsync(input, setting);

            await context.Response.WriteAsJsonAsync(mapper.Map<CompletionCreateResponse>(result));

            responseToken = TokenHelper.GetTokens(result.Choices.FirstOrDefault()?.Delta.Content ?? string.Empty);
        }

        return (requestToken, responseToken);
    }

    /// <summary>
    /// Stream 对话处理
    /// </summary>
    /// <param Name="context"></param>
    /// <param Name="body"></param>
    /// <param Name="module"></param>
    /// <param Name="channel"></param>
    /// <param Name="openService"></param>
    /// <param name="input">输入</param>
    /// <param name="channel">渠道</param>
    /// <returns></returns>
    private static async ValueTask<(int, int)> StreamHandlerAsync(HttpContext context,
        ChatCompletionCreateRequest input, ChatChannel channel, IApiChatCompletionService openService)
    {
        int requestToken;
        int responseToken = 0;

        var setting = new ChatOptions()
        {
            Key = channel.Key,
            Address = channel.Address,
        };

        var id = "chatcmpl-" + StringHelper.GenerateRandomString(29);
        var systemFingerprint = "fp_" + StringHelper.GenerateRandomString(10);
        var responseMessage = new StringBuilder();

        if (input.Model?.Contains("vision") == true)
        {
            requestToken = 0;

            requestToken = TokenHelper.GetTotalTokens(input?.Messages.SelectMany(x => x.Contents)
                .Where(x => x.Type == "text")
                .Select(x => x.Text).ToArray());

            await foreach (var item in openService.StreamChatAsync(input, setting))
            {
                responseMessage.Append(item.Choices.FirstOrDefault()?.Delta.Content ?? string.Empty);
                await context.WriteOpenAiResultAsync(item.Choices.FirstOrDefault()?.Delta.Content ?? string.Empty,
                    input.Model,
                    systemFingerprint, id);
            }

            await context.WriteEndAsync();
        }
        else
        {
            requestToken = TokenHelper.GetTotalTokens(input?.Messages.Select(x => x.Content).ToArray());

            await foreach (var item in openService.StreamChatAsync(input, setting))
            {
                responseMessage.Append(item);
                await context.WriteOpenAiResultAsync(item.Choices.FirstOrDefault()?.Delta.Content ?? string.Empty,
                    input.Model,
                    systemFingerprint, id);
            }

            await context.WriteEndAsync();
        }

        responseToken = TokenHelper.GetTokens(responseMessage.ToString());

        return (requestToken, responseToken);
    }


    /// <summary>
    /// 权重算法
    /// </summary>
    /// <param Name="channel"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static decimal GetCompletionRatio(string name)
    {
        if (SettingService.CompletionRate.TryGetValue(name, out var ratio))
        {
            return ratio;
        }

        if (name.StartsWith("gpt-3.5"))
        {
            if (name.EndsWith("0125"))
            {
                return 3;
            }

            if (name.EndsWith("1106"))
            {
                return 2;
            }

            if (name is "gpt-3.5-turbo" or "gpt-3.5-turbo-16k")
            {
                return 2;
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

    public static decimal GetImageCostRatio(ImageCreateRequest module)
    {
        var imageCostRatio = GetImageSizeRatio(module.Model, module.Size);
        if (module is { Quality: "hd", Model: "dall-e-3" })
        {
            if (module.Size == "1024x1024")
            {
                imageCostRatio *= 2;
            }
            else
            {
                imageCostRatio *= (decimal)1.5;
            }
        }

        return imageCostRatio;
    }

    public static decimal GetImageSizeRatio(string model, string size)
    {
        if (!ImageSizeRatios.TryGetValue(model, out var ratios)) return 1;

        if (ratios.TryGetValue(size, out var ratio))
        {
            return (decimal)ratio;
        }

        return 1;
    }
}