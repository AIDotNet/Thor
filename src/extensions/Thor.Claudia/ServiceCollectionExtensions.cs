using Thor.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Thor.Claudia
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClaudia(this IServiceCollection services)
        {
            ThorGlobal.PlatformNames.Add(ClaudiaPlatformOptions.PlatformName, ClaudiaPlatformOptions.PlatformCode);

            services.AddKeyedSingleton<IChatCompletionsService, ClaudiaChatCompletionsService>(ClaudiaPlatformOptions.PlatformCode);
            return services;
        }
    }
}