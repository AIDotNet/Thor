using Thor.BuildingBlocks.Data;
using Thor.Service.Eto;
using Thor.Service.Service;

namespace Thor.Service.EventBus;

/// <summary>
/// 创建用户事件处理器
/// </summary>
[Registration(typeof(IEventHandler<CreateUserEto>))]
public class CreateUserEventHandler : IEventHandler<CreateUserEto>, IDisposable, IScopeDependency
{
    private readonly ILogger<CreateUserEventHandler> _logger;
    private readonly TokenService _tokenService;
    private readonly LoggerService _loggerService;

    public CreateUserEventHandler(ILogger<CreateUserEventHandler> logger, TokenService tokenService,
        LoggerService loggerService)
    {
        _logger = logger;
        _tokenService = tokenService;
        _loggerService = loggerService;
        _logger.LogInformation("CreateUserEventHandler created");
    }

    public async Task HandleAsync(CreateUserEto @event)
    {
        _logger.LogInformation("CreateUserEto event received");

        await _tokenService.CreateAsync(new TokenInput
        {
            Name = "默认Token",
            UnlimitedQuota = true,
            UnlimitedExpired = true
        }, @event.User.Id);

        switch (@event.Source)
        {
            case CreateUserSource.System:
                await _loggerService.CreateSystemAsync("新增用户：" + @event.User.UserName);
                break;
            case CreateUserSource.Github:
                await _loggerService.CreateSystemAsync("Github新增用户：" + @event.User.UserName);
                break;
            case CreateUserSource.Gitee:
                await _loggerService.CreateSystemAsync("Gitee新增用户：" + @event.User.UserName);
                break;
        }

        _logger.LogInformation("CreateUserEto event received");
    }

    public void Dispose()
    {
        _logger.LogInformation("CreateUserEventHandler disposed");
    }
}