using System.Diagnostics;
using Thor.Core.DataAccess;

namespace Thor.Service.Infrastructure.Middlewares;

public class UnitOfWorkMiddleware(ILogger<UnitOfWorkMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 如果不是Get则自动开启事务
        if (context.Request.Method != "GET" && context.Request.Method != "OPTIONS" &&
            context.Request.Method != "HEAD" && context.Request.Method != "TRACE" &&
            context.Request.Method != "CONNECT")
        {
            using var activity =
                Activity.Current?.Source.StartActivity("UnitOfWork", ActivityKind.Internal);

            activity?.SetTag("UnitOfWork", "Begin");

            var dbContext = context.RequestServices.GetRequiredService<IThorContext>();
            var loggerDbContext = context.RequestServices.GetRequiredService<ILoggerDbContext>();
            try
            {
                await next(context);
                await dbContext.SaveChangesAsync();
                await loggerDbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred during the transaction. Message: {Message}",
                    exception.Message);

                activity?.SetTag("UnitOfWork", "Rollback");
            }

            activity?.SetTag("UnitOfWork", "End");

            return;
        }

        await next(context);
    }
}