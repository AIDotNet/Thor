namespace Thor.Rabbit;

public class RabbitClientBus : RabbitClient
{
    private readonly IServiceProvider _sp;

    // ReSharper disable once ConvertToPrimaryConstructor
    public RabbitClientBus(IServiceProvider sp, ILogger<RabbitClient> logger, RabbitOptions rabbitOptions) : base(
        logger, rabbitOptions, sp)
    {
        _sp = sp;
    }
}