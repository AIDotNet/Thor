using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Thor.Core;

namespace Thor.Provider;

public class PostgreSQLThorContext(DbContextOptions<PostgreSQLThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<PostgreSQLThorContext>(context, serviceProvider)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.ConfigureWarnings(warnings =>
			warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
	}
}