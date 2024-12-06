using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Thor.Core.DataAccess;

namespace Thor.Core.Extensions;

public static class ServiceExtensions
{
	public static IServiceCollection AddThorDataAccess<TContext>(this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> configureContext)
		where TContext : BaseContext<TContext>, IThorContext
	{
		//services.TryAddScoped<IThorContext>(provider => provider.GetRequiredService<TContext>());

		services.AddDbContext<IThorContext, TContext>(configureContext);

		return services;
	}

	public static IServiceCollection AddLocalDataAccess<TContext>(this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> configureContext)
		where TContext : BaseContext<TContext>, ILoggerDbContext
	{
		//services.TryAddScoped<ILoggerDbContext>(provider => provider.GetRequiredService<TContext>());

		services.AddDbContext<ILoggerDbContext, TContext>(configureContext);

		return services;
	}

	public static IServiceCollection AddThorDataAccess(this IServiceCollection services,
		Action<IServiceCollection> configureServices)
	{

		configureServices(services);

		return services;
	}
}