using System.Text.Json;
using Microsoft.Extensions.Options;
using Thor.BuildingBlocks.Event;
using Thor.Core.DataAccess;
using Thor.Domain.Chats;
using Thor.Service.Infrastructure;
using Thor.Service.Infrastructure.Middlewares;
using Thor.Service.Options;

namespace Thor.Service.Service
{
    public sealed class RequestLogService : ApplicationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventBus<RequestLog> _eventBus;
        private readonly IOptions<ThorOptions> _thorOptions;
        private readonly ILoggerDbContext _loggerDbContext;

        public RequestLogService(
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ThorOptions> thorOptions,
            IEventBus<RequestLog> eventBus,
            ILoggerDbContext loggerDbContext)
            : base(serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _thorOptions = thorOptions;
            _eventBus = eventBus;
            _loggerDbContext = loggerDbContext;
        }

        public RequestLog? BeginRequestLog<T>(string routePath, T data)
        {
            if (!ThorOptions.EnableRequestLog)
            {
                return null;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidOperationException("HttpContext is not available");

            var request = httpContext.Request;
            var chatLoggerId = httpContext.Items["ChatLoggerId"] as string;

            var log = new RequestLog
            {
                Id = Guid.NewGuid().ToString("N"),
                RequestTime = DateTime.UtcNow,
                RoutePath = routePath,
                RequestBody = JsonSerializer.Serialize(data, ThorJsonSerializer.DefaultOptions),
                ClientIp = httpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = request.Headers["User-Agent"].ToString(),
                ChatLoggerId = chatLoggerId
            };

            log.SetRequestHeaders(request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()));

            return log;
        }

        public async Task EndRequestLog(RequestLog? log, int httpStatusCode, string? responseBody = null,
            Exception? ex = null)
        {
            if (log == null || !ThorOptions.EnableRequestLog)
            {
                return;
            }

            log.ResponseTime = DateTime.UtcNow;
            log.DurationMs = (long)(log.ResponseTime - log.RequestTime).TotalMilliseconds;
            log.HttpStatusCode = httpStatusCode;
            log.Creator = UserContext.CurrentUserId;
            
            log.ResponseBody = httpStatusCode == 200 ? StreamResponseInterceptor.GetCollectedContent() : responseBody;

            if (ex != null)
            {
                log.IsSuccess = false;
                log.ErrorMessage = ex.ToString();
            }
            else
            {
                log.IsSuccess = httpStatusCode is >= 200 and < 300;
            }

            await _eventBus.PublishAsync(log);
        }

        /// <summary>
        /// 获取请求日志列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="model">模型名称</param>
        /// <param name="userName">用户名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public async Task<PagingDto<RequestLog>> GetAsync(int page, int pageSize, string? userName,
            DateTime? startTime, DateTime? endTime)
        {
            var query = _loggerDbContext.RequestLogs
                .AsNoTracking();

            // 时间范围过滤
            if (startTime.HasValue)
            {
                query = query.Where(x => x.RequestTime >= startTime);
            }

            if (endTime.HasValue)
            {
                query = query.Where(x => x.RequestTime <= endTime);
            }

            // 权限控制：非管理员只能查看自己的日志
            if (!UserContext.IsAdmin)
            {
                query = query.Where(x => x.Creator == UserContext.CurrentUserId);
            }

            var total = await query.CountAsync();

            if (total <= 0) return new PagingDto<RequestLog>(total, []);

            var result = await query
                .OrderByDescending(x => x.RequestTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDto<RequestLog>(total, result);
        }
    }
}