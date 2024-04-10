using AIDotNet.Abstractions;
using AIDotNet.SparkDesk;
using Sdcb.SparkDesk;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSparkDeskService(this IServiceCollection services)
    {
        IChatCompletionService.ServiceNames.Add("星火大模型", SparkDeskOptions.ServiceName);

        return services;
    }
}