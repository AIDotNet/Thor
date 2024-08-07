﻿using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Exceptions;

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
            return new ThorChatCompletionsResponse()
            {
                Error = new ThorError()
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
            return new ThorChatCompletionsResponse()
            {
                Error = new ThorError()
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
