using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Thor.Core;

namespace Thor.Provider;

public class SqliteThorContext(DbContextOptions<SqliteThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<SqliteThorContext>(context, serviceProvider)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.ConfigureWarnings(warnings =>
			warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
	}
}