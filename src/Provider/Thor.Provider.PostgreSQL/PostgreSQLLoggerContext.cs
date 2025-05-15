using Microsoft.EntityFrameworkCore;
using Thor.Core;
using Thor.Core.DataAccess;

namespace Thor.Provider;

public class PostgreSQLLoggerContext(DbContextOptions<PostgreSQLLoggerContext> options, IServiceProvider serviceProvider)
    : LoggerDbContext<PostgreSQLLoggerContext>(options, serviceProvider)
{
    
}