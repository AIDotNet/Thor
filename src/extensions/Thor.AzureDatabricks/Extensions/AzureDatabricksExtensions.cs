using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Anthropic;
using Thor.Abstractions.Chats;
using Thor.AzureDatabricks.Chats;

namespace Thor.AzureDatabricks.Extensions;

public static class AzureDatabricksExtensions
{
    public static IServiceCollection AddAzureDatabricksPlatform(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(AzureDatabricksPlatformOptions.PlatformName,
            AzureDatabricksPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, AzureDatabricksChatCompletionsService>(
            AzureDatabricksPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IAnthropicChatCompletionsService>(AzureDatabricksPlatformOptions
                .PlatformCode,
            ((provider, o) => new AzureDatabricksAnthropicChatCompletionsService(
                provider.GetKeyedService<IThorChatCompletionsService>(AzureDatabricksPlatformOptions.PlatformCode),
                provider.GetRequiredService<ILogger<AzureDatabricksAnthropicChatCompletionsService>>())));

        return services;
    }
}