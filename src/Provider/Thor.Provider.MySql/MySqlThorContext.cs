using Microsoft.EntityFrameworkCore;
using Thor.Core;

namespace Thor.Provider;

public class MySqlThorContext(DbContextOptions<MySqlThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<MySqlThorContext>(context, serviceProvider)
{
    
}