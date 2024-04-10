using AIDotNet.Abstractions.Dto;
using TokenApi.Service.Exceptions;

namespace AIDotNet.API.Service;

public sealed class ChatFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (NotModelException notModel)
        {
            return new OpenAIResultDto()
            {
                _object = Guid.NewGuid().ToString("N"),
                Error = new OpenAIErrorDto()
                {
                    code = "500",
                    message = $"未找到模型：{notModel.Message}",
                    type = "500"
                }
            };
        }
        catch (Exception e)
        {
            return new OpenAIResultDto()
            {
                _object = Guid.NewGuid().ToString("N"),
                Error = new OpenAIErrorDto()
                {
                    code = "500",
                    message = e.Message,
                    type = "500"
                }
            };
        }
    }
}
