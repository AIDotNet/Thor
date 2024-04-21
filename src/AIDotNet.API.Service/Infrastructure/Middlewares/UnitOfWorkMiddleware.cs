using AIDotNet.API.Service.DataAccess;

namespace AIDotNet.API.Service.Infrastructure.Middlewares;

public class UnitOfWorkMiddleware(ILogger<UnitOfWorkMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 如果不是Get则自动开启事务
        if (context.Request.Method != "GET" && context.Request.Method != "OPTIONS" &&
            context.Request.Method != "HEAD" && context.Request.Method != "TRACE" &&
            context.Request.Method != "CONNECT")
        {
            var dbContext = context.RequestServices.GetRequiredService<AIDotNetDbContext>();
            var loggerDbContext = context.RequestServices.GetRequiredService<LoggerDbContext>();
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
            }

            return;
        }

        await next(context);
    }
}