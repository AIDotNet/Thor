using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Thor.Core;

namespace Thor.Provider;

public class MySqlThorContext(DbContextOptions<MySqlThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<MySqlThorContext>(context, serviceProvider)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.ConfigureWarnings(warnings =>
			warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
	}
}