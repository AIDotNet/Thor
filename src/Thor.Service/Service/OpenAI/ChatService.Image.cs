using System.Diagnostics;
using System.Text.Json;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Images;
using Thor.Abstractions.Images.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Infrastructure;
using Thor.Service.Domain.Core;
using Thor.Service.Extensions;

namespace Thor.Service.Service.OpenAI;

partial class ChatService
{
    public async Task EditsImageAsync(HttpContext context)
    {
        using var image =
            Activity.Current?.Source.StartActivity("图片修改图片");

        int count = 0;
        var request = new ImageEditCreateRequest();
        Exception? lastException = null;

        rateLimit:

        try
        {
            var organizationId = string.Empty;
            if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
            {
                organizationId = organizationIdHeader.ToString();
            }

            var requestToken = 0;
            // 读取表单数据
            if (context.Request.HasFormContentType && request.Image == null)
            {
                var form = await context.Request.ReadFormAsync(context.RequestAborted);
                request.Prompt = form["prompt"];
                request.N = int.TryParse(form["n"], out var n) ? n : 1;
                request.Size = form["size"];
                request.Model = form["model"];
                request.ResponseFormat = form["response_format"];
                request.Quality = form["quality"];
                request.Background = form["background"];
                request.OutputCompression = form["output_compression"];
                request.OutputFormat = form["output_format"];
                request.Moderation = form["moderation"];

                // 文件stream转换byte

                var imageFile = form.Files.GetFile("image");
                if (imageFile != null)
                {
                    await using var stream = new MemoryStream();
                    await imageFile.CopyToAsync(stream, context.RequestAborted);
                    request.Image = stream.ToArray();
                    request.ImageName = imageFile.FileName;

                    // 计算图片token
                    var (t, e) = CountImageTokens(request.Image, request.Quality);
                    requestToken += t * 2;
                }
                else
                {
                    throw new BusinessException("image是必须的", "400");
                }

                // 先判断Mask是否
                var mask = form.Files.GetFile("mask");

                if (mask != null)
                {
                    await using var stream = new MemoryStream();
                    await mask.CopyToAsync(stream, context.RequestAborted);
                    request.Mask = stream.ToArray();
                    request.MaskName = mask.FileName;
                }
            }

            var model = request.Model;

            if (string.IsNullOrEmpty(request?.Model)) model = "dall-e-2";

            var rate = ModelManagerService.PromptRate[model];

            var (token, user) = await tokenService.CheckTokenAsync(context, rate);

            await rateLimitModelService.CheckAsync(model, user.Id);
            TokenService.CheckModel(model, token, context);

            model = await modelMapService.ModelMap(model);


            request.N ??= 1;

            // 获取渠道 通过算法计算权重
            var channel =
                CalculateWeight(await channelService.GetChannelsContainsModelAsync(model, user, token));

            if (lastException == null && channel == null)
                throw new NotModelException(
                    $"{model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

            if (lastException != null && channel == null)
            {
                context.Response.StatusCode = 500;
                await context.WriteOpenAIErrorAsync(lastException.Message);
                return;
            }

            var userGroup = await userGroupService.GetAsync(channel.Groups);

            if (userGroup == null)
            {
                throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
            }


            request.N ??= 1;
            // 
            //     Quality	Square (1024×1024)	Portrait (1024×1536)	Landscape (1536×1024)
            //     Low	272 tokens	408 tokens	400 tokens
            //     Medium	1056 tokens	1584 tokens	1568 tokens
            //     High	4160 tokens	6240 tokens	6208 tokens
            requestToken += TokenHelper.GetTotalTokens(request.Prompt ?? string.Empty);
            var responseToken = 0;
            if (request.Quality?.Equals("low", StringComparison.OrdinalIgnoreCase) == true &&
                request.Size == "1024x1024")
            {
                responseToken += 272;
            }
            else if (request.Quality?.Equals("medium", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1024")
            {
                responseToken += 1056;
            }
            else if (request.Quality?.Equals("high", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1024")
            {
                responseToken += 4160;
            }
            else if (request.Quality?.Equals("low", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1536")
            {
                responseToken += 408;
            }
            else if (request.Quality?.Equals("medium", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1536")
            {
                responseToken += 1584;
            }
            else if (request.Quality?.Equals("high", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1536")
            {
                responseToken += 6240;
            }
            else if (request.Quality?.Equals("low", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1536x1024")
            {
                responseToken += 400;
            }
            else if (request.Quality?.Equals("medium", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1536x1024")
            {
                responseToken += 1568;
            }
            else if (request.Quality?.Equals("high", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1536x1024")
            {
                responseToken += 6208;
            }
            else
            {
                if (request.Size == "1024x1024")
                {
                    responseToken += 4160;
                }
                else if (request.Size == "1024x1536")
                {
                    responseToken += 6240;
                }
                else if (request.Size == "1536x1024")
                {
                    responseToken += 6208;
                }
            }

            var quota = requestToken * rate.PromptRate;

            var completionRatio = rate.CompletionRate ?? GetCompletionRatio(model);
            quota += (responseToken) * rate.PromptRate * completionRatio;

            quota = (decimal)userGroup.Rate * quota;

            // 将quota 四舍五入
            quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);


            // 获取渠道指定的实现类型的服务
            var openService = GetKeyedService<IThorImageService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");

            var sw = Stopwatch.StartNew();

            var response = await openService.CreateImageEdit(request, new ThorPlatformOptions
            {
                ApiKey = channel.Key,
                Address = channel.Address,
                Other = channel.Other
            });

            if (response.Error != null || response.Results.Count == 0)
            {
                logger.LogError("图片修改失败：{error}", JsonSerializer.Serialize(response.Error));
                throw new BusinessException(response.Error?.Message ?? "图片修改失败", response.Error?.Code?.ToString());
            }

            await context.Response.WriteAsJsonAsync(response);

            sw.Stop();

            if (response.Usage is { InputTokens: not null })
            {
                quota = response.Usage.InputTokens.Value * rate.PromptRate;

                completionRatio = rate.CompletionRate ?? GetCompletionRatio(model);
                quota += (response.Usage.OutputTokens ?? responseToken) * rate.PromptRate * completionRatio;

                quota = (decimal)userGroup.Rate * quota;

                // 将quota 四舍五入
                quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                if (request == null) throw new Exception("模型校验异常");

                if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");
            }

            // 判断是否按次
            if (rate.QuotaType == ModelQuotaType.ByCount)
            {
                quota = (decimal)(rate.PromptRate) * (decimal)userGroup.Rate;

                await loggerService.CreateConsumeAsync("/v1/images/edits",
                    string.Format(ConsumerImageTemplate, RenderHelper.RenderQuota(rate.PromptRate),
                        userGroup.Rate),
                    request.Model,
                    requestToken, responseToken, (int)((int)quota), token?.Key,
                    user?.UserName, user?.Id,
                    channel.Id,
                    channel.Name, context.GetIpAddress(), context.GetUserAgent(),
                    false,
                    (int)sw.ElapsedMilliseconds, organizationId);

                await userService.ConsumeAsync(user!.Id, (long)rate.PromptRate, requestToken, token?.Key,
                    channel.Id,
                    request.Model);
            }
            else
            {
                await loggerService.CreateConsumeAsync("/v1/images/edits",
                    string.Format(ConsumerTemplateOnDemand, rate.PromptRate, userGroup.Rate),
                    model,
                    0, 0, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                    channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                    organizationId);

                await userService.ConsumeAsync(user!.Id, (int)quota, 0, token?.Key, channel.Id, model);
            }
        }
        catch (PaymentRequiredException)
        {
            context.Response.StatusCode = 402;
            await context.WriteOpenAIErrorAsync("账号余额不足请充值", "402");
        }
        catch (RateLimitException)
        {
            lastException = new RateLimitException("请求过于频繁，请稍后再试");
            if (count > 5)
            {
                context.Response.StatusCode = 429;
                await context.WriteOpenAIErrorAsync("请求过于频繁，请稍后再试", "429");
                return;
            }

            count++;
            goto rateLimit;
        }
        catch (NotModelException model)
        {
            context.Response.StatusCode = 400;
            await context.WriteOpenAIErrorAsync(model.Message, "400");
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (Exception e)
        {
            lastException = e;
            if (count > 5)
            {
                context.Response.StatusCode = 500;
                logger.LogError("图片修改请求异常：{e}", e);
                await context.WriteOpenAIErrorAsync(e.Message);
                return;
            }

            count++;

            goto rateLimit;
        }
    }


    public async Task CreateImageAsync(HttpContext context, ImageCreateRequest request)
    {
        using var image =
            Activity.Current?.Source.StartActivity("文字生成图片");

        var organizationId = string.Empty;
        if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
        {
            organizationId = organizationIdHeader.ToString();
        }

        int count = 0;
        Exception? lastException = null;

        rateLimit:


        try
        {
            var model = request.Model;


            if (string.IsNullOrEmpty(model)) model = "dall-e-2";

            var rate = ModelManagerService.PromptRate[model];

            var (token, user) = await tokenService.CheckTokenAsync(context, rate);

            await rateLimitModelService.CheckAsync(model, user.Id);
            TokenService.CheckModel(model, token, context);

            model = await modelMapService.ModelMap(model);

            // 获取渠道 通过算法计算权重
            var channel =
                CalculateWeight(await channelService.GetChannelsContainsModelAsync(model, user, token));

            if (lastException == null && channel == null)
                throw new NotModelException(
                    $"{model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

            if (lastException != null && channel == null)
            {
                context.Response.StatusCode = 500;
                await context.WriteOpenAIErrorAsync(lastException.Message);
                return;
            }

            var userGroup = await userGroupService.GetAsync(channel.Groups);

            if (userGroup == null)
            {
                throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
            }

            request.N ??= 1;

            var requestToken = TokenHelper.GetTotalTokens(request.Prompt ?? string.Empty);
            var responseToken = 0;
            if (request.Quality?.Equals("low", StringComparison.OrdinalIgnoreCase) == true &&
                request.Size == "1024x1024")
            {
                responseToken += 272;
            }
            else if (request.Quality?.Equals("medium", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1024")
            {
                responseToken += 1056;
            }
            else if (request.Quality?.Equals("high", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1024")
            {
                responseToken += 4160;
            }
            else if (request.Quality?.Equals("low", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1536")
            {
                responseToken += 408;
            }
            else if (request.Quality?.Equals("medium", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1536")
            {
                responseToken += 1584;
            }
            else if (request.Quality?.Equals("high", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1024x1536")
            {
                responseToken += 6240;
            }
            else if (request.Quality?.Equals("low", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1536x1024")
            {
                responseToken += 400;
            }
            else if (request.Quality?.Equals("medium", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1536x1024")
            {
                responseToken += 1568;
            }
            else if (request.Quality?.Equals("high", StringComparison.OrdinalIgnoreCase) == true &&
                     request.Size == "1536x1024")
            {
                responseToken += 6208;
            }
            else
            {
                if (request.Size == "1024x1024")
                {
                    responseToken += 272;
                }
                else if (request.Size == "1024x1536")
                {
                    responseToken += 408;
                }
                else if (request.Size == "1536x1024")
                {
                    responseToken += 400;
                }
            }

            var quota = requestToken * rate.PromptRate;

            var completionRatio = rate.CompletionRate ?? GetCompletionRatio(model);
            quota += responseToken * rate.PromptRate * completionRatio;

            quota = (decimal)userGroup.Rate * quota;

            // 将quota 四舍五入
            quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

            if (request == null) throw new Exception("模型校验异常");

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");

            // 获取渠道指定的实现类型的服务
            var openService = GetKeyedService<IThorImageService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");

            // 记录请求信息
            logger.LogInformation($"分配渠道：{channel.Name}, 地址：{channel.Address}, 模型：{model}");

            var sw = Stopwatch.StartNew();

            var response = await openService.CreateImage(request, new ThorPlatformOptions
            {
                ApiKey = channel.Key,
                Address = channel.Address,
                Other = channel.Other
            });

            if (response.Error != null || response.Results.Count == 0)
            {
                logger.LogError("图片生成失败：{error}", JsonSerializer.Serialize(response.Error));
                throw new BusinessException(response.Error?.Message ?? "图片生成失败", response.Error?.Code?.ToString());
            }

            // 根据请求格式转换，如果请求格式是base64，但是返回格式是url，则需要转换
            if (request.ResponseFormat?.Equals("b64_json", StringComparison.OrdinalIgnoreCase) == true &&
                response.Results.Any(x => string.IsNullOrEmpty(x.B64)))
            {
                var client = HttpClientFactory.GetHttpClient("default");
                foreach (var dataResult in response.Results.Where(dataResult => string.IsNullOrEmpty(dataResult.B64) && !string.IsNullOrEmpty(dataResult.Url)))
                {
                    try
                    {
                        var imageBytes = await client.GetByteArrayAsync(dataResult.Url);
                        dataResult.B64 = Convert.ToBase64String(imageBytes);
                        dataResult.Url = null;
                    }
                    catch (Exception e)
                    {
                        logger.LogError("图片转换失败：{e}", e);
                    }
                }
            }

            await context.Response.WriteAsJsonAsync(response);

            sw.Stop();

            if (response.Usage is { InputTokens: not null })
            {
                quota = response.Usage.InputTokens.Value * rate.PromptRate;

                completionRatio = rate.CompletionRate ?? GetCompletionRatio(model);
                quota += (response.Usage.OutputTokens ?? responseToken) * rate.PromptRate * completionRatio;

                quota = (decimal)userGroup.Rate * quota;

                // 将quota 四舍五入
                quota = Math.Round(quota, 0, MidpointRounding.AwayFromZero);

                requestToken = response.Usage.InputTokens.Value;
                responseToken = response.Usage.OutputTokens ?? responseToken;

                if (request == null) throw new Exception("模型校验异常");

                if (quota > user.ResidualCredit) throw new InsufficientQuotaException("账号余额不足请充值");
            }

            // 判断是否按次
            if (rate.QuotaType == ModelQuotaType.ByCount)
            {
                quota = (decimal)(rate.PromptRate) * (decimal)userGroup.Rate;

                await loggerService.CreateConsumeAsync("/v1/images/generations",
                    string.Format(ConsumerImageTemplate, rate.PromptRate, userGroup.Rate),
                    model,
                    requestToken, responseToken, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                    channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                    organizationId);

                await userService.ConsumeAsync(user!.Id, (long)quota, 0, token?.Key, channel.Id, model);
            }
            else
            {
                await loggerService.CreateConsumeAsync("/v1/images/generations",
                    string.Format(ConsumerTemplateOnDemand, rate.PromptRate, userGroup.Rate),
                    model,
                    0, 0, (int)quota, token?.Key, user?.UserName, user?.Id, channel.Id,
                    channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                    organizationId);

                await userService.ConsumeAsync(user!.Id, (int)quota, 0, token?.Key, channel.Id, model);
            }
        }
        catch (PaymentRequiredException)
        {
            context.Response.StatusCode = 402;
            await context.WriteOpenAIErrorAsync("账号余额不足请充值", "402");
        }
        catch (RateLimitException)
        {
            context.Response.StatusCode = 429;
            lastException ??= new RateLimitException("请求过于频繁，请稍后再试");

            if (count > 3)
            {
                context.Response.StatusCode = 500;
                await context.WriteOpenAIErrorAsync("请求过于频繁，请稍后再试", "429");
                return;
            }

            count++;
            goto rateLimit;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (NotModelException model)
        {
            context.Response.StatusCode = 400;
            await context.WriteOpenAIErrorAsync(model.Message, "400");
        }
        catch (Exception e)
        {
            lastException = e;
            if (count > 3)
            {
                context.Response.StatusCode = 500;
                logger.LogError("图片生成请求异常：{e}", e);
                await context.WriteOpenAIErrorAsync(e.Message);
                return;
            }

            count++;

            goto rateLimit;
        }
    }


    public async Task VariationsAsync(HttpContext context)
    {
        using var image =
            Activity.Current?.Source.StartActivity("图片变体");

        var organizationId = string.Empty;
        if (context.Request.Headers.TryGetValue("OpenAI-Organization", out var organizationIdHeader))
        {
            organizationId = organizationIdHeader.ToString();
        }

        var request = new ImageVariationCreateRequest();

        int count = 0;
        rateLimit:

        try
        {
            // 读取表单数据
            if (context.Request.HasFormContentType && request.Image == null)
            {
                var form = await context.Request.ReadFormAsync(context.RequestAborted);
                request.N = int.TryParse(form["n"], out var n) ? n : 1;
                request.Size = form["size"];
                request.Model = form["model"];
                request.ResponseFormat = form["response_format"];

                // 文件stream转换byte

                var imageFile = form.Files.GetFile("image");
                if (imageFile != null)
                {
                    await using var stream = new MemoryStream();
                    await imageFile.CopyToAsync(stream, context.RequestAborted);
                    request.Image = stream.ToArray();
                    request.ImageName = imageFile.FileName;
                }
            }

            var model = request.Model;


            if (string.IsNullOrEmpty(request?.Model)) model = "dall-e-2";

            var imageCostRatio = GetImageCostRatio(model, request.Size);

            var rate = ModelManagerService.PromptRate[model];

            var (token, user) = await tokenService.CheckTokenAsync(context, rate);

            await rateLimitModelService.CheckAsync(model, user.Id);
            TokenService.CheckModel(model, token, context);

            model = await modelMapService.ModelMap(model);

            request.N ??= 1;

            var quota = (int)(rate.PromptRate * imageCostRatio) * request.N;

            if (request == null) throw new Exception("模型校验异常");

            if (quota > user.ResidualCredit) throw new InsufficientQuotaException("您的账号钱包余额不足请在控制台充值。");

            // 获取渠道 通过算法计算权重
            var channel =
                CalculateWeight(await channelService.GetChannelsContainsModelAsync(model, user, token));

            if (channel == null)
                throw new NotModelException(
                    $"{model}在分组：{(token?.Groups.FirstOrDefault() ?? user.Groups.FirstOrDefault())} 未找到可用渠道");

            var userGroup = await userGroupService.GetAsync(channel.Groups);

            if (userGroup == null)
            {
                throw new BusinessException("当前渠道未设置分组，请联系管理员设置分组", "400");
            }

            // 获取渠道指定的实现类型的服务
            var openService = GetKeyedService<IThorImageService>(channel.Type);

            if (openService == null) throw new Exception($"并未实现：{channel.Type} 的服务");

            var sw = Stopwatch.StartNew();

            var response = await openService.CreateImageVariation(request, new ThorPlatformOptions
            {
                ApiKey = channel.Key,
                Address = channel.Address,
                Other = channel.Other
            });

            // 计算createdAT
            var createdAt = DateTime.Now;
            var created = (int)createdAt.ToUnixTimeSeconds();
            await context.Response.WriteAsJsonAsync(new ThorImageCreateResponse
            {
                data = response.Results,
                created = created,
                successful = response.Successful
            });

            sw.Stop();

            quota = (int)((decimal)userGroup.Rate * quota);

            await loggerService.CreateConsumeAsync("/v1/images/variations",
                string.Format(ConsumerTemplate, rate.PromptRate, 0, userGroup.Rate),
                model,
                0, 0, quota ?? 0, token?.Key, user?.UserName, user?.Id, channel.Id,
                channel.Name, context.GetIpAddress(), context.GetUserAgent(), false, (int)sw.ElapsedMilliseconds,
                organizationId);

            await userService.ConsumeAsync(user!.Id, quota ?? 0, 0, token?.Key, channel.Id, model);
        }
        catch (PaymentRequiredException)
        {
            context.Response.StatusCode = 402;
            await context.WriteOpenAIErrorAsync("账号余额不足请充值", "402");
        }
        catch (RateLimitException)
        {
            if (count > 3)
            {
                context.Response.StatusCode = 429;
                await context.WriteOpenAIErrorAsync("请求过于频繁，请稍后再试", "429");
                return;
            }

            count++;
            goto rateLimit;
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
        }
        catch (Exception e)
        {
            if (count > 3)
            {
                context.Response.StatusCode = 500;
                logger.LogError("对话模型请求异常：{e}", e);
                await context.WriteOpenAIErrorAsync(e.Message);
                return;
            }

            count++;
            goto rateLimit;
        }
    }
}