using Microsoft.Extensions.DependencyInjection;
using Thor.BuildingBlocks.Data;

namespace Thor.LocalEvent;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalEventBus(this IServiceCollection services)
    {
        services
            .AddSingleton(typeof(IEventBus<>), typeof(LocalEventBus<>));

        return services;
    }
}