using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Thor.Core;

namespace Thor.Provider;

public class SqlServerThorContext(DbContextOptions<SqlServerThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<SqlServerThorContext>(context, serviceProvider)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.ConfigureWarnings(warnings =>
			warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
	}
}