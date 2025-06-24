using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
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

        return services;
    }
}