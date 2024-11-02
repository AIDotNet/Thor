using Microsoft.EntityFrameworkCore;
using Thor.Core;

namespace Thor.Provider;

public class SqlServerLoggerContext(DbContextOptions<SqlServerLoggerContext> options, IServiceProvider serviceProvider)
    : LoggerDbContext<SqlServerLoggerContext>(options, serviceProvider)
{
    
}