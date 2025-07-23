using Thor.Domain.Chats;
using Thor.Service.Infrastructure;

namespace Thor.Service.Service;

public abstract class AIService(IServiceProvider serviceProvider, ImageService imageService)
    : ApplicationService(serviceProvider)
{
    /// <summary>
    ///  按量计费模型倍率模板
    /// </summary>
    protected const string ConsumerTemplate = "模型倍率：{0} 补全倍率：{1} 分组倍率：{2}";


    /// <summary>
    ///  按量计费模型倍率模板
    /// </summary>
    protected const string ConsumerImageTemplate = "模型倍率：{0}  分组倍率：{1}";

    /// <summary>
    /// 按量计费命中缓存模型
    /// </summary>
    protected const string ConsumerTemplateCache = "模型倍率：{0} 补全倍率：{1} 分组倍率：{2} 命中缓存tokens {3} 缓存倍率：{4}";

    protected const string ConsumerTemplateCacheWriteTokens =
        "模型倍率：{0} 补全倍率：{1} 分组倍率：{2} 命中缓存tokens {3} 缓存倍率：{4} 写入缓存tokens {5}";

    /// <summary>
    /// 按次计费模型倍率模板
    /// </summary>
    protected const string ConsumerTemplateOnDemand = "按次数计费费用：{0} 分组倍率：{1}";

    /// <summary>
    /// 实时对话计费模型倍率模板
    /// </summary>
    protected const string RealtimeConsumerTemplate =
        "模型倍率：文本提示词倍率:{0} 文本完成倍率:{1} 音频请求倍率:{2} 音频完成倍率:{3}  实时对话 分组倍率：{4}";


    protected static readonly Dictionary<string, Dictionary<string, double>> ImageSizeRatios = new()
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


    /// <summary>
    /// 权重算法
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    protected static ChatChannel? CalculateWeight(IEnumerable<ChatChannel> channel)
    {
        var chatChannels = channel.ToList();
        if (chatChannels.Count == 0)
        {
            return null;
        }

        // 所有权重值之和
        var total = chatChannels.Sum(x => x.Order);

        var value = Convert.ToInt32(Random.Shared.NextDouble() * total);

        foreach (var chatChannel in chatChannels)
        {
            value -= chatChannel.Order;
            if (value > 0) continue;

            ChannelAsyncLocal.ChannelIds.Add(chatChannel.Id);
            return chatChannel;
        }

        var v = chatChannels.Last();

        ChannelAsyncLocal.ChannelIds.Add(v.Id);

        return v;
    }

    /// <summary>
    ///     对话模型补全倍率
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected decimal GetCompletionRatio(string name)
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
    /// 计算图片token
    /// </summary>
    /// <param name="url"></param>
    /// <param name="detail"></param>
    /// <returns></returns>
    protected Tuple<int, Exception> CountImageTokens(ReadOnlyMemory<byte> url, string detail)
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
                        (width, height) = imageService.GetImageSizeFromUrlAsync(url);
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

    /// <summary>
    /// 计算图片token
    /// </summary>
    /// <param name="url"></param>
    /// <param name="detail"></param>
    /// <returns></returns>
    protected async ValueTask<Tuple<int, Exception>> CountImageTokens(string url, string detail)
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