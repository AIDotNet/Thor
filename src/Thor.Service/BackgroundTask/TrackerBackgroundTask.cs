using System.Diagnostics;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Tracker;
using Thor.Service.Options;

namespace Thor.Service.BackgroundTask;

public class TrackerBackgroundTask(
    ITrackerStorage trackerStorage,
    IHttpClientFactory httpClientFactory,
    ILogger<TrackerBackgroundTask> logger)
    : BackgroundService
{
    private const string Prompt =
        @"
1+1=？请直接给出答案，不要回复喝解释
";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (!TrackerOptions.Enable)
            {
                logger.LogWarning("没有启用Tracker");
                return;
            }

            await Task.Delay(10000, stoppingToken);

            var client = httpClientFactory.CreateClient("Tracker");

            var chatRequest = new ThorChatCompletionsRequest()
            {
                TopP = 0.7f,
                Temperature = 0.95f,
                MaxTokens = 50,
                Model = TrackerOptions.Model,
                Messages = [ThorChatMessage.CreateUserMessage(Prompt)]
            };

            client.DefaultRequestHeaders.Add("Authorization", TrackerOptions.ApiKey);

            while (!stoppingToken.IsCancellationRequested)
            {
                var sw = Stopwatch.StartNew();
                // 是否成功
                var success = false;

                var response = await client.PostAsJsonAsync(
                    TrackerOptions.Endpoint.TrimEnd('/') + "/v1/chat/completions",
                    chatRequest, stoppingToken);

                sw.Stop();

                if (response.IsSuccessStatusCode)
                {
                    var chatResponse =
                        await response.Content.ReadFromJsonAsync<ThorChatCompletionsResponse>(stoppingToken);

                    if (chatResponse?.Choices != null && chatResponse.Choices.Any())
                    {
                        var choice = chatResponse.Choices.FirstOrDefault();

                        if (string.IsNullOrWhiteSpace(choice?.Delta.Content))
                        {
                            success = false;
                        }
                        else
                        {
                            success = true;
                        }
                    }
                }

                if (success)
                {
                    logger.LogInformation($"TrackerBackgroundTask Success: {sw.ElapsedMilliseconds}ms");
                }
                else
                {
                    logger.LogWarning($"TrackerBackgroundTask Fail: {sw.ElapsedMilliseconds}ms");
                }

                // 根据超时时间计算服务器负载比例
                var (percentage, color) = CalculateLoadAvailabilityStatus(sw.ElapsedMilliseconds);
                var tracker = new TrackerDto
                {
                    Percentage = (int)(percentage),
                    Color = color,
                    Time = DateTime.Now,
                    Tooltip = $"{DateTime.Now:HH:mm}\n当前服务器负载 " + (percentage) + "% "
                };

                // 保存到数据库
                await trackerStorage.AddAsync(tracker);

                // 每一分钟检查一次
                await Task.Delay(1000 * 60, stoppingToken);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, $"TrackerBackgroundTask Error: {e.Message}");
        }
    }

    /// <summary>
    /// 计算负载可用性状态，包括百分比和对应的颜色。
    /// </summary>
    /// <param name="responseTimeMilliseconds">接口响应时间（毫秒）。</param>
    /// <param name="optimalResponseTimeMilliseconds">最佳响应时间，低于或等于此时间对应100%可用性。</param>
    /// <param name="midResponseTimeMilliseconds">中间响应时间，对应30%可用性。</param>
    /// <param name="maxResponseTimeMilliseconds">最大响应时间，对应0%可用性。</param>
    /// <returns>负载可用性状态。</returns>
    public (double, string) CalculateLoadAvailabilityStatus(
        double responseTimeMilliseconds,
        double optimalResponseTimeMilliseconds = 1000,
        double midResponseTimeMilliseconds = 10000,
        double maxResponseTimeMilliseconds = 20000)
    {
        double percentage;

        if (responseTimeMilliseconds <= optimalResponseTimeMilliseconds)
        {
            percentage = 100.0;
        }
        else if (responseTimeMilliseconds <= midResponseTimeMilliseconds)
        {
            // 从 1s (100%) 到 10s (30%) 线性下降
            double slope = (30.0 - 100.0) / (midResponseTimeMilliseconds - optimalResponseTimeMilliseconds);
            percentage = 100.0 + slope * (responseTimeMilliseconds - optimalResponseTimeMilliseconds);
        }
        else if (responseTimeMilliseconds <= maxResponseTimeMilliseconds)
        {
            // 从 10s (30%) 到 20s (0%) 线性下降
            double slope = (0.0 - 30.0) / (maxResponseTimeMilliseconds - midResponseTimeMilliseconds);
            percentage = 30.0 + slope * (responseTimeMilliseconds - midResponseTimeMilliseconds);
        }
        else
        {
            // 超过20s，保持0%
            percentage = 0.0;
        }

        // 确保百分比在0%到100%之间
        percentage = Math.Max(0.0, Math.Min(percentage, 100.0));

        string color = GetColorBasedOnPercentage(percentage);

        return new(Math.Round(percentage, 2), color);
    }

    /// <summary>
    /// 根据负载可用性百分比返回对应的颜色。
    /// </summary>
    /// <param name="percentage">负载可用性百分比。</param>
    /// <returns>颜色代码（十六进制）。</returns>
    private string GetColorBasedOnPercentage(double percentage)
    {
        if (percentage >= 90.0)
        {
            return "#00FF00"; // 绿色
        }

        if (percentage >= 60.0)
        {
            return "#FFFF00"; // 黄色
        }

        if (percentage >= 30.0)
        {
            return "#FFA500"; // 橙色
        }

        return "#FF0000"; // 红色
    }
}