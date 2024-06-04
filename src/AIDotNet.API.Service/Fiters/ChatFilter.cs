using AIDotNet.Abstractions.Dto;
using AIDotNet.Abstractions.Exceptions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using ChatCompletionCreateResponse = AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.ChatCompletionCreateResponse;

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
            return new ChatCompletionCreateResponse()
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
            return new ChatCompletionCreateResponse()
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
