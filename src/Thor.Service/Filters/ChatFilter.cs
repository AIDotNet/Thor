using Thor.Abstractions.Exceptions;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.Service.Filters;

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
            return new ChatCompletionsResponse()
            {
                Error = new Error()
                {
                    Code = "400",
                    Messages =
                    {
                        notModel.Message
                    },
                    Type = "not_model"
                }
            };
        }
        catch (Exception e)
        {
            return new ChatCompletionsResponse()
            {
                Error = new Error()
                {
                    Code = "400",
                    Messages =
                    {
                        e.Message
                    },
                    Type = "error"
                }
            };
        }
    }
}
