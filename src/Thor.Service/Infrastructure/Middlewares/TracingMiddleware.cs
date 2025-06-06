using System.Diagnostics;
using Thor.Domain.Chats;
using Thor.Service.Infrastructure.Helper;

namespace Thor.Service.Infrastructure.Middlewares;

/// <summary>
/// 链路跟踪中间件
/// </summary>
public class TracingMiddleware : IMiddleware
{
    private static readonly ActivitySource ActivitySource = new("Thor.Service");
    
    static TracingMiddleware()
    {
        // 注册 ActivitySource 以确保其被监听
        ActivitySource.AddActivityListener(new ActivityListener
        {
            ActivityStarted = activity => 
            {
                Console.WriteLine($"Activity started: {activity.DisplayName}, ID: {activity.Id}");
            },
            ActivityStopped = activity => 
            {
                Console.WriteLine($"Activity stopped: {activity.DisplayName}, ID: {activity.Id}");
            },
            ShouldListenTo = source => source.Name.StartsWith("Thor"),
            Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllData
        });
    }

    /// <summary>
    /// 处理HTTP请求
    /// </summary>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 清除之前可能存在的链路信息
        TracingService.ClearCurrentTracing();
        
        // 创建一个根跟踪实体（无论是否有 Activity）
        var rootTracing = new Tracing
        {
            Id = Guid.NewGuid().ToString("N"),
            TraceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString(),
            Name = $"{context.Request.Method} {context.Request.Path}",
            StartTime = DateTime.UtcNow,
            ServiceName = "Thor.Service",
            Status = 0,
            Type = 0,
            Depth = 0,
            Attributes = new Dictionary<string, string>
            {
                ["http.method"] = context.Request.Method,
                ["http.url"] = context.Request.Path,
                ["http.host"] = context.Request.Host.ToString(),
                ["http.scheme"] = context.Request.Scheme
            }
        };
        
        // 如果有查询字符串，也添加
        if (context.Request.QueryString.HasValue)
        {
            rootTracing.Attributes["http.query_string"] = context.Request.QueryString.Value ?? string.Empty;
        }
        
        // 手动设置根跟踪实体
        TracingService.SetRootTracing(rootTracing);
        
        // 创建顶层Activity
        using var rootActivity = ActivitySource.StartActivity(
            $"{context.Request.Method} {context.Request.Path}",
            ActivityKind.Server);

        if (rootActivity != null)
        {
            // 设置Activity标签
            rootActivity.SetTag("http.method", context.Request.Method);
            rootActivity.SetTag("http.url", context.Request.Path);
            rootActivity.SetTag("http.host", context.Request.Host.ToString());
            rootActivity.SetTag("http.scheme", context.Request.Scheme);
            
            // 如果有查询字符串，也添加
            if (context.Request.QueryString.HasValue)
            {
                rootActivity.SetTag("http.query_string", context.Request.QueryString.Value);
            }
        }

        try
        {
            // 继续处理请求
            await next(context);

            // 设置响应状态码
            rootActivity?.SetTag("http.status_code", context.Response.StatusCode);
            rootTracing.Attributes["http.status_code"] = context.Response.StatusCode.ToString();
            
            // 如果是成功的请求，设置状态
            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 400)
            {
                rootActivity?.SetTag("status", "success");
                rootTracing.Status = 1; // 成功状态
            }
            else
            {
                rootActivity?.SetTag("status", "error");
                rootActivity?.SetTag("error.type", $"HTTP {context.Response.StatusCode}");
                rootTracing.Status = 2; // 错误状态
                rootTracing.ErrorMessage = $"HTTP {context.Response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            // 记录异常信息
            rootActivity?.SetTag("status", "error");
            rootActivity?.SetTag("error.type", ex.GetType().Name);
            rootActivity?.SetTag("error.message", ex.Message);
            rootActivity?.SetTag("error.stack_trace", ex.StackTrace);
            
            rootTracing.Status = 2; // 错误状态
            rootTracing.ErrorMessage = ex.Message;
            
            // 重新抛出异常，让上层中间件处理
            throw;
        }
        finally
        {
            // 手动结束根跟踪
            rootTracing.EndTime = DateTime.UtcNow;
            rootTracing.Duration = (long)(rootTracing.EndTime.Value - rootTracing.StartTime).TotalMilliseconds;
        }
    }
} 