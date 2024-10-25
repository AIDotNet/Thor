﻿using Thor.BuildingBlocks.Data;
using Thor.Service.Eto;
using Thor.Service.Service;

namespace Thor.Service.EventBus;

/// <summary>
/// 创建用户事件处理器
/// </summary>
[Registration(typeof(IEventHandler<CreateUserEto>))]
public sealed class CreateUserEventHandler : IEventHandler<CreateUserEto>, IDisposable, ISingletonDependency
{
    private readonly ILogger<CreateUserEventHandler> _logger;
    private readonly IServiceScope _scope;
    private readonly TokenService _tokenService;
    private readonly LoggerService _loggerService;

    public CreateUserEventHandler(IServiceProvider serviceProvider, ILogger<CreateUserEventHandler> logger)
    {
        _logger = logger;
        _scope = serviceProvider.CreateScope();

        _tokenService = _scope.ServiceProvider.GetRequiredService<TokenService>();
        _loggerService = _scope.ServiceProvider.GetRequiredService<LoggerService>();
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
        
        await _loggerService.CreateSystemAsync("新增用户：" + @event.User.UserName);
        
        _logger.LogInformation("CreateUserEto event received");
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}