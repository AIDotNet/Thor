
namespace Thor.Rabbit;

public class RabbitBoot : IHostedService
{
    private readonly RabbitClientBus _rabbitClient;

    // ReSharper disable once ConvertToPrimaryConstructor
    public RabbitBoot(RabbitClientBus rabbitClient)
    {
        _rabbitClient = rabbitClient;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _rabbitClient.StartAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _rabbitClient.StopAsync(cancellationToken).ConfigureAwait(false);
    }
}