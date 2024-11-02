using Microsoft.EntityFrameworkCore;
using Thor.Core;

namespace Thor.Provider;

public class PostgreSQLThorContext(DbContextOptions<PostgreSQLThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<PostgreSQLThorContext>(context, serviceProvider)
{
    
}