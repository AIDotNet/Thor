using Microsoft.EntityFrameworkCore;
using Thor.Core;

namespace Thor.Provider;

public class DMLoggerContext(DbContextOptions<DMLoggerContext> options, IServiceProvider serviceProvider)
    : LoggerDbContext<DMLoggerContext>(options, serviceProvider)
{
    
}