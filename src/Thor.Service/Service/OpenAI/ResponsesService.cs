using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Responses;
using Thor.Abstractions.Responses.Dto;
using Thor.Domain.Chats;
using Thor.Infrastructure;
using Thor.Service.Domain.Core;
using Thor.Service.Extensions;
using Thor.Service.Service.OpenAI;

namespace Thor.Service.Service;

/// <summary>
/// OpenAI Responses Service
/// </summary>
/// <param name="serviceProvider"></param>
public sealed class ResponsesService(
    IServiceProvider serviceProvider,
    RateLimitModelService rateLimitModelService,
    TokenService tokenService,
    ILogger<ResponsesService> logger,
    ModelMapService modelMapService,
    ChannelService channelService,
    UserGroupService userGroupService,
    LoggerService loggerService,
    UserService userService,
    ImageService imageService
)
    : AIService(serviceProvider, imageService)
{
    public async Task ExecuteAsync(HttpContext context, ResponsesInput request)
    {
        using var chatCompletions =
            Activity.Current?.Source.StartActivity("对话补全调用");

        var model = request.Model;

        var rateLimit = 0;
        Exception? exception = null;

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
                    CalculateWeight(
                        await channelService.GetChannelsContainsModelAsync(request.Model, user, token, true));

                if (channel == null && exception == null)
                    throw new NotModelException(
                        $"{request.Model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

                if (channel == null && exception != null)
                {
                    await context.WriteErrorAsync(exception.Message, "400");
                    return;
                }

                var userGroup = await userGroupService.GetAsync(channel.Groups);

                if (userGroup == null)
                {
                    throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
                }

                // 获取渠道指定的实现类型的服务
                var chatCompletionsService = GetKeyedService<IThorResponsesService>(channel.Type);

                if (chatCompletionsService == null)
                {
                    throw new BusinessException($"并未实现：{channel.Type} 的服务", "400");
                }


                // 记录请求模型 / 请求用户
                logger.LogInformation("请求模型：{model} 请求用户：{user} 请求分配渠道 ：{name}", model, user?.UserName,
                    channel.Name);

                int requestToken;
                var responseToken = 0;
                int cachedTokens = 0;

                var sw = Stopwatch.StartNew();

                if (request.Stream == true)
                {
                    using var activity =
                        Activity.Current?.Source.StartActivity("流式对话", ActivityKind.Internal);

                    (requestToken, responseToken, cachedTokens) =
                        await StreamHandlerAsync(context, request, channel, chatCompletionsService, user,
                            rate);
                }
                else
                {
                    using var activity =
                        Activity.Current?.Source.StartActivity("非流式对话", ActivityKind.Internal);

                    (requestToken, responseToken, cachedTokens) =
                        await ChatHandlerAsync(context, request, channel, chatCompletionsService, user,
                            rate);
                }

                var quota = requestToken * rate.PromptRate;

                var completionRatio = rate.CompletionRate ?? GetCompletionRatio(model);
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

                        await loggerService.CreateConsumeAsync("/v1/responses",
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
                        await loggerService.CreateConsumeAsync("/v1/responses",
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
                    await loggerService.CreateConsumeAsync("/v1/responses",
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
            exception = new ThorRateLimitException("对话模型请求限流，请稍后再试");
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
            exception = e;
            logger.LogError("对话模型请求异常：{e} 准备重试{rateLimit}，请求参数：{request}", e, rateLimit,
                JsonSerializer.Serialize(request, ThorJsonSerializer.DefaultOptions));
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

    private async Task<(int requestToken, int responseToken, int cachedTokens)> ChatHandlerAsync(HttpContext context,
        ResponsesInput request, ChatChannel channel, IThorResponsesService responsesService, User? user,
        ModelManager rate)
    {
        int requestToken = 0;
        int responseToken = 0;

        // 命中缓存tokens数量
        int cachedTokens = 0;

        var platformOptions = new ThorPlatformOptions(channel.Address, channel.Key, channel.Other);

        ResponsesDto result = null;

        // 这里应该用其他的方式来判断是否是vision模型，目前先这样处理
        if (rate.QuotaType == ModelQuotaType.OnDemand && request.IsMessageArray)
        {
            requestToken = TokenHelper.GetTotalTokens(request?.Inputs.Where(x => x.IsMessageArray)
                .SelectMany(x => x.Contents)
                .Where(x => x.Type == "input_text").Select(x => x.Text).ToArray());

            requestToken += TokenHelper.GetTotalTokens(request.Inputs.Where(x => x.Contents == null)
                .Select(x => x.Content).ToArray());

            // 解析图片
            foreach (var message in request.Inputs.Where(x => x.Contents != null).SelectMany(x => x.Contents)
                         .Where(x => x.Type is "input_image"))
            {
                var imageUrl = message.ImageUrl;
                if (imageUrl != null)
                {
                    try
                    {
                        var imageTokens = await CountImageTokens(message.ImageUrl, "low");
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

            result = await responsesService.GetResponseAsync(request, platformOptions);

            await context.Response.WriteAsJsonAsync(result, ThorJsonSerializer.DefaultOptions);

            if (result?.Usage?.InputTokens is not null && result.Usage.InputTokens > 0)
            {
                requestToken = result.Usage.InputTokens;
            }

            // 如果存在返回的Usage则使用返回的Usage中的CompletionTokens
            if (result?.Usage?.OutputTokens is not null && result.Usage.OutputTokens > 0)
            {
                responseToken = result.Usage.OutputTokens;
            }
            else
            {
                responseToken =
                    TokenHelper.GetTotalTokens(
                        result?.Output?.SelectMany(x => x.Content?.Select(x => x.Text)).ToArray() ?? []);
            }
        }
        else if (rate.QuotaType == ModelQuotaType.OnDemand)
        {
            requestToken =
                TokenHelper.GetTotalTokens(result?.Output?.SelectMany(x => x.Content?.Select(x => x.Text)).ToArray() ??
                                           []);

            var quota = requestToken * rate.PromptRate;

            // 判断请求token数量是否超过额度
            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            result = await responsesService.GetResponseAsync(request, platformOptions);

            await context.Response.WriteAsJsonAsync(result, ThorJsonSerializer.DefaultOptions);

            if (result?.Usage?.InputTokens is not null && result.Usage.InputTokens > 0)
            {
                requestToken = result.Usage.InputTokens;
            }

            if (result?.Usage?.OutputTokens is not null && result.Usage.OutputTokens > 0)
            {
                responseToken = result.Usage.OutputTokens;
            }
            else
            {
                responseToken =
                    TokenHelper.GetTotalTokens(
                        result?.Output?.SelectMany(x => x.Content?.Select(x => x.Text)).ToArray() ?? []);
            }
        }
        else
        {
            result = await responsesService.GetResponseAsync(request, platformOptions);

            await context.Response.WriteAsJsonAsync(result);

            if (result?.Usage?.InputTokens is not null && result.Usage.InputTokens > 0)
            {
                requestToken = result.Usage.InputTokens;
            }

            if (result?.Usage?.OutputTokens is not null && result.Usage.OutputTokens > 0)
            {
                responseToken = result.Usage.OutputTokens;
            }
            else
            {
                responseToken =
                    TokenHelper.GetTotalTokens(
                        result?.Output?.SelectMany(x => x.Content?.Select(x => x.Text)).ToArray() ?? []);
            }
        }

        if (rate.QuotaType == ModelQuotaType.OnDemand && request.Tools != null && request.Tools.Count != 0)
        {
            requestToken += TokenHelper.GetTotalTokens(request.Tools.Where(x => !string.IsNullOrEmpty(x?.Name))
                .Select(x => x!.Name).ToArray());
            requestToken += TokenHelper.GetTotalTokens(request.Tools
                .Where(x => !string.IsNullOrEmpty(x?.Description))
                .Select(x => x!.Description!).ToArray());
            requestToken += TokenHelper.GetTotalTokens(request.Tools.Where(x => !string.IsNullOrEmpty(x?.Type))
                .Select(x => x!.Type!).ToArray());
        }

        if (result?.Usage?.InputTokens is not null && result.Usage.InputTokens > 0)
        {
            requestToken = result.Usage.InputTokens;
        }

        if (result?.Usage?.InputTokensDetails?.CachedTokens > 0)
        {
            cachedTokens = result.Usage.InputTokensDetails.CachedTokens;
        }

        return (requestToken, responseToken, cachedTokens);
    }

    private async Task<(int requestToken, int responseToken, int cachedTokens)> StreamHandlerAsync(HttpContext context,
        ResponsesInput request, ChatChannel channel, IThorResponsesService responsesService, User? user,
        ModelManager rate)
    {
        int requestToken = TokenHelper.GetTokens(request.Instructions ?? string.Empty);
        int responseToken = 0;

        // 命中缓存tokens数量
        int cachedTokens = 0;

        var platformOptions = new ThorPlatformOptions(channel.Address, channel.Key, channel.Other);

        // 这里应该用其他的方式来判断是否是vision模型，目前先这样处理
        if (rate.QuotaType == ModelQuotaType.OnDemand && request.IsMessageArray)
        {
            requestToken += TokenHelper.GetTotalTokens(request?.Inputs.Where(x => x.IsMessageArray)
                .SelectMany(x => x.Contents)
                .Where(x => x.Type == "input_text").Select(x => x.Text).ToArray());

            requestToken += TokenHelper.GetTotalTokens(request.Inputs.Where(x => x.Contents == null)
                .Select(x => x.Content).ToArray());

            // 解析图片
            foreach (var message in request.Inputs.Where(x => x.Contents != null).SelectMany(x => x.Contents)
                         .Where(x => x.Type is "input_image"))
            {
                var imageUrl = message.ImageUrl;
                if (imageUrl != null)
                {
                    try
                    {
                        var imageTokens = await CountImageTokens(message.ImageUrl, "low");
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
        }
        else if (rate.QuotaType == ModelQuotaType.OnDemand)
        {
            if (request?.Inputs != null)
            {
                requestToken += TokenHelper.GetTotalTokens(request?.Inputs?.Where(x => x.IsMessageArray)
                    .Where(x => x.Content != null)
                    .SelectMany(x => x.Contents)
                    .Where(x => x.Type == "input_text").Select(x => x.Text).ToArray());
            }
            else
            {
                requestToken += TokenHelper.GetTotalTokens(request?.Input);
            }

            var quota = requestToken * rate.PromptRate;

            // 判断请求token数量是否超过额度
            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");
        }

        // 是否第一次输出
        bool isFirst = true;
        await foreach (var (@event, item) in responsesService.GetResponsesAsync(request, platformOptions))
        {
            if (isFirst)
            {
                context.SetEventStreamHeaders();
                isFirst = false;
            }

            if (item?.Response?.Output != null)
            {
                foreach (var output in item.Response.Output)
                {
                    if (output.Content is { Length: > 0 })
                    {
                        // 计算输出的token数量
                        responseToken += TokenHelper.GetTotalTokens(output.Content.Select(x => x.Text).ToArray());
                    }
                }
            }


            responseToken = TokenHelper.GetTotalTokens(item?.Delta ?? string.Empty);

            if (@event.Equals("response.completed"))
            {
                if (item?.Response?.Usage?.InputTokens > 0)
                {
                    requestToken = item.Response.Usage.InputTokens;
                }

                if (item?.Response?.Usage?.OutputTokens > 0)
                {
                    responseToken = item.Response.Usage.OutputTokens;
                }

                if (item?.Response?.Usage?.InputTokensDetails?.CachedTokens > 0)
                {
                    cachedTokens = item.Response.Usage.InputTokensDetails.CachedTokens;
                }
            }

            await context.WriteAsEventStreamDataAsync(@event, item).ConfigureAwait(false);
        }

        if (rate.QuotaType == ModelQuotaType.OnDemand && request.Tools != null && request.Tools.Count != 0)
        {
            requestToken += TokenHelper.GetTotalTokens(request.Tools.Where(x => !string.IsNullOrEmpty(x?.Name))
                .Select(x => x!.Name).ToArray());
            requestToken += TokenHelper.GetTotalTokens(request.Tools
                .Where(x => !string.IsNullOrEmpty(x?.Description))
                .Select(x => x!.Description!).ToArray());
            requestToken += TokenHelper.GetTotalTokens(request.Tools.Where(x => !string.IsNullOrEmpty(x?.Type))
                .Select(x => x!.Type!).ToArray());
        }

        return (requestToken, responseToken, cachedTokens);
    }
}