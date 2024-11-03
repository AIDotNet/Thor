using Microsoft.EntityFrameworkCore;
using Thor.Core;

namespace Thor.Provider;

public class DMThorContext(DbContextOptions<DMThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<DMThorContext>(context, serviceProvider)
{
    
}