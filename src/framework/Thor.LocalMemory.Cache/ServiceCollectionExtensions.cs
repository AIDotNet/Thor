using Microsoft.Extensions.DependencyInjection;
using Thor.BuildingBlocks.Cache;

namespace Thor.LocalMemory.Cache;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalMemory(this IServiceCollection services)
    {
        services.AddMemoryCache()
            .AddSingleton<IServiceCache,MemoryCache>();

        return services;
    }
}