using Microsoft.EntityFrameworkCore;
using Thor.Core;

namespace Thor.Provider;

public class SqlServerThorContext(DbContextOptions<SqlServerThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<SqlServerThorContext>(context, serviceProvider)
{
    
}