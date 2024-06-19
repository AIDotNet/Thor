using System.Threading.Channels;
using Thor.Abstractions;
using Thor.Service.DataAccess;
using Thor.Service.Domain;

namespace Thor.Service.EventBus;

public sealed class ChannelEventHandler : IEventHandler<ChatLogger>, IDisposable
{
    private readonly ILogger<ChannelEventHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;
    private readonly Timer _timer;
    private readonly Channel<ChatLogger> _events = Channel.CreateUnbounded<ChatLogger>();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private const int Interval = 10000; // Interval in milliseconds

    public ChannelEventHandler(IServiceProvider serviceProvider, ILogger<ChannelEventHandler> logger)
    {
        _logger = logger;
        _scope = serviceProvider.CreateScope();
        _serviceProvider = _scope.ServiceProvider;
        _timer = new Timer(Flush, null, Interval, Interval);
    }

    public async Task HandleAsync(ChatLogger @event)
    {
        await _events.Writer.WriteAsync(@event);
    }

    private async void Flush(object state)
    {
        await FlushAsync();
    }

    private async Task FlushAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            var currentEvents = new List<ChatLogger>();

            _logger.LogInformation("Flushing events to database");

            while (_events.Reader.TryRead(out var value))
            {
                currentEvents.Add(value);
            }

            _logger.LogInformation($"Flushing {currentEvents.Count} events to database");

            if (!currentEvents.Any())
            {
                return;
            }

            var loggerDbContext = _serviceProvider.GetRequiredService<LoggerDbContext>();
            await loggerDbContext.Loggers.AddRangeAsync(currentEvents);
            await loggerDbContext.SaveChangesAsync();

            _logger.LogInformation("Events flushed to database");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
        _scope.Dispose();
    }
}