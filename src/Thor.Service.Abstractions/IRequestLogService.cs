using System;
using Thor.Domain.Chats;

namespace Thor.Service.Abstractions
{
    public interface IRequestLogService
    {
        RequestLog BeginRequestLog(string routePath, string requestBody);

        void EndRequestLog(RequestLog log, int httpStatusCode, string responseBody, Exception? ex = null);
    }
}
