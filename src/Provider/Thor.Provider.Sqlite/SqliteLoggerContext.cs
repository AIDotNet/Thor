using Microsoft.EntityFrameworkCore;
using Thor.Core;

namespace Thor.Provider;

public class SqliteLoggerContext(DbContextOptions<SqliteLoggerContext> options, IServiceProvider serviceProvider)
    : LoggerDbContext<SqliteLoggerContext>(options, serviceProvider)
{
    
}