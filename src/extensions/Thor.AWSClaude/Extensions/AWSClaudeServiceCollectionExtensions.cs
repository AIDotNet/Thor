using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions.Chats;
using Thor.Abstractions;
using Thor.AWSClaude.Chats;

namespace Thor.AWSClaude.Extensions
{
    public static class AWSClaudeServiceCollectionExtensions
    {
        public static IServiceCollection AddAWSClaudeService(this IServiceCollection services)
        {
            ThorGlobal.PlatformNames.Add(AWSClaudePlatformOptions.PlatformName, AWSClaudePlatformOptions.PlatformCode);

            services.AddKeyedSingleton<IThorChatCompletionsService, AwsClaudeChatCompletionsService>(AWSClaudePlatformOptions.PlatformCode);
            return services;
        }
    }
}
