using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Infrastructure;
using MapsterMapper;

namespace AIDotNet.API.Service.Service;

public abstract class ApplicationService(IServiceProvider serviceProvider)
{
    protected T GetService<T>()
    {
        return serviceProvider.GetRequiredService<T>();
    }

    protected T? GetKeyedService<T>(string key)
    {
        return serviceProvider.GetKeyedService<T>(key);
    }

    protected T GetRequiredKeyedService<T>(string key)
        => serviceProvider.GetRequiredKeyedService<T>(key);

    protected IUserContext UserContext => GetService<IUserContext>();

    protected AIDotNetDbContext DbContext => GetService<AIDotNetDbContext>();

    protected IMapper Mapper => GetService<IMapper>();
    
    protected LoggerDbContext LoggerDbContext => GetService<LoggerDbContext>();
}