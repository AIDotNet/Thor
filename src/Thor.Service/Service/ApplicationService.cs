using MapsterMapper;
using Thor.Service.Infrastructure;

namespace Thor.Service.Service;

public abstract class ApplicationService(IServiceProvider serviceProvider)
{
    protected IUserContext UserContext => GetService<IUserContext>();

    protected AIDotNetDbContext DbContext => GetService<AIDotNetDbContext>();

    protected IMapper Mapper => GetService<IMapper>();

    protected LoggerDbContext LoggerDbContext => GetService<LoggerDbContext>();

    protected T GetService<T>()
    {
        return serviceProvider.GetRequiredService<T>();
    }

    public ILogger<T> GetLogger<T>()
    {
        return serviceProvider.GetRequiredService<ILogger<T>>();
    }

    protected T? GetKeyedService<T>(string key)
    {
        return serviceProvider.GetKeyedService<T>(key);
    }

    protected T GetRequiredKeyedService<T>(string key)
    {
        return serviceProvider.GetRequiredKeyedService<T>(key);
    }
}