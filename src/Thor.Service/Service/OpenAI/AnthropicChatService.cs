using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Thor.Abstractions.Anthropic;
using Thor.Abstractions.Exceptions;
using Thor.Domain.Chats;
using Thor.Infrastructure;
using Thor.Service.Domain.Core;
using Thor.Service.Extensions;
using Thor.Service.Infrastructure;

namespace Thor.Service.Service.OpenAI;

public class AnthropicChatService(
    IServiceProvider serviceProvider,
    ImageService imageService,
    TokenService tokenService,
    RateLimitModelService rateLimitModelService,
    ModelMapService modelMapService,
    ChannelService channelService,
    UserGroupService userGroupService,
    LoggerService loggerService,
    UserService userService,
    ILogger<AnthropicChatService> logger,
    ContextPricingService contextPricingService)
    : AIService(serviceProvider, imageService)
{
    public async Task MessageAsync(HttpContext context, AnthropicInput request)
    {
        using var chatCompletions =
            Activity.Current?.Source.StartActivity("Anthropic对话补全调用");

        var model = request.Model;
        Exception? lastException = null;

        var rateLimit = 0;

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

                if (channel == null && lastException != null)
                {
                    logger.LogError("对话模型请求异常：{lastException}，请求参数：{request}",
                        lastException, JsonSerializer.Serialize(request, ThorJsonSerializer.DefaultOptions));
                    await context.WriteErrorAsync(
                        lastException.Message, "400");
                    return;
                }

                if (channel == null)
                    throw new NotModelException(
                        $"{request.Model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

                var userGroup = await userGroupService.GetAsync(channel.Groups);

                if (userGroup == null)
                {
                    throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
                }

                ChannelAsyncLocal.ChannelIds.Add(channel.Id);

                // 获取渠道指定的实现类型的服务
                var chatCompletionsService = GetKeyedService<IAnthropicChatCompletionsService>(channel.Type);

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

                    (requestToken, responseToken, cachedTokens) =
                        await ChatCompletionsHandlerAsync(context, request, channel, chatCompletionsService, user,
                            rate);
                }

                // 计算上下文长度（使用请求token数作为近似值）
                var contextLength = requestToken;

                // 使用上下文定价服务计算费用
                var pricingResult =
                    contextPricingService.CalculatePricing(rate, requestToken, responseToken, contextLength);
                var quota = pricingResult.TotalCost;

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

                        await loggerService.CreateConsumeAsync("/v1/messages",
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
                        await loggerService.CreateConsumeAsync("/v1/messages",
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
                    await loggerService.CreateConsumeAsync("/v1/messages",
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
                await context.WriteErrorAsync($"当前{model}模型未设置倍率,请联系管理员设置倍率", "400");
            }
        }
        catch (ThorRateLimitException thorRateLimitException)
        {
            lastException = thorRateLimitException;
            logger.LogWarning("对话模型请求限流：{rateLimit}", rateLimit);
            rateLimit++;
            // TODO：限流重试次数
            if (rateLimit > 3)
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
            // 读取body
            logger.LogError("对话模型请求异常：{e} 准备重试{rateLimit}，请求参数：{request}", e, rateLimit,
                JsonSerializer.Serialize(request, ThorJsonSerializer.DefaultOptions));
            logger.LogError("对话模型请求异常：{e} 准备重试{rateLimit}，请求参数：{request}", e, rateLimit,
                JsonSerializer.Serialize(request, ThorJsonSerializer.DefaultOptions));
            rateLimit++;
            // TODO：限流重试次数
            if (rateLimit > 3)
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

    private async Task<(int requestToken, int responseToken, int cachedTokens)> ChatCompletionsHandlerAsync(
        HttpContext context, AnthropicInput input, ChatChannel channel,
        IAnthropicChatCompletionsService openService, User? user, ModelManager rate)
    {
        int requestToken = 0;
        int responseToken = 0;

        // 命中缓存tokens数量
        int cachedTokens = 0;

        var platformOptions = new ThorPlatformOptions(channel.Address, channel.Key, channel.Other);

        ClaudeChatCompletionDto result = null;

        if (!string.IsNullOrEmpty(input.System))
        {
            requestToken += TokenHelper.GetTotalTokens(input.System);
        }

        if (input.Systems?.Count > 0)
        {
            requestToken += TokenHelper.GetTotalTokens(input.Systems.Select(x => x.Text).ToArray());
        }

        if (input.Tools != null)
        {
            foreach (var tool in input.Tools)
            {
                requestToken += TokenHelper.GetTotalTokens(tool.name, tool.Description);
                requestToken += TokenHelper.GetTotalTokens(tool.InputSchema.Required ?? []);
                requestToken += TokenHelper.GetTotalTokens(JsonSerializer.Serialize(tool.InputSchema.Properties));
            }
        }

        // 这里应该用其他的方式来判断是否是vision模型，目前先这样处理
        if (rate.QuotaType == ModelQuotaType.OnDemand && input.Messages.Any(x => x.Contents != null))
        {
            foreach (var content in input?.Messages.Where(x => x.Contents != null)
                         .SelectMany(x => x.Contents))
            {
                requestToken += TokenHelper.GetTotalTokens(content.Text ?? string.Empty);
                if (content.Content != null)
                {
                    requestToken += TokenHelper.GetTotalTokens(JsonSerializer.Serialize(content.Content));
                }

                if (content.Input != null)
                {
                    requestToken += TokenHelper.GetTotalTokens(JsonSerializer.Serialize(content.Input));
                }
            }

            requestToken += TokenHelper.GetTotalTokens(input.Messages.Where(x => x.Contents == null)
                .Select(x => x.Content).ToArray());

            // 解析图片
            foreach (var message in input.Messages.Where(x => x.Contents != null).SelectMany(x => x.Contents)
                         .Where(x => x.Type is "image" or "image_url"))
            {
                var imageUrl = message.Source;
                if (imageUrl != null)
                {
                    var url = imageUrl.Data;

                    try
                    {
                        var imageTokens = await CountImageTokens(url, "high");
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

            result = await openService.ChatCompletionsAsync(input, platformOptions);

            await context.Response.WriteAsJsonAsync(result);

            if (result?.Usage?.input_tokens is not null && result.Usage.input_tokens > 0)
            {
                requestToken = result.Usage.input_tokens.Value;
            }

            // 如果存在返回的Usage则使用返回的Usage中的CompletionTokens
            if (result?.Usage?.output_tokens is not null && result.Usage.output_tokens > 0)
            {
                responseToken = result.Usage.output_tokens.Value;
            }
            else
            {
                responseToken =
                    TokenHelper.GetTotalTokens(result?.content?.Select(x => x.text).ToArray() ?? []);
            }
        }
        else if (rate.QuotaType == ModelQuotaType.OnDemand)
        {
            var contentArray = input.Messages.Select(x => x.Content).ToArray();
            requestToken = TokenHelper.GetTotalTokens(contentArray);

            var quota = requestToken * rate.PromptRate;

            // 判断请求token数量是否超过额度
            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            result = await openService.ChatCompletionsAsync(input, platformOptions);

            await context.Response.WriteAsJsonAsync(result);
        }
        else
        {
            result = await openService.ChatCompletionsAsync(input, platformOptions);

            await context.Response.WriteAsJsonAsync(result);
        }

        if (result?.Usage?.input_tokens is not null && result.Usage.input_tokens > 0)
        {
            requestToken = result.Usage.input_tokens.Value;
        }

        if (result?.Usage?.output_tokens is not null && result.Usage.output_tokens > 0)
        {
            responseToken = result.Usage.output_tokens.Value;
        }
        else
        {
            responseToken += TokenHelper.GetTotalTokens(result?.content?.Select(x => x.text).ToArray() ?? []);
            responseToken += TokenHelper.GetTotalTokens(result?.content?.Select(x => x.Thinking).ToArray() ?? []);
        }

        if (result?.Usage?.cache_read_input_tokens.HasValue == true)
        {
            cachedTokens = result.Usage.cache_read_input_tokens.Value;
        }


        return (requestToken, responseToken, cachedTokens);
    }

    private async Task<(int requestToken, int responseToken)> StreamChatCompletionsHandlerAsync(HttpContext context,
        AnthropicInput input, ChatChannel channel, IAnthropicChatCompletionsService openService, User? user,
        ModelManager rate)
    {
        int requestToken = 0;

        var platformOptions = new ThorPlatformOptions(channel.Address, channel.Key, channel.Other);

        var responseMessage = new StringBuilder();

        requestToken += TokenHelper.GetTotalTokens(input?.System);
        requestToken += TokenHelper.GetTotalTokens(input?.Systems?.Select(x => x.Text).ToArray() ?? []);

        if (!string.IsNullOrEmpty(input.System))
        {
            requestToken += TokenHelper.GetTotalTokens(input.System);
        }

        if (input.Systems?.Count > 0)
        {
            requestToken += TokenHelper.GetTotalTokens(input.Systems.Select(x => x.Text).ToArray());
        }

        if (input.Tools != null)
        {
            foreach (var tool in input.Tools)
            {
                requestToken += TokenHelper.GetTotalTokens(tool.name, tool.Description);
                requestToken += TokenHelper.GetTotalTokens(tool.InputSchema.Required ?? []);
                requestToken += TokenHelper.GetTotalTokens(JsonSerializer.Serialize(tool.InputSchema.Properties));
            }
        }

        if (input.Messages.Any(x => x.Contents != null) && rate.QuotaType == ModelQuotaType.OnDemand)
        {
            foreach (var content in input?.Messages.Where(x => x.Contents != null)
                         .SelectMany(x => x.Contents))
            {
                requestToken += TokenHelper.GetTotalTokens(content.Text ?? string.Empty);
                if (content.Content != null)
                {
                    requestToken += TokenHelper.GetTotalTokens(JsonSerializer.Serialize(content.Content));
                }

                if (content.Input != null)
                {
                    requestToken += TokenHelper.GetTotalTokens(JsonSerializer.Serialize(content.Input));
                }
            }

            requestToken += TokenHelper.GetTotalTokens(input.Messages.Where(x => x.Contents == null)
                .Select(x => x.Content).ToArray());

            requestToken += TokenHelper.GetTotalTokens(input.Messages.Where(x => x.Contents == null)
                .Select(x => x.Content).ToArray());

            foreach (var message in input.Messages.Where(x => x is { Contents: not null }).SelectMany(x => x.Contents)
                         .Where(x => x.Type is "image" or "image_url"))
            {
                var imageUrl = message.Source;
                if (imageUrl == null) continue;
                var url = imageUrl.MediaType;
                var detail = "";
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

        // if (rate.QuotaType == ModelQuotaType.OnDemand && input.ResponseFormat?.JsonSchema is not null)
        // {
        //     requestToken += TokenHelper.GetTotalTokens(input.ResponseFormat.JsonSchema.Name,
        //         input.ResponseFormat.JsonSchema.Description ?? string.Empty,
        //         JsonSerializer.Serialize(input.ResponseFormat.JsonSchema.Schema));
        // }

        // 是否第一次输出
        bool isFirst = true;
        int responseToken = 0;

        await foreach (var (@event, item) in openService.StreamChatCompletionsAsync(input, platformOptions))
        {
            if (isFirst)
            {
                context.SetEventStreamHeaders();
                isFirst = false;
            }

            if (item?.Usage is { output_tokens: > 0 } || item?.message?.Usage?.output_tokens is not null &&
                item.message.Usage.output_tokens > 0)
            {
                responseToken = item.Usage?.output_tokens ?? item.message?.Usage?.output_tokens ?? 0;
            }
            else
            {
                responseToken +=
                    TokenHelper.GetTotalTokens(item?.delta?.partial_json ?? item?.delta?.text ?? item?.delta?.thinking
                        ?? item?.message?.content?.FirstOrDefault()?.text ?? string.Empty);

                if (!string.IsNullOrEmpty(item?.content_block?.thinking))
                {
                    responseToken += TokenHelper.GetTotalTokens(item.content_block.thinking);
                }
            }

            responseMessage.Append(item?.delta?.text ?? item?.message?.content?.FirstOrDefault()?.text);
            if (item == null && !string.IsNullOrEmpty(@event))
            {
                await context.WriteAsEventStreamAsync(@event).ConfigureAwait(false);
                continue;
            }
            else if (item != null && !string.IsNullOrEmpty(@event))
            {
                if (@event.Equals("message_start", StringComparison.CurrentCulture))
                {
                    item.Usage ??= new ClaudeChatCompletionDtoUsage();
                    item.Usage.input_tokens = requestToken;
                }
                
                await context.WriteAsEventStreamDataAsync(@event, item).ConfigureAwait(false);
                continue;
            }

            if (item == null)
            {
                continue;
            }

            if (item?.Usage is { input_tokens: > 0 } || item?.message?.Usage is { input_tokens: > 0 })
            {
                requestToken = item.Usage?.input_tokens ?? item.message.Usage.input_tokens ?? 0;
            }

            if (@event.StartsWith("data:"))
            {
                await context.WriteAsEventAsync(@event + "\n\n").ConfigureAwait(false);
            }
            else
            {
                await context.WriteAsEventAsync(@event).ConfigureAwait(false);
            }
        }

        responseToken = rate.QuotaType switch
        {
            ModelQuotaType.OnDemand when responseToken == 0 => TokenHelper.GetTokens(responseMessage.ToString()),
            ModelQuotaType.ByCount => rate.QuotaType == ModelQuotaType.OnDemand
                ? TokenHelper.GetTokens(responseMessage.ToString())
                : 0,
            _ => responseToken
        };

        return (requestToken, responseToken);
    }
}