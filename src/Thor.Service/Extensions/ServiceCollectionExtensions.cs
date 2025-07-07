using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using Thor.Service.Infrastructure;
using Thor.Service.Options;
using Thor.Service.Infrastructure.Middlewares;

namespace Thor.Service.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册自定义认证
    /// </summary>
    /// <param Name="services"></param>
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        //使用应用密钥得到一个加密密钥字节数组
        services
            .AddAuthorization()
            .AddAuthentication("CustomAuthentication")
            .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomAuthentication", null);

        return services;
    }


    public static IEndpointRouteBuilder UseOpenTelemetry(this WebApplication app)
    {
        app.Use((async (context, next) =>
        {
            context.Response.Headers.TryAdd<string, StringValues>("X-Request-Id",
                (StringValues)Activity.Current?.TraceId.ToString());

            Activity.Current?.SetTag("http.method", context.Request.Method);
            Activity.Current?.SetTag("http.url", context.Request.Path);
            Activity.Current?.SetTag("http.host", context.Request.Host.ToString());

            await next(context);
        }));
        return app;
    }

    public static WebApplicationBuilder HostEnvironment(this WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(JwtOptions.Name)
            .Get<JwtOptions>();

        builder.Configuration.GetSection(CacheOptions.Name)
            .Get<CacheOptions>();
        builder.Configuration.GetSection(ChatCoreOptions.Name)
            .Get<ChatCoreOptions>();
        builder.Configuration.GetSection(TrackerOptions.Tracker)
            .Get<TrackerOptions>();

        var cacheType = Environment.GetEnvironmentVariable("CACHE_TYPE");
        var connectionString = Environment.GetEnvironmentVariable("CACHE_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(cacheType))
        {
            CacheOptions.Type = cacheType;
        }

        if (!string.IsNullOrEmpty(connectionString))
        {
            CacheOptions.ConnectionString = connectionString;
        }
        
        // var ConnectionStrings:DefaultConnection
        var defaultConnection = builder.Configuration["DefaultConnection"];
        var loggerConnection = builder.Configuration["LoggerConnection"];
        
        if (!string.IsNullOrEmpty(defaultConnection))
        {
            // 修改默认连接字符串
            builder.Configuration["ConnectionStrings:DefaultConnection"] = defaultConnection;
        }
        
        if (!string.IsNullOrEmpty(loggerConnection))
        {
            // 修改日志连接字符串
            builder.Configuration["ConnectionStrings:LoggerConnection"] = loggerConnection;
        }

        return builder;
    }

    /// <summary>
    /// 添加链路跟踪服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTracingService(this IServiceCollection services)
    {
        // 注册链路跟踪中间件
        services.AddTransient<TracingMiddleware>();
        
        // 确保 Activity 跟踪器已启用
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;
        
        return services;
    }

    /// <summary>
    /// 使用链路跟踪中间件
    /// </summary>
    /// <param name="app">应用程序</param>
    /// <returns>应用程序</returns>
    public static IApplicationBuilder UseTracingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TracingMiddleware>();
    }
}