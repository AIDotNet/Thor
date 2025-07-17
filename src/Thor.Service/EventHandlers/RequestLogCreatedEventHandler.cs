using Thor.BuildingBlocks.Event;
using Thor.Core.DataAccess;
using Thor.Domain.Chats;

namespace Thor.Service.EventHandlers
{
    public sealed class RequestLogCreatedEventHandler(
        ILogger<RequestLogCreatedEventHandler> logger,
        ILoggerDbContext loggerDbContext)
        : IEventHandler<RequestLog>
    {
        public async Task HandleAsync(RequestLog @event)
        {
            await loggerDbContext.RequestLogs.AddAsync(@event);

            await loggerDbContext.SaveChangesAsync();

            logger.LogInformation("RequestLog created: {Id}, {ChatLoggerId}", @event.Id, @event.ChatLoggerId);
        }
    }
}