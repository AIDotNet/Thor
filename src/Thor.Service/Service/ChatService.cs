using System.Buffers;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Thor.Abstractions.Audios;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Embeddings.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Images;
using Thor.Abstractions.Images.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.Realtime;
using Thor.Abstractions.Realtime.Dto;
using Thor.Infrastructure;
using Thor.Service.Domain.Core;
using Thor.Service.Extensions;

namespace Thor.Service.Service;

/// <summary>
/// 对话服务
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="channelService"></param>
/// <param name="tokenService"></param>
/// <param name="imageService"></param>
/// <param name="rateLimitModelService"></param>
/// <param name="userService"></param>
/// <param name="loggerService"></param>
public sealed class ChatService(
    IServiceProvider serviceProvider,
    ChannelService channelService,
    TokenService tokenService,
    ImageService imageService,
    RateLimitModelService rateLimitModelService,
    UserService userService,
    ILogger<ChatService> logger,
    LoggerService loggerService)
    : ApplicationService(serviceProvider), IScopeDependency
{
    /// <summary>
    ///  按量计费模型倍率模板
    /// </summary>
    private const string ConsumerTemplate = "模型倍率：{0} 补全倍率：{1}";

    /// <summary>
    /// 按次计费模型倍率模板
    /// </summary>
    private const string ConsumerTemplateOnDemand = "按次数计费费用：{0}";

    /// <summary>
    /// 实时对话计费模型倍率模板
    /// </summary>
    private const string RealtimeConsumerTemplate = "模型倍率：文本提示词倍率:{0} 文本完成倍率:{1} 音频请求倍率:{2} 音频完成倍率:{3}  实时对话";


    private static readonly Dictionary<string, Dictionary<string, double>> ImageSizeRatios = new()
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


    public async Task CreateImageAsync(HttpContext context, ImageCreateRequest request)
    {
        try
        {
            using var image =
                Activity.Current?.Source.StartActivity("文字生成图片");

            var organizationId = string.Empty;
            if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
            {
                organizationId = organizationIdHeader.ToString();
            }


            if (string.IsNullOrEmpty(request?.Model)) request.Model = "dall-e-2";

            var imageCostRatio = GetImageCostRatio(request);

            var rate = ModelManagerService.PromptRate[request.Model];


            var (token, user) = await tokenService.CheckTokenAsync(context, rate);

            await rateLimitModelService.CheckAsync(request.Model, user.Id);

            request.Model = TokenService.ModelMap(request.Model);

            TokenService.CheckModel(request.Model, token, context);


            request.N ??= 1;

            var quota = (int)(rate.PromptRate * imageCostRatio * 1000) * request.N;

            if (request == null) throw new Exception("模型校验异常");

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            // 获取渠道 通过算法计算权重
            var channel = CalculateWeight(await channelService.GetChannelsContainsModelAsync(request.Model),
                request.Model);

            if (channel == null) throw new NotModelException(request.Model);

            // 获取渠道指定的实现类型的服务
            var openService = GetKeyedService<IThorImageService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");

            var sw = Stopwatch.StartNew();

            var response = await openService.CreateImage(request, new ThorPlatformOptions
            {
                ApiKey = channel.Key,
                Address = channel.Address,
                Other = channel.Other
            }, context.RequestAborted);

            await context.Response.WriteAsJsonAsync(new ThorImageCreateResponse
            {
                data = response.Results,
                created = response.CreatedAt,
                successful = response.Successful
            });

            sw.Stop();

            await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate, 0), request.Model,
                0, 0, quota ?? 0, token?.Key, user?.UserName, user?.Id, channel.Id,
                channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                organizationId);

            await userService.ConsumeAsync(user!.Id, quota ?? 0, 0, token?.Key, channel.Id, request.Model);
        }
        catch (PaymentRequiredException)
        {
            context.Response.StatusCode = 402;
            await context.WriteErrorAsync("账号余额不足请充值", "402");
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (Exception e)
        {
            logger.LogError("对话模型请求异常：{e}", e);
            await context.WriteErrorAsync(e.Message);
        }
    }

    public async ValueTask EmbeddingAsync(HttpContext context, ThorEmbeddingInput input)
    {
        try
        {
            if (input == null) throw new Exception("模型校验异常");

            using var embedding =
                Activity.Current?.Source.StartActivity("向量模型调用");

            var organizationId = string.Empty;
            if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
            {
                organizationId = organizationIdHeader.ToString();
            }

            if (ModelManagerService.PromptRate.TryGetValue(input.Model, out var rate))
            {
                var (token, user) = await tokenService.CheckTokenAsync(context, rate);

                await rateLimitModelService.CheckAsync(input!.Model, user.Id);

                input.Model = TokenService.ModelMap(input.Model);

                TokenService.CheckModel(input.Model, token, context);

                // 获取渠道 通过算法计算权重
                var channel = CalculateWeight(await channelService.GetChannelsContainsModelAsync(input.Model),
                    input.Model);

                if (channel == null) throw new NotModelException(input.Model);

                // 获取渠道指定的实现类型的服务
                var embeddingService = GetKeyedService<IThorTextEmbeddingService>(channel.Type);

                if (embeddingService == null) throw new Exception($"并未实现：{channel.Type} 的服务");

                var embeddingCreateRequest = new EmbeddingCreateRequest
                {
                    Model = input.Model,
                    EncodingFormat = input.EncodingFormat
                };

                int requestToken;
                if (input.Input is JsonElement str)
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

                var sw = Stopwatch.StartNew();

                var stream = await embeddingService.EmbeddingAsync(embeddingCreateRequest, new ThorPlatformOptions
                {
                    ApiKey = channel.Key,
                    Address = channel.Address,
                    Other = channel.Other
                }, context.RequestAborted);
                sw.Stop();


                var quota = requestToken * rate.PromptRate;

                var completionRatio = GetCompletionRatio(input.Model);
                quota += rate.PromptRate * completionRatio;

                // 将quota 四舍五入
                quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                await loggerService.CreateConsumeAsync(
                    string.Format(ConsumerTemplate, rate.PromptRate, completionRatio),
                    input.Model,
                    requestToken, 0, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                    channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                    organizationId);

                await userService.ConsumeAsync(user!.Id, (long)quota, requestToken, token?.Key, channel.Id,
                    input.Model);
                stream.ConvertEmbeddingData(input.EncodingFormat);

                await context.Response.WriteAsJsonAsync(stream);
            }
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (Exception e)
        {
            GetLogger<ChatService>().LogError(e.Message);
            await context.WriteErrorAsync(e.Message);
        }
    }

    public async ValueTask CompletionsAsync(HttpContext context, CompletionCreateRequest input)
    {
        using var textCompletions =
            Activity.Current?.Source.StartActivity("文本补全接口");

        if (input == null)
        {
            throw new Exception("模型校验异常");
        }

        try
        {
            // 获取渠道 通过算法计算权重
            var channel = CalculateWeight(await channelService.GetChannelsContainsModelAsync(input.Model), input.Model);

            if (channel == null) throw new NotModelException(input.Model);

            var openService = GetKeyedService<IThorCompletionsService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");

            if (ModelManagerService.PromptRate.TryGetValue(input.Model, out var rate))
            {
                var (token, user) = await tokenService.CheckTokenAsync(context, rate);

                await rateLimitModelService.CheckAsync(input!.Model, user.Id);

                input.Model = TokenService.ModelMap(input.Model);

                TokenService.CheckModel(input.Model, token, context);

                if (input.Stream == false)
                {
                    var sw = Stopwatch.StartNew();
                    var (requestToken, responseToken) =
                        await CompletionsHandlerAsync(context, input, channel, openService, user, rate.PromptRate);

                    var quota = requestToken * rate.PromptRate;

                    var completionRatio = GetCompletionRatio(input.Model);
                    quota += responseToken * rate.PromptRate * completionRatio;

                    // 将quota 四舍五入
                    quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                    sw.Stop();

                    await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate, completionRatio),
                        input.Model,
                        requestToken, responseToken, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                        channel.Name, context.GetIpAddress(), context.GetUserAgent(), false,
                        (int)sw.ElapsedMilliseconds);

                    await userService.ConsumeAsync(user!.Id, (long)quota, requestToken, token?.Key, channel.Id,
                        input.Model);
                }
            }
            else
            {
                context.Response.StatusCode = 200;
                if (input.Stream == true)
                    await context.WriteStreamErrorAsync("当前模型未设置倍率");
                else
                    await context.WriteErrorAsync("当前模型未设置倍率");
            }
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
        }
        catch (Exception e)
        {
            GetLogger<ChatService>().LogError(e.Message);
            await context.WriteErrorAsync(e.Message);
        }
    }

    public async ValueTask<(int, int)> CompletionsHandlerAsync(HttpContext context, CompletionCreateRequest input,
        ChatChannel channel, IThorCompletionsService openService, User user, decimal rate)
    {
        var setting = new ThorPlatformOptions
        {
            ApiKey = channel.Key,
            Address = channel.Address,
            Other = channel.Other
        };

        var requestToken = TokenHelper.GetTotalTokens(input.Prompt ?? string.Empty);

        var result = await openService.CompletionAsync(input, setting);

        var responseToken = TokenHelper.GetTotalTokens(result.Choices.FirstOrDefault()?.Text ?? string.Empty);

        return (requestToken, responseToken);
    }

    /// <summary>
    /// 对话补全调用
    /// </summary>
    /// <param name="context"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotModelException"></exception>
    public async ValueTask ChatCompletionsAsync(HttpContext context, ThorChatCompletionsRequest request)
    {
        using var chatCompletions =
            Activity.Current?.Source.StartActivity("对话补全调用");

        var rateLimit = 0;

        // 用于限流重试，如果限流则重试并且进行重新负载均衡计算
        limitGoto:

        try
        {
            var organizationId = string.Empty;
            if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
            {
                organizationId = organizationIdHeader.ToString();
            }

            if (ModelManagerService.PromptRate.TryGetValue(request.Model, out var rate))
            {
                var (token, user) = await tokenService.CheckTokenAsync(context, rate);

                await rateLimitModelService.CheckAsync(request.Model, user.Id);
                
                request.Model = TokenService.ModelMap(request.Model);

                TokenService.CheckModel(request.Model, token, context);

                // 获取渠道通过算法计算权重
                var channel = CalculateWeight(await channelService.GetChannelsContainsModelAsync(request.Model),
                    request.Model);

                if (channel == null)
                {
                    throw new NotModelException(request.Model);
                }

                // 获取渠道指定的实现类型的服务
                var chatCompletionsService = GetKeyedService<IThorChatCompletionsService>(channel.Type);

                if (chatCompletionsService == null)
                {
                    throw new Exception($"并未实现：{channel.Type} 的服务");
                }


                // 记录请求模型 / 请求用户
                logger.LogInformation("请求模型：{model} 请求用户：{user}", request.Model, user?.UserName);

                int requestToken;
                var responseToken = 0;

                var sw = Stopwatch.StartNew();

                if (request.Stream == true)
                {
                    using var activity =
                        Activity.Current?.Source.StartActivity("流式对话", ActivityKind.Internal);

                    (requestToken, responseToken) =
                        await StreamChatCompletionsHandlerAsync(context, request, channel, chatCompletionsService, user,
                            rate);
                }
                else
                {
                    using var activity =
                        Activity.Current?.Source.StartActivity("非流式对话", ActivityKind.Internal);

                    (requestToken, responseToken) =
                        await ChatCompletionsHandlerAsync(context, request, channel, chatCompletionsService, user,
                            rate);
                }

                var quota = requestToken * rate.PromptRate;

                var completionRatio = GetCompletionRatio(request.Model);
                quota += responseToken * rate.PromptRate * completionRatio;

                // 将quota 四舍五入
                quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                sw.Stop();

                // 判断是否按次
                if (rate.QuotaType == ModelQuotaType.OnDemand)
                {
                    await loggerService.CreateConsumeAsync(
                        string.Format(ConsumerTemplate, rate.PromptRate, completionRatio),
                        request.Model,
                        requestToken, responseToken, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                        channel.Name, context.GetIpAddress(), context.GetUserAgent(),
                        request.Stream is true,
                        (int)sw.ElapsedMilliseconds, organizationId);

                    await userService.ConsumeAsync(user!.Id, (long)quota, requestToken, token?.Key, channel.Id,
                        request.Model);
                }
                else
                {
                    // 费用
                    await loggerService.CreateConsumeAsync(
                        string.Format(ConsumerTemplateOnDemand, RenderHelper.RenderQuota(rate.PromptRate)),
                        request.Model,
                        requestToken, responseToken, (int)rate.PromptRate, token?.Key, user?.UserName, user?.Id,
                        channel.Id,
                        channel.Name, context.GetIpAddress(), context.GetUserAgent(),
                        request.Stream is true,
                        (int)sw.ElapsedMilliseconds, organizationId);

                    await userService.ConsumeAsync(user!.Id, (long)rate.PromptRate, requestToken, token?.Key,
                        channel.Id,
                        request.Model);
                }
            }
            else
            {
                context.Response.StatusCode = 200;
                if (request.Stream == true)
                    await context.WriteStreamErrorAsync("当前模型未设置倍率");
                else
                    await context.WriteErrorAsync("当前模型未设置倍率");
            }
        }
        catch (ThorRateLimitException)
        {
            logger.LogWarning("对话模型请求限流：{rateLimit}", rateLimit);
            rateLimit++;
            // TODO：限流重试次数
            if (rateLimit > 3)
            {
                context.Response.StatusCode = 429;
            }
            else
            {
                goto limitGoto;
            }
        }
        catch (InsufficientQuotaException insufficientQuotaException)
        {
            if (context.Response.StatusCode != 402)
            {
                context.Response.StatusCode = 402;
            }

            await context.WriteErrorAsync(insufficientQuotaException.Message, "402");
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (OpenAIErrorException error)
        {
            context.Response.StatusCode = 400;
            await context.WriteErrorAsync(error.Message, error.Code);
        }
        catch (NotModelException modelException)
        {
            context.Response.StatusCode = 400;
            await context.WriteErrorAsync(modelException.Message, "400");
        }
        catch (Exception e)
        {
            logger.LogError("对话模型请求异常：{e} 准备重试{rateLimit}", e, rateLimit);
            rateLimit++;
            // TODO：限流重试次数
            if (rateLimit > 3)
            {
                context.Response.StatusCode = 400;
                await context.WriteErrorAsync(e.Message, "500");
            }
            else
            {
                goto limitGoto;
            }
        }
    }

    public async ValueTask RealtimeAsync(HttpContext context)
    {
        try
        {
            var organizationId = string.Empty;
            if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
            {
                organizationId = organizationIdHeader.ToString();
            }

            var model = context.Request.Query["model"].ToString();

            using var chatCompletions =
                Activity.Current?.Source.StartActivity("对话补全调用");

            // 获取渠道通过算法计算权重
            var channel = CalculateWeight(await channelService.GetChannelsContainsModelAsync(model), model);

            if (channel == null)
            {
                throw new NotModelException(model);
            }


            // 获取渠道指定的实现类型的服务
            var realtimeService = GetKeyedService<IThorRealtimeService>(channel.Type);

            if (realtimeService == null)
            {
                throw new Exception($"并未实现：{channel.Type} 的服务");
            }

            if (ModelManagerService.PromptRate.TryGetValue(model, out var rate))
            {
                var (token, user) = await tokenService.CheckTokenAsync(context, rate);

                await rateLimitModelService.CheckAsync(model, user.Id);
                
                model = TokenService.ModelMap(model);

                TokenService.CheckModel(model, token, context);
                // 记录请求模型 / 请求用户
                logger.LogInformation("请求模型：{model} 请求用户：{user}", model, user?.UserName);

                decimal requestToken = 0;
                decimal audioRequestToken = 0;
                decimal responseToken = 0;
                decimal audioResponseTokens = 0;

                var sw = Stopwatch.StartNew();

                if (context.WebSockets.IsWebSocketRequest)
                {
                    var buffer = ArrayPool<byte>.Shared.Rent(1024 * 1024 * 2);

                    try
                    {
                        var platformOptions = new ThorPlatformOptions(channel.Address, channel.Key, channel.Other);

                        using var websocket = await context.WebSockets.AcceptWebSocketAsync("realtime");

                        using var client = realtimeService.CreateClient();

                        client.OnBinaryMessage += async (sender, args) =>
                        {
                            if (websocket is { State: WebSocketState.Open })
                            {
                                await websocket!.SendAsync(args.Item1, WebSocketMessageType.Text, args.Item2,
                                    CancellationToken.None);
                            }
                        };

                        client.OnMessage += async (sender, args) =>
                        {
                            if (websocket is { State: WebSocketState.Open })
                            {
                                if (args is { Type: "response.done", Response.Usage: not null })
                                {
                                    requestToken = args.Response.Usage.InputTokenDetails?.TextTokens ?? 0;
                                    audioRequestToken = args.Response.Usage.InputTokenDetails?.AudioTokens ?? 0;
                                    responseToken = args.Response.Usage.OutputTokenDetails?.TextTokens ?? 0;
                                    audioResponseTokens = args.Response.Usage.OutputTokenDetails?.AudioTokens ?? 0;

                                    if (args.Response.Usage.InputTokenDetails?.CachedTokensDetails?.Audio > 0 ||
                                        args.Response.Usage.InputTokenDetails?.CachedTokensDetails?.Text > 0)
                                    {
                                        requestToken +=
                                            args.Response.Usage.InputTokenDetails?.CachedTokensDetails.Text ?? 0;
                                        audioRequestToken += args.Response.Usage.InputTokenDetails?.CachedTokensDetails
                                            .Audio ?? 0;
                                    }
                                }

                                await websocket?.SendAsync(
                                    JsonSerializer.SerializeToUtf8Bytes(args, ThorJsonSerializer.DefaultOptions),
                                    WebSocketMessageType.Text, true,
                                    CancellationToken.None);
                            }
                        };

                        await client.OpenAsync(new OpenRealtimeInput()
                        {
                            Model = model
                        }, platformOptions);

                        var result =
                            await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            return;
                        }

                        var messageBytes = buffer.AsSpan(0, result.Count).ToArray();


                        await client.SendAsync(JsonSerializer.Deserialize<RealtimeInput>(messageBytes,
                            ThorJsonSerializer.DefaultOptions) ?? new RealtimeInput()).ConfigureAwait(false);


                        while (true)
                        {
                            result =
                                await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                            if (result.MessageType == WebSocketMessageType.Close)
                            {
                                break;
                            }

                            await client.SendAsync(JsonSerializer.Deserialize<RealtimeInput>(
                                buffer.AsSpan(0, result.Count).ToArray(),
                                ThorJsonSerializer.DefaultOptions) ?? new RealtimeInput()).ConfigureAwait(false);
                        }

                        await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
                    }
                    catch (Exception e)
                    {
                        logger.LogError("实时对话异常：{e}", e);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }

                    // 如果没有请求token和响应token则直接返回
                    if (requestToken == 0 && audioRequestToken == 0 && responseToken == 0 && audioResponseTokens == 0)
                    {
                        return;
                    }

                    var quota = requestToken * rate.PromptRate;

                    var completionRatio = GetCompletionRatio(model);
                    quota += responseToken * rate.PromptRate * completionRatio;

                    var audioQuota = audioRequestToken * (ModelManagerService.PromptRate[model].AudioPromptRate ?? 0);
                    var audioCompletionRatio = audioResponseTokens *
                                               (ModelManagerService.PromptRate[model].AudioOutputRate ?? 0);

                    quota += audioQuota;
                    quota += audioCompletionRatio;

                    // 将quota 四舍五入
                    quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                    sw.Stop();

                    await loggerService.CreateConsumeAsync(
                        string.Format(RealtimeConsumerTemplate, rate, completionRatio,
                            (ModelManagerService.PromptRate[model].AudioPromptRate),
                            ModelManagerService.PromptRate[model].AudioOutputRate),
                        model,
                        (int)requestToken + (int)audioRequestToken, (int)responseToken + (int)audioResponseTokens,
                        (int)quota, token?.Key,
                        user?.UserName, user?.Id,
                        channel.Id,
                        channel.Name, context.GetIpAddress(), context.GetUserAgent(),
                        true,
                        (int)sw.ElapsedMilliseconds, organizationId);

                    await userService.ConsumeAsync(user!.Id, (long)quota, (int)requestToken, token?.Key, channel.Id,
                        model);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                throw new Exception("当前模型未设置倍率");
            }
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
    }

    /// <summary>
    /// 对话补全服务处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="request"></param>
    /// <param name="channel"></param>
    /// <param name="openService"></param>
    /// <param name="user"></param>
    /// <param name="rate"></param>
    /// <returns></returns>
    /// <exception cref="InsufficientQuotaException"></exception>
    private async ValueTask<(int requestToken, int responseToken)> ChatCompletionsHandlerAsync(
        HttpContext context,
        ThorChatCompletionsRequest request,
        ChatChannel channel,
        IThorChatCompletionsService openService,
        User user,
        ModelManager rate)
    {
        int requestToken = 0;
        int responseToken = 0;

        var platformOptions = new ThorPlatformOptions(channel.Address, channel.Key, channel.Other);


        // 这里应该用其他的方式来判断是否是vision模型，目前先这样处理
        if (rate.QuotaType == ModelQuotaType.OnDemand && request.Messages.Any(x => x.Contents != null))
        {
            requestToken = TokenHelper.GetTotalTokens(request?.Messages.Where(x => x.Contents != null)
                .SelectMany(x => x.Contents)
                .Where(x => x.Type == "text").Select(x => x.Text).ToArray());

            requestToken += TokenHelper.GetTotalTokens(request.Messages.Where(x => x.Contents == null)
                .Select(x => x.Content).ToArray());

            // 解析图片
            foreach (var message in request.Messages.Where(x => x.Contents != null).SelectMany(x => x.Contents)
                         .Where(x => x.Type is "image" or "image_url"))
            {
                var imageUrl = message.ImageUrl;
                if (imageUrl != null)
                {
                    var url = imageUrl.Url;
                    var detail = "";
                    if (!string.IsNullOrEmpty(imageUrl.Detail)) detail = imageUrl.Detail;

                    try
                    {
                        var imageTokens = await CountImageTokens(url, detail);
                        requestToken += imageTokens.Item1;
                    }
                    catch (Exception ex)
                    {
                        GetLogger<ChatService>().LogError("Error counting image tokens: " + ex.Message);
                    }
                }
            }


            var quota = requestToken * rate.PromptRate;

            // 判断请求token数量是否超过额度
            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            ThorChatCompletionsResponse result = await openService.ChatCompletionsAsync(request, platformOptions);

            await context.Response.WriteAsJsonAsync(result);

            if (result?.Usage?.PromptTokens is not null && result.Usage.PromptTokens > 0)
            {
                requestToken = result.Usage.PromptTokens.Value;
            }

            // 如果存在返回的Usage则使用返回的Usage中的CompletionTokens
            if (result?.Usage?.CompletionTokens is not null && result.Usage.CompletionTokens > 0)
            {
                responseToken = result.Usage.CompletionTokens.Value;
            }
            else
            {
                responseToken =
                    TokenHelper.GetTotalTokens(result?.Choices?.Select(x => x.Delta?.Content).ToArray() ?? []);
            }
        }
        else if (rate.QuotaType == ModelQuotaType.OnDemand)
        {
            var contentArray = request.Messages.Select(x => x.Content).ToArray();
            requestToken = TokenHelper.GetTotalTokens(contentArray);

            var quota = requestToken * rate.PromptRate;

            // 判断请求token数量是否超过额度
            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            var result = await openService.ChatCompletionsAsync(request, platformOptions);

            await context.Response.WriteAsJsonAsync(result);

            responseToken = TokenHelper.GetTokens(result.Choices?.FirstOrDefault()?.Delta.Content ?? string.Empty);
        }
        else
        {
            ThorChatCompletionsResponse result = await openService.ChatCompletionsAsync(request, platformOptions);

            await context.Response.WriteAsJsonAsync(result);
        }

        if (rate.QuotaType == ModelQuotaType.OnDemand && request.ResponseFormat?.JsonSchema is not null)
        {
            requestToken += TokenHelper.GetTotalTokens(request.ResponseFormat.JsonSchema.Name,
                request.ResponseFormat.JsonSchema.Description ?? string.Empty,
                JsonSerializer.Serialize(request.ResponseFormat.JsonSchema.Schema));
        }

        if (rate.QuotaType == ModelQuotaType.OnDemand && request.Tools != null && request.Tools.Count != 0)
        {
            requestToken += TokenHelper.GetTotalTokens(request.Tools.Where(x => !string.IsNullOrEmpty(x.Function?.Name))
                .Select(x => x.Function!.Name).ToArray());
            requestToken += TokenHelper.GetTotalTokens(request.Tools
                .Where(x => !string.IsNullOrEmpty(x.Function?.Description))
                .Select(x => x.Function!.Description!).ToArray());
            requestToken += TokenHelper.GetTotalTokens(request.Tools.Where(x => !string.IsNullOrEmpty(x.Function?.Type))
                .Select(x => x.Function!.Type!).ToArray());
        }

        return (requestToken, responseToken);
    }

    public async Task TranslationsAsync(HttpContext context)
    {
        try
        {
            using var audio =
                Activity.Current?.Source.StartActivity("音频翻译");

            var organizationId = string.Empty;
            if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
            {
                organizationId = organizationIdHeader.ToString();
            }

            var audioCreateTranscriptionRequest = new AudioCreateTranscriptionRequest();

            var responseFormat = context.Request.Form["response_format"].ToString();
            var temperature = context.Request.Form["temperature"].ToString();
            audioCreateTranscriptionRequest.Model = context.Request.Form["model"].ToString();
            audioCreateTranscriptionRequest.Prompt = context.Request.Form["prompt"].ToString();
            audioCreateTranscriptionRequest.ResponseFormat = responseFormat;
            if (!string.IsNullOrEmpty(temperature))
            {
                audioCreateTranscriptionRequest.Temperature = float.Parse(temperature);
            }

            // 读取文件
            var file = context.Request.Form.Files.GetFile("file");
            if (file == null)
            {
                throw new Exception("文件不能为空");
            }

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            audioCreateTranscriptionRequest.File = ms.ToArray();
            audioCreateTranscriptionRequest.FileName = file.FileName;
            audioCreateTranscriptionRequest.FileStream = ms;

            var rate = ModelManagerService.PromptRate[audioCreateTranscriptionRequest.Model];

            var (token, user) = await tokenService.CheckTokenAsync(context, rate);

            await rateLimitModelService.CheckAsync(audioCreateTranscriptionRequest.Model, user.Id);

            audioCreateTranscriptionRequest.Model = TokenService.ModelMap(audioCreateTranscriptionRequest.Model);

            TokenService.CheckModel(audioCreateTranscriptionRequest.Model, token, context);

            decimal quota = (int)rate.PromptRate;

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            // 获取渠道 通过算法计算权重
            var channel = CalculateWeight(
                await channelService.GetChannelsContainsModelAsync(audioCreateTranscriptionRequest.Model),
                audioCreateTranscriptionRequest.Model);

            if (channel == null) throw new NotModelException(audioCreateTranscriptionRequest.Model);

            // 获取渠道指定的实现类型的服务
            var openService = GetKeyedService<IThorAudioService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");


            var sw = Stopwatch.StartNew();

            var response =
                await openService.TranslationsAsync(audioCreateTranscriptionRequest, new ThorPlatformOptions
                {
                    ApiKey = channel.Key,
                    Address = channel.Address,
                    Other = channel.Other
                }, context.RequestAborted);

            var requestToken = TokenHelper.GetTotalTokens(response.Text);

            quota = requestToken * rate.PromptRate;

            await context.Response.WriteAsJsonAsync(response);

            sw.Stop();

            await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate.PromptRate, 0),
                audioCreateTranscriptionRequest.Model,
                requestToken, 0, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                organizationId);

            await userService.ConsumeAsync(user!.Id, (int)quota, 0, token?.Key, channel.Id,
                audioCreateTranscriptionRequest.Model);
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (Exception e)
        {
            logger.LogError("对话模型请求异常：{e}", e);
            await context.WriteErrorAsync(e.Message);
        }
    }

    public async Task SpeechAsync(HttpContext context, AudioCreateSpeechRequest request)
    {
        try
        {
            using var audio =
                Activity.Current?.Source.StartActivity("文本转语音");

            var organizationId = string.Empty;
            if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
            {
                organizationId = organizationIdHeader.ToString();
            }

            var rate = ModelManagerService.PromptRate[request.Model];

            var (token, user) = await tokenService.CheckTokenAsync(context, rate);

            await rateLimitModelService.CheckAsync(request.Model, user.Id);

            request.Model = TokenService.ModelMap(request.Model);

            TokenService.CheckModel(request.Model, token, context);

            decimal quota = (int)rate.PromptRate;

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            // 获取渠道 通过算法计算权重
            var channel = CalculateWeight(
                await channelService.GetChannelsContainsModelAsync(request.Model),
                request.Model);

            if (channel == null) throw new NotModelException(request.Model);

            // 获取渠道指定的实现类型的服务
            var openService = GetKeyedService<IThorAudioService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");


            var sw = Stopwatch.StartNew();

            var response =
                await openService.SpeechAsync(request, new ThorPlatformOptions
                {
                    ApiKey = channel.Key,
                    Address = channel.Address,
                    Other = channel.Other
                }, context.RequestAborted);

            // 计算音频的时长
            var requestToken = TokenHelper.GetTotalTokens(request.Input);

            quota = requestToken * rate.PromptRate * (rate.CompletionRate ?? 1);

            switch (request.ResponseFormat)
            {
                case "opus":
                    context.Response.ContentType = "audio/ogg";
                    context.Response.Headers["Content-Disposition"] = $"attachment; filename={Guid.NewGuid()}.opus";
                    break;
                case "aac":
                    context.Response.ContentType = "audio/aac";
                    context.Response.Headers["Content-Disposition"] = $"attachment; filename={Guid.NewGuid()}.aac";
                    break;
                case "flac":
                    context.Response.ContentType = "audio/flac";
                    context.Response.Headers["Content-Disposition"] = $"attachment; filename={Guid.NewGuid()}.flac";
                    break;
                case "wav":
                    context.Response.ContentType = "audio/wav";
                    context.Response.Headers["Content-Disposition"] = $"attachment; filename={Guid.NewGuid()}.wav";
                    break;
                case "pcm":
                    context.Response.ContentType = "audio/wav";
                    context.Response.Headers["Content-Disposition"] = $"attachment; filename={Guid.NewGuid()}.wav";
                    break;
                case "mp3":
                    context.Response.ContentType = "audio/mpeg";
                    context.Response.Headers["Content-Disposition"] = $"attachment; filename={Guid.NewGuid()}.mp3";
                    break;
                default:
                    context.Response.ContentType = "audio/mpeg";
                    context.Response.Headers["Content-Disposition"] = $"attachment; filename={Guid.NewGuid()}.mp3";
                    break;
            }

            await response.CopyToAsync(context.Response.Body);

            sw.Stop();

            await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate.PromptRate, 0),
                request.Model,
                requestToken, 0, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                organizationId);

            await userService.ConsumeAsync(user!.Id, (int)quota, 0, token?.Key, channel.Id,
                request.Model);
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (Exception e)
        {
            logger.LogError("对话模型请求异常：{e}", e);
            await context.WriteErrorAsync(e.Message);
        }
    }

    public async Task TranscriptionsAsync(HttpContext context)
    {
        try
        {
            using var audio =
                Activity.Current?.Source.StartActivity("音频转写");

            var organizationId = string.Empty;
            if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
            {
                organizationId = organizationIdHeader.ToString();
            }

            var audioCreateTranscriptionRequest = new AudioCreateTranscriptionRequest();

            var responseFormat = context.Request.Form["response_format"].ToString();
            var temperature = context.Request.Form["temperature"].ToString();
            var language = context.Request.Form["language"].ToString();
            audioCreateTranscriptionRequest.Model = context.Request.Form["model"].ToString();
            audioCreateTranscriptionRequest.Prompt = context.Request.Form["prompt"].ToString();
            audioCreateTranscriptionRequest.ResponseFormat = responseFormat;
            if (!string.IsNullOrEmpty(temperature))
            {
                audioCreateTranscriptionRequest.Temperature = float.Parse(temperature);
            }

            // 读取文件
            var file = context.Request.Form.Files.GetFile("file");
            if (file == null)
            {
                throw new Exception("文件不能为空");
            }

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            audioCreateTranscriptionRequest.File = ms.ToArray();
            audioCreateTranscriptionRequest.FileName = file.FileName;
            audioCreateTranscriptionRequest.FileStream = ms;

            audioCreateTranscriptionRequest.Language = language;

            var rate = ModelManagerService.PromptRate[audioCreateTranscriptionRequest.Model];

            var (token, user) = await tokenService.CheckTokenAsync(context, rate);

            await rateLimitModelService.CheckAsync(audioCreateTranscriptionRequest.Model, user.Id);

            audioCreateTranscriptionRequest.Model = TokenService.ModelMap(audioCreateTranscriptionRequest.Model);

            TokenService.CheckModel(audioCreateTranscriptionRequest.Model, token, context);

            decimal quota = (int)rate.PromptRate;

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            // 获取渠道 通过算法计算权重
            var channel = CalculateWeight(
                await channelService.GetChannelsContainsModelAsync(audioCreateTranscriptionRequest.Model),
                audioCreateTranscriptionRequest.Model);

            if (channel == null) throw new NotModelException(audioCreateTranscriptionRequest.Model);

            // 获取渠道指定的实现类型的服务
            var openService = GetKeyedService<IThorAudioService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");


            var sw = Stopwatch.StartNew();

            var response =
                await openService.TranscriptionsAsync(audioCreateTranscriptionRequest, new ThorPlatformOptions
                {
                    ApiKey = channel.Key,
                    Address = channel.Address,
                    Other = channel.Other
                }, context.RequestAborted);


            var requestToken = TokenHelper.GetTotalTokens(response.Text);

            quota = requestToken * rate.PromptRate;

            await context.Response.WriteAsJsonAsync(response);

            sw.Stop();

            await loggerService.CreateConsumeAsync(string.Format(ConsumerTemplate, rate.PromptRate, 0),
                audioCreateTranscriptionRequest.Model,
                requestToken, 0, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                organizationId);

            await userService.ConsumeAsync(user!.Id, (int)quota, 0, token?.Key, channel.Id,
                audioCreateTranscriptionRequest.Model);
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (Exception e)
        {
            logger.LogError("对话模型请求异常：{e}", e);
            await context.WriteErrorAsync(e.Message);
        }
    }

    /// <summary>
    /// 流式对话补全服务处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="input">输入</param>
    /// <param name="channel">渠道</param>
    /// <param name="openService"></param>
    /// <param name="user"></param>
    /// <param Name="context"></param>
    /// <param Name="body"></param>
    /// <param Name="module"></param>
    /// <param Name="channel"></param>
    /// <param Name="openService"></param>
    /// <param name="rate"></param>
    /// <returns></returns>
    private async ValueTask<(int requestToken, int responseToken)> StreamChatCompletionsHandlerAsync(
        HttpContext context,
        ThorChatCompletionsRequest input, ChatChannel channel, IThorChatCompletionsService openService, User user,
        ModelManager rate)
    {
        int requestToken = 0;

        var platformOptions = new ThorPlatformOptions(channel.Address, channel.Key, channel.Other);

        var responseMessage = new StringBuilder();

        if (input.Messages.Any(x => x.Contents != null) && rate.QuotaType == ModelQuotaType.OnDemand)
        {
            requestToken = TokenHelper.GetTotalTokens(input?.Messages.Where(x => x.Contents != null)
                .SelectMany(x => x.Contents)
                .Where(x => x.Type == "text").Select(x => x.Text).ToArray());

            requestToken += TokenHelper.GetTotalTokens(input.Messages.Where(x => x.Contents == null)
                .Select(x => x.Content).ToArray());

            // 解析图片
            foreach (var message in input.Messages.Where(x => x is { Contents: not null }).SelectMany(x => x.Contents)
                         .Where(x => x.Type is "image" or "image_url"))
            {
                var imageUrl = message.ImageUrl;
                if (imageUrl != null)
                {
                    var url = imageUrl.Url;
                    var detail = "";
                    if (!string.IsNullOrEmpty(imageUrl.Detail)) detail = imageUrl.Detail;

                    try
                    {
                        var imageTokens = await CountImageTokens(url, detail);
                        requestToken += imageTokens.Item1;
                    }
                    catch (Exception ex)
                    {
                        GetLogger<ChatService>().LogError("Error counting image tokens: " + ex.Message);
                    }
                }
            }

            var quota = requestToken * rate.PromptRate;
            // 判断请求token数量是否超过额度
            if (quota > user.ResidualCredit)
            {
                throw new InsufficientQuotaException("账号余额不足请充值");
            }
        }
        else if (rate.QuotaType == ModelQuotaType.OnDemand)
        {
            requestToken = TokenHelper.GetTotalTokens(input?.Messages.Select(x => x.Content).ToArray());


            var quota = requestToken * rate.PromptRate;

            // 判断请求token数量是否超过额度
            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");
        }

        if (rate.QuotaType == ModelQuotaType.OnDemand && input.ResponseFormat?.JsonSchema is not null)
        {
            requestToken += TokenHelper.GetTotalTokens(input.ResponseFormat.JsonSchema.Name,
                input.ResponseFormat.JsonSchema.Description ?? string.Empty,
                JsonSerializer.Serialize(input.ResponseFormat.JsonSchema.Schema));
        }

        if (rate.QuotaType == ModelQuotaType.OnDemand && input.Tools != null && input.Tools.Count != 0)
        {
            requestToken += TokenHelper.GetTotalTokens(input.Tools.Where(x => !string.IsNullOrEmpty(x.Function?.Name))
                .Select(x => x.Function!.Name).ToArray());
            requestToken += TokenHelper.GetTotalTokens(input.Tools
                .Where(x => !string.IsNullOrEmpty(x.Function?.Description))
                .Select(x => x.Function!.Description!).ToArray());
            requestToken += TokenHelper.GetTotalTokens(input.Tools.Where(x => !string.IsNullOrEmpty(x.Function?.Type))
                .Select(x => x.Function!.Type!).ToArray());
        }

        // 是否第一次输出
        bool isFirst = true;

        await foreach (var item in openService.StreamChatCompletionsAsync(input, platformOptions))
        {
            if (isFirst)
            {
                context.SetEventStreamHeaders();
                isFirst = false;
            }

            if (item.Error != null)
            {
                await context.WriteStreamErrorAsync(item.Error.Message);
            }
            else
            {
                foreach (var response in item.Choices)
                {
                    if (string.IsNullOrEmpty(response.Delta.Role))
                    {
                        response.Delta.Role = "assistant";
                    }

                    if (string.IsNullOrEmpty(response.Message.Role))
                    {
                        response.Message.Role = "assistant";
                    }

                    if (string.IsNullOrEmpty(response.Delta.Content))
                    {
                        response.Delta.Content = null;
                        response.Message.Content = null;
                    }

                    if (string.IsNullOrEmpty(response.FinishReason))
                    {
                        response.FinishReason = null;
                    }
                }
            }

            responseMessage.Append(item.Choices?.FirstOrDefault()?.Delta.Content ?? string.Empty);
            await context.WriteAsEventStreamDataAsync(item).ConfigureAwait(false);
        }

        await context.WriteAsEventStreamEndAsync();

        var responseToken = rate.QuotaType == ModelQuotaType.OnDemand
            ? TokenHelper.GetTokens(responseMessage.ToString())
            : 0;

        return (requestToken, responseToken);
    }

    /// <summary>
    /// 权重算法
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    private static ChatChannel CalculateWeight(IEnumerable<ChatChannel> channel, string model)
    {
        var chatChannels = channel.ToList();
        if (chatChannels.Count == 0)
        {
            throw new NotModelException($"{model} 模型未找到可用的渠道");
        }

        // 所有权重值之和
        var total = chatChannels.Sum(x => x.Order);

        var value = Convert.ToInt32(Random.Shared.NextDouble() * total);

        foreach (var chatChannel in chatChannels)
        {
            value -= chatChannel.Order;
            if (value <= 0)
            {
                return chatChannel;
            }
        }

        return chatChannels.Last();
    }

    /// <summary>
    ///     对话模型补全倍率
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private decimal GetCompletionRatio(string name)
    {
        if (ModelManagerService.PromptRate?.TryGetValue(name, out var ratio) == true)
            return (decimal)(ratio.CompletionRate ?? 0);

        if (name.StartsWith("gpt-3.5"))
        {
            if (name == "gpt-3.5-turbo" || name.EndsWith("0125")) return 3;

            if (name.EndsWith("1106")) return 2;

            return (decimal)(4.0 / 3.0);
        }

        if (name.StartsWith("gpt-4")) return name.StartsWith("gpt-4-turbo") ? 3 : 2;

        if (name.StartsWith("claude-")) return name.StartsWith("claude-3") ? 5 : 3;

        if (name.StartsWith("mistral-") || name.StartsWith("gemini-")) return 3;

        return name switch
        {
            "llama2-70b-4096" => new decimal(0.8 / 0.7),
            _ => 1
        };
    }

    /// <summary>
    /// 计算图片倍率
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    private static decimal GetImageCostRatio(ImageCreateRequest module)
    {
        var imageCostRatio = GetImageSizeRatio(module.Model, module.Size);
        if (module is { Quality: "hd", Model: "dall-e-3" })
        {
            if (module.Size == "1024x1024")
                imageCostRatio *= 2;
            else
                imageCostRatio *= (decimal)1.5;
        }

        return imageCostRatio;
    }

    /// <summary>
    /// 计算图片倍率
    /// </summary>
    /// <param name="model"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static decimal GetImageSizeRatio(string model, string size)
    {
        if (!ImageSizeRatios.TryGetValue(model, out var ratios)) return 1;

        if (ratios.TryGetValue(size, out var ratio)) return (decimal)ratio;

        return 1;
    }

    /// <summary>
    /// 计算图片token
    /// </summary>
    /// <param name="url"></param>
    /// <param name="detail"></param>
    /// <returns></returns>
    public async ValueTask<Tuple<int, Exception>> CountImageTokens(string url, string detail)
    {
        var fetchSize = true;
        int width = 0, height = 0;
        var lowDetailCost = 20; // Assuming lowDetailCost is 20
        var highDetailCostPerTile = 100; // Assuming highDetailCostPerTile is 100
        var additionalCost = 50; // Assuming additionalCost is 50

        if (string.IsNullOrEmpty(detail) || detail == "auto") detail = "high";

        switch (detail)
        {
            case "low":
                return new Tuple<int, Exception>(lowDetailCost, null);
            case "high":
                if (fetchSize)
                    try
                    {
                        (width, height) = await imageService.GetImageSize(url);
                    }
                    catch (Exception e)
                    {
                        return new Tuple<int, Exception>(0, e);
                    }

                if (width > 2048 || height > 2048)
                {
                    var ratio = 2048.0 / Math.Max(width, height);
                    width = (int)(width * ratio);
                    height = (int)(height * ratio);
                }

                if (width > 768 && height > 768)
                {
                    var ratio = 768.0 / Math.Min(width, height);
                    width = (int)(width * ratio);
                    height = (int)(height * ratio);
                }

                var numSquares = (int)Math.Ceiling((double)width / 512) * (int)Math.Ceiling((double)height / 512);
                var result = numSquares * highDetailCostPerTile + additionalCost;
                return new Tuple<int, Exception>(result, null);
            default:
                return new Tuple<int, Exception>(0, new Exception("Invalid detail option"));
        }
    }
}