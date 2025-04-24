using Thor.BuildingBlocks.Event;
using Thor.Core.DataAccess;
using Thor.Service.Eto;
using Thor.Service.Service;

namespace Thor.Service.EventBus;

/// <summary>
/// 当模型映射缓存更新事件触发时，更新模型管理器缓存
/// </summary>
/// <param name="thorContext"></param>
public sealed class ModelManagerEventHandler(IThorContext thorContext)
    : IEventHandler<UpdateModelManagerCache>
{
    public async Task HandleAsync(UpdateModelManagerCache @event)
    {
        await ModelManagerService.LoadingSettings(thorContext);
    }
}