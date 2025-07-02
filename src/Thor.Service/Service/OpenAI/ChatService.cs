using System.Buffers;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Thor.Abstractions.Audios;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Embeddings.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Images;
using Thor.Abstractions.Images.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.Abstractions.Realtime;
using Thor.Abstractions.Realtime.Dto;
using Thor.Domain.Chats;
using Thor.Infrastructure;
using Thor.Service.Domain.Core;
using Thor.Service.Extensions;
using Thor.Service.Infrastructure;

namespace Thor.Service.Service.OpenAI;

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
public sealed partial class ChatService(
    IServiceProvider serviceProvider,
    ChannelService channelService,
    TokenService tokenService,
    ImageService imageService,
    RateLimitModelService rateLimitModelService,
    UserService userService,
    UserGroupService userGroupService,
    ILogger<ChatService> logger,
    ModelMapService modelMapService,
    LoggerService loggerService)
    : AIService(serviceProvider, imageService)
{
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

            logger.LogInformation("请求参数：{query}", input);


            if (ModelManagerService.PromptRate.TryGetValue(input.Model, out var rate))
            {
                var (token, user) = await tokenService.CheckTokenAsync(context, rate);

                await rateLimitModelService.CheckAsync(input!.Model, user.Id);

                TokenService.CheckModel(input.Model, token, context);

                input.Model = await modelMapService.ModelMap(input.Model);

                // 获取渠道 通过算法计算权重
                var channel = CalculateWeight(
                    await channelService.GetChannelsContainsModelAsync(input.Model, user, token));

                if (channel == null)
                    throw new NotModelException(
                        $"{input.Model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

                var userGroup = await userGroupService.GetAsync(channel.Groups);

                if (userGroup == null)
                {
                    throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
                }

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


                var quota = (stream.Usage?.InputTokens ?? requestToken) * rate.PromptRate;

                var completionRatio = GetCompletionRatio(input.Model);
                quota += rate.PromptRate * completionRatio;

                quota = (decimal)userGroup.Rate * quota;

                // 将quota 四舍五入
                quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                await loggerService.CreateConsumeAsync("/v1/embeddings",
                    string.Format(ConsumerTemplate, rate.PromptRate, completionRatio, userGroup.Rate),
                    input.Model,
                    (stream.Usage?.InputTokens ?? requestToken), 0, (int)quota, token?.Key, user?.UserName, user?.Id,
                    channel.Id,
                    channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                    organizationId);

                await userService.ConsumeAsync(user!.Id, (long)quota, (stream.Usage?.InputTokens ?? requestToken),
                    token?.Key, channel.Id,
                    input.Model);

                stream.ConvertEmbeddingData(input.EncodingFormat);

                await context.Response.WriteAsJsonAsync(new
                {
                    input.Model,
                    stream.Data,
                    stream.Error,
                    stream.ObjectTypeName,
                    Usage = new ThorUsageResponse()
                    {
                        InputTokens = (stream.Usage?.InputTokens ?? requestToken),
                        CompletionTokens = 0,
                        TotalTokens = (stream.Usage?.InputTokens ?? requestToken)
                    }
                });
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
            await context.WriteOpenAIErrorAsync(e.Message);
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
            if (ModelManagerService.PromptRate.TryGetValue(input.Model, out var rate))
            {
                var (token, user) = await tokenService.CheckTokenAsync(context, rate);

                // 获取渠道 通过算法计算权重
                var channel = CalculateWeight(
                    await channelService.GetChannelsContainsModelAsync(input.Model, user, token));

                if (channel == null)
                    throw new NotModelException(
                        $"{input.Model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

                var userGroup = await userGroupService.GetAsync(channel.Groups);

                if (userGroup == null)
                {
                    throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
                }

                var openService = GetKeyedService<IThorCompletionsService>(channel.Type);

                if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");

                await rateLimitModelService.CheckAsync(input!.Model, user.Id);

                TokenService.CheckModel(input.Model, token, context);

                input.Model = await modelMapService.ModelMap(input.Model);

                if (input.Stream == false)
                {
                    var sw = Stopwatch.StartNew();
                    var (requestToken, responseToken) =
                        await CompletionsHandlerAsync(context, input, channel, openService, user, rate.PromptRate);

                    var quota = requestToken * rate.PromptRate;

                    var completionRatio = GetCompletionRatio(input.Model);
                    quota += responseToken * rate.PromptRate * completionRatio;

                    quota = (decimal)userGroup.Rate * quota;

                    // 将quota 四舍五入
                    quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                    sw.Stop();

                    await loggerService.CreateConsumeAsync("/v1/completions",
                        string.Format(ConsumerTemplate, rate, completionRatio, userGroup.Rate),
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

        var model = request.Model;

        if (request.MaxCompletionTokens is > 0)
        {
            request.MaxTokens = request.MaxCompletionTokens;
            request.MaxCompletionTokens = null;
        }

        if (request.Model.StartsWith("o3-mini") || request.Model.StartsWith("o4-mini"))
        {
            request.MaxCompletionTokens = request.MaxTokens;
            request.MaxTokens = null;
            request.Temperature = null;
        }

        var rateLimit = 0;
        Exception? lastException = null;

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

                TokenService.CheckModel(request.Model, token, context);

                request.Model = await modelMapService.ModelMap(request.Model);

                // 获取渠道通过算法计算权重
                var channel =
                    CalculateWeight(await channelService.GetChannelsContainsModelAsync(request.Model, user, token));

                if (lastException == null && channel == null)
                    throw new NotModelException(
                        $"{request.Model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

                if (lastException != null && channel == null)
                {
                    await context.WriteErrorAsync(lastException.Message);
                    return;
                }

                var userGroup = await userGroupService.GetAsync(channel.Groups);

                if (userGroup == null)
                {
                    throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
                }

                ChannelAsyncLocal.ChannelIds.Add(channel.Id);

                // 获取渠道指定的实现类型的服务
                var chatCompletionsService = GetKeyedService<IThorChatCompletionsService>(channel.Type);

                if (chatCompletionsService == null)
                {
                    throw new BusinessException($"并未实现：{channel.Type} 的服务", "400");
                }


                // 记录请求模型 / 请求用户
                logger.LogInformation("请求模型：{model} 请求用户：{user} 请求分配渠道 ：{name}", request.Model, user?.UserName,
                    channel.Name);

                int requestToken;
                var responseToken = 0;
                int cachedTokens = 0;

                var sw = Stopwatch.StartNew();

                if (request.Stream == true)
                {
                    (requestToken, responseToken) =
                        await StreamChatCompletionsHandlerAsync(context, request, channel, chatCompletionsService, user,
                            rate);
                }
                else
                {
                    (requestToken, responseToken, cachedTokens) =
                        await ChatCompletionsHandlerAsync(context, request, channel, chatCompletionsService, user,
                            rate);
                }

                var quota = requestToken * rate.PromptRate;

                var completionRatio = rate.CompletionRate ?? GetCompletionRatio(request.Model);
                quota += responseToken * rate.PromptRate * completionRatio;

                // 计算分组倍率
                quota = (decimal)userGroup!.Rate * quota;

                // 将quota 四舍五入
                quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                sw.Stop();

                // 判断是否按次
                if (rate.QuotaType == ModelQuotaType.OnDemand)
                {
                    // 如果命中缓存 并且缓存倍率大于0 小于0则不计算缓存
                    if (cachedTokens > 0 && rate.CacheRate > 0)
                    {
                        // 如果命中缓存充值quota
                        quota = requestToken * rate.CacheRate.Value;

                        quota += responseToken * rate.CacheRate.Value * completionRatio;

                        // 计算分组倍率
                        quota = (decimal)userGroup!.Rate * quota;

                        // 将quota 四舍五入
                        quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                        await loggerService.CreateConsumeAsync("/v1/chat/completions",
                            string.Format(ConsumerTemplateCache, rate.PromptRate, completionRatio, userGroup.Rate,
                                cachedTokens, rate.CacheRate),
                            model,
                            requestToken, responseToken, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                            channel.Name, context.GetIpAddress(), context.GetUserAgent(),
                            request.Stream is true,
                            (int)sw.ElapsedMilliseconds, organizationId);
                    }
                    else
                    {
                        await loggerService.CreateConsumeAsync("/v1/chat/completions",
                            string.Format(ConsumerTemplate, rate.PromptRate, completionRatio, userGroup.Rate),
                            model,
                            requestToken, responseToken, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                            channel.Name, context.GetIpAddress(), context.GetUserAgent(),
                            request.Stream is true,
                            (int)sw.ElapsedMilliseconds, organizationId);

                        await userService.ConsumeAsync(user!.Id, (long)quota, requestToken, token?.Key, channel.Id,
                            model);
                    }
                }
                else
                {
                    // 费用
                    await loggerService.CreateConsumeAsync("/v1/chat/completions",
                        string.Format(ConsumerTemplateOnDemand, RenderHelper.RenderQuota(rate.PromptRate),
                            userGroup.Rate),
                        model,
                        requestToken, responseToken, (int)((int)rate.PromptRate * (decimal)userGroup.Rate), token?.Key,
                        user?.UserName, user?.Id,
                        channel.Id,
                        channel.Name, context.GetIpAddress(), context.GetUserAgent(),
                        request.Stream is true,
                        (int)sw.ElapsedMilliseconds, organizationId);

                    await userService.ConsumeAsync(user!.Id, (long)rate.PromptRate, requestToken, token?.Key,
                        channel.Id,
                        model);
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.WriteErrorAsync($"当前{request.Model}模型未设置倍率,请联系管理员设置倍率", "400");
            }
        }
        catch (ThorRateLimitException)
        {
            lastException = new ThorRateLimitException("对话模型请求限流");
            logger.LogWarning("对话模型请求限流：{rateLimit}", rateLimit);
            rateLimit++;
            // TODO：限流重试次数
            if (rateLimit > 5)
            {
                context.Response.StatusCode = 429;
            }
            else
            {
                request.Model = model;
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
            lastException = e;
            rateLimit++;
            // TODO：限流重试次数
            if (rateLimit > 5)
            {
                context.Response.StatusCode = 400;
                await context.WriteErrorAsync(e.Message, "500");
            }
            else
            {
                request.Model = model;
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

            if (ModelManagerService.PromptRate.TryGetValue(model, out var rate))
            {
                var (token, user) = await tokenService.CheckTokenAsync(context, rate);

                // 获取渠道通过算法计算权重
                var channel = CalculateWeight(await channelService.GetChannelsContainsModelAsync(model, user, token));


                if (channel == null)
                    throw new NotModelException(
                        $"{model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

                var userGroup = await userGroupService.GetAsync(channel.Groups);

                if (userGroup == null)
                {
                    throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
                }

                // 获取渠道指定的实现类型的服务
                var realtimeService = GetKeyedService<IThorRealtimeService>(channel.Type);

                if (realtimeService == null)
                {
                    throw new Exception($"并未实现：{channel.Type} 的服务");
                }

                await rateLimitModelService.CheckAsync(model, user.Id);

                TokenService.CheckModel(model, token, context);

                model = await modelMapService.ModelMap(model);
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

                    quota = (decimal)userGroup.Rate * quota;

                    // 将quota 四舍五入
                    quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                    sw.Stop();

                    await loggerService.CreateConsumeAsync("/v1/realtime",
                        string.Format(RealtimeConsumerTemplate, rate, completionRatio,
                            (ModelManagerService.PromptRate[model].AudioPromptRate),
                            ModelManagerService.PromptRate[model].AudioOutputRate, userGroup.Rate),
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
    private async ValueTask<(int requestToken, int responseToken, int cachedTokens)> ChatCompletionsHandlerAsync(
        HttpContext context,
        ThorChatCompletionsRequest request,
        ChatChannel channel,
        IThorChatCompletionsService openService,
        User user,
        ModelManager rate)
    {
        int requestToken = 0;
        int responseToken = 0;

        // 命中缓存tokens数量
        int cachedTokens = 0;

        var platformOptions = new ThorPlatformOptions(channel.Address, channel.Key, channel.Other);

        ThorChatCompletionsResponse result = null;

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

            result = await openService.ChatCompletionsAsync(request, platformOptions);

            await context.Response.WriteAsJsonAsync(result);

            if (result?.Usage?.PromptTokens is not null && result.Usage.PromptTokens > 0)
            {
                requestToken = result.Usage.PromptTokens.Value;
            }

            // 如果存在返回的Usage则使用返回的Usage中的CompletionTokens
            if (result?.Usage?.CompletionTokens is not null && result.Usage.CompletionTokens > 0)
            {
                responseToken = (int)result.Usage.CompletionTokens.Value;
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

            result = await openService.ChatCompletionsAsync(request, platformOptions);

            await context.Response.WriteAsJsonAsync(result);
        }
        else
        {
            result = await openService.ChatCompletionsAsync(request, platformOptions);

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

        if (result?.Usage?.PromptTokens is not null && result.Usage.PromptTokens > 0)
        {
            requestToken = result.Usage.PromptTokens.Value;
        }

        if (result?.Usage?.CompletionTokens is not null && result.Usage.CompletionTokens > 0)
        {
            responseToken = (int)result.Usage.CompletionTokens.Value;
        }
        else
        {
            responseToken += TokenHelper.GetTokens(result?.Choices?.FirstOrDefault()?.Delta.Content ?? string.Empty);
        }


        if (result?.Usage?.PromptTokensDetails?.CachedTokens > 0)
        {
            cachedTokens = result.Usage.PromptTokensDetails.CachedTokens.Value;
        }


        return (requestToken, responseToken, cachedTokens);
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

            TokenService.CheckModel(audioCreateTranscriptionRequest.Model, token, context);

            audioCreateTranscriptionRequest.Model =
                await modelMapService.ModelMap(audioCreateTranscriptionRequest.Model);

            decimal quota = (int)rate.PromptRate;

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            // 获取渠道 通过算法计算权重
            var channel = CalculateWeight(
                await channelService.GetChannelsContainsModelAsync(audioCreateTranscriptionRequest.Model, user, token));

            if (channel == null)
                throw new NotModelException(
                    $"{audioCreateTranscriptionRequest.Model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

            var userGroup = await userGroupService.GetAsync(channel.Groups);

            if (userGroup == null)
            {
                throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
            }

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

            quota = (decimal)userGroup.Rate * quota;

            await loggerService.CreateConsumeAsync("/v1/audio/transcriptions",
                string.Format(ConsumerTemplate, rate.PromptRate, 0, userGroup.Rate),
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

            TokenService.CheckModel(request.Model, token, context);

            request.Model = await modelMapService.ModelMap(request.Model);

            decimal quota = (int)rate.PromptRate;

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            // 获取渠道 通过算法计算权重
            var channel = CalculateWeight(
                await channelService.GetChannelsContainsModelAsync(request.Model, user, token));


            if (channel == null)
                throw new NotModelException(
                    $"{request.Model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

            var userGroup = await userGroupService.GetAsync(channel.Groups);

            if (userGroup == null)
            {
                throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
            }

            // 获取渠道指定的实现类型的服务
            var openService = GetKeyedService<IThorAudioService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");


            var sw = Stopwatch.StartNew();

            var (response, usage) =
                await openService.SpeechAsync(request, new ThorPlatformOptions
                {
                    ApiKey = channel.Key,
                    Address = channel.Address,
                    Other = channel.Other
                }, context.RequestAborted);

            // 计算音频的时长
            var requestToken = usage?.PromptTokens ?? TokenHelper.GetTotalTokens(request.Input);

            quota = requestToken * rate.PromptRate * (rate.CompletionRate ?? 1);

            if (usage?.CompletionTokens > 0)
            {
                // (模型倍率 * (文字输入 + 文字输出 * 补全倍率)
                quota = (decimal)((requestToken + usage.CompletionTokens * (rate.CompletionRate ?? 1)) *
                                  rate.PromptRate) ;
            }

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

            quota = (decimal)userGroup.Rate * quota;

            await loggerService.CreateConsumeAsync("/v1/audio/speech",
                string.Format(ConsumerTemplate, rate.PromptRate, ((int?)usage?.CompletionTokens ?? 0), userGroup.Rate),
                request.Model,
                requestToken, ((int?)usage?.CompletionTokens ?? 0), (int)quota, token?.Key, user?.UserName, user?.Id,
                channel.Id,
                channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                organizationId);

            await userService.ConsumeAsync(user!.Id, (int)quota, ((int?)usage?.CompletionTokens ?? 0), token?.Key,
                channel.Id,
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
            await context.WriteOpenAIErrorAsync(e.Message);
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

            TokenService.CheckModel(audioCreateTranscriptionRequest.Model, token, context);

            audioCreateTranscriptionRequest.Model =
                await modelMapService.ModelMap(audioCreateTranscriptionRequest.Model);

            decimal quota = (int)rate.PromptRate;

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            // 获取渠道 通过算法计算权重
            var channel = CalculateWeight(
                await channelService.GetChannelsContainsModelAsync(audioCreateTranscriptionRequest.Model, user, token));


            if (channel == null)
                throw new NotModelException(
                    $"{audioCreateTranscriptionRequest.Model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

            var userGroup = await userGroupService.GetAsync(channel.Groups);

            if (userGroup == null)
            {
                throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
            }

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

            quota = (decimal)userGroup.Rate * quota;

            await loggerService.CreateConsumeAsync("/v1/audio/translations",
                string.Format(ConsumerTemplate, rate.PromptRate, 0, userGroup.Rate),
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
        int responseToken = 0;

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
                if (item.Usage is { PromptTokens: > 0 })
                {
                    requestToken = item.Usage.PromptTokens.Value;
                }

                if (item.Usage is { CompletionTokens: > 0 })
                {
                    responseToken = (int)item.Usage.CompletionTokens.Value;
                }

                if (item.Choices != null)
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
            }

            responseMessage.Append(item.Choices?.FirstOrDefault()?.Delta.Content ?? string.Empty);
            await context.WriteAsEventStreamDataAsync(item).ConfigureAwait(false);
        }

        await context.WriteAsEventStreamEndAsync();

        if (rate.QuotaType == ModelQuotaType.OnDemand && responseToken == 0)
        {
            responseToken = TokenHelper.GetTokens(responseMessage.ToString());
        }
        else if (rate.QuotaType == ModelQuotaType.ByCount)
        {
            responseToken = rate.QuotaType == ModelQuotaType.OnDemand
                ? TokenHelper.GetTokens(responseMessage.ToString())
                : 0;
        }


        return (requestToken, responseToken);
    }


    /// <summary>
    /// 计算图片倍率
    /// </summary>
    /// <param name="model"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    private static decimal GetImageCostRatio(string model, string size)
    {
        var imageCostRatio = GetImageSizeRatio(model, size);

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
}