using Microsoft.EntityFrameworkCore;
using Thor.Core;
using Thor.Core.DataAccess;

namespace Thor.Provider;

public class DMLoggerContext(DbContextOptions<DMLoggerContext> options, IServiceProvider serviceProvider)
    : LoggerDbContext<DMLoggerContext>(options, serviceProvider)
{
    
}