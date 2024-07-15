using Thor.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions.Chats;
using Thor.Claudia.Chats;

namespace Thor.Claudia.Extensions
{
    public static class ClaudiaServiceCollectionExtensions
    {
        public static IServiceCollection AddClaudia(this IServiceCollection services)
        {
            ThorGlobal.PlatformNames.Add(ClaudiaPlatformOptions.PlatformName, ClaudiaPlatformOptions.PlatformCode);

            services.AddKeyedSingleton<IThorChatCompletionsService, ClaudiaChatCompletionsService>(ClaudiaPlatformOptions.PlatformCode);
            return services;
        }
    }
}