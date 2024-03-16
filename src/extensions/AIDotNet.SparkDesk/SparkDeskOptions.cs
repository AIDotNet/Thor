using Sdcb.SparkDesk;

namespace AIDotNet.SparkDesk;

public sealed class SparkDeskOptions
{
    public const string ServiceName = "SparkDesk";

    public IServiceProvider ServiceProvider { get; set; }
}