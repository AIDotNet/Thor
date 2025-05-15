using Microsoft.EntityFrameworkCore;
using Thor.Core;
using Thor.Core.DataAccess;

namespace Thor.Provider;

public class SqlServerLoggerContext(DbContextOptions<SqlServerLoggerContext> options, IServiceProvider serviceProvider)
    : LoggerDbContext<SqlServerLoggerContext>(options, serviceProvider)
{
    
}