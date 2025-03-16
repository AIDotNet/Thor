using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions.Chats;
using Thor.Abstractions;
using Thor.GoogleClaude;
using Thor.GCPClaude.Chats;

namespace Thor.GCPClaude.Extensions
{
    public static class GCPClaudeServiceCollectionExtensions
    {
        public static IServiceCollection AddGCPClaudeService(this IServiceCollection services)
        {
            ThorGlobal.PlatformNames.Add(GCPClaudePlatformOptions.PlatformName, GCPClaudePlatformOptions.PlatformCode);

            services.AddKeyedSingleton<IThorChatCompletionsService, GCPClaudeChatCompletionsService>(GCPClaudePlatformOptions.PlatformCode);
            return services;
        }
    }
}
