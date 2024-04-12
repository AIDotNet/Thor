using AIDotNet.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Qiansail
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQiansail(this IServiceCollection services)
        {
            IApiChatCompletionService.ServiceNames.Add("通义千问（阿里云）", QiansailOptions.ServiceName);
            services.AddKeyedSingleton<IApiChatCompletionService, QiansailService>(QiansailOptions.ServiceName);
            return services;
        }
    }
}