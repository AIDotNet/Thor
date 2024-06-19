using Thor.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Thor.Claudia
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClaudia(this IServiceCollection services)
        {
            IApiChatCompletionService.ServiceNames.Add("Claudia", ClaudiaOptions.ServiceName);
            services.AddKeyedSingleton<IApiChatCompletionService, ClaudiaService>(ClaudiaOptions.ServiceName);
            return services;
        }
    }
}