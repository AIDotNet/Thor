using Microsoft.EntityFrameworkCore;
using Thor.Core;

namespace Thor.Provider;

public class SqliteThorContext(DbContextOptions<SqliteThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<SqliteThorContext>(context, serviceProvider)
{
    
}