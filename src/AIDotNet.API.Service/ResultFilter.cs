using AIDotNet.API.Service.Model;

namespace AIDotNet.API.Service;

public sealed class ResultFilter : IEndpointFilter
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
            return ResultDto.CreateFail(e.Message);
        }
    }
}