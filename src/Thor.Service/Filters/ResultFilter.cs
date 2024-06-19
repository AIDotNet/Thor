using Thor.Service.Model;

namespace Thor.Service.Filters;

public sealed class ResultFilter(ILogger<ResultFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            var result = await next(context);

            if (result is ResultDto resultDto)
            {
                return resultDto;
            }

            return ResultDto.CreateSuccess(string.Empty, result);
        }
        catch (UnauthorizedAccessException e)
        {
            context.HttpContext.Response.StatusCode = 401;
            return null;
        }
        catch (Exception e)
        {
            logger.LogError("An error occurred while processing the request. {e}", e);
            return ResultDto.CreateFail(e.Message);
        }
    }
}