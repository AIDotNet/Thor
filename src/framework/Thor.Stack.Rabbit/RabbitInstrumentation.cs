using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Thor.Rabbit;

public class RabbitInstrumentation
{
    public const string MeterName = "Raccoon.Rabbit.Meter";

    public const string ActivitySourceName = "Raccoon.Rabbit.ActivitySource";

    static RabbitInstrumentation()
    {
        var version = typeof(RabbitInstrumentation).Assembly.GetName().Version?.ToString();
        Meter = new Meter(MeterName, version);
        ActivitySource = new ActivitySource(ActivitySourceName, version);
    }

    public static Meter Meter { get; }
    public static ActivitySource ActivitySource { get; }
}