using AIDotNet.API.Service.Infrastructure;
using Microsoft.AspNetCore.Authentication;

namespace AIDotNet.API.Service;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册JWT Bearer认证服务的静态扩展方法
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
}