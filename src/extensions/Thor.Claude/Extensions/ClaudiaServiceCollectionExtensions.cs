using Thor.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions.Chats;
using Thor.Claude.Chats;

namespace Thor.Claude.Extensions
{
    public static class ClaudiaServiceCollectionExtensions
    {
        public static IServiceCollection AddClaudiaService(this IServiceCollection services)
        {
            ThorGlobal.PlatformNames.Add(ClaudiaPlatformOptions.PlatformName, ClaudiaPlatformOptions.PlatformCode);

            services.AddKeyedSingleton<IThorChatCompletionsService, ClaudiaChatCompletionsService>(ClaudiaPlatformOptions.PlatformCode);
            return services;
        }
    }
}