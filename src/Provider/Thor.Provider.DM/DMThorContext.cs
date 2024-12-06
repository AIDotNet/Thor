using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Thor.Core;

namespace Thor.Provider;

public class DMThorContext(DbContextOptions<DMThorContext> context, IServiceProvider serviceProvider)
    : ThorContext<DMThorContext>(context, serviceProvider)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.ConfigureWarnings(warnings =>
			warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
	}
}