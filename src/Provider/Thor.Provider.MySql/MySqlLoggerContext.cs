using Microsoft.EntityFrameworkCore;
using Thor.Core;
using Thor.Core.DataAccess;

namespace Thor.Provider;

public class MySqlLoggerContext(DbContextOptions<MySqlLoggerContext> options, IServiceProvider serviceProvider)
    : LoggerDbContext<MySqlLoggerContext>(options, serviceProvider)
{
    
}