using AIDotNet.Abstractions;
using AIDotNet.OpenAI;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAIService(this IServiceCollection services)
    {
        IChatCompletionService.ServiceNames.Add("OpenAI", OpenAIServiceOptions.ServiceName);
        return services;
    }
}