using Thor.Service.Filters;
using Thor.Service.Service;

namespace Thor.Service;

public static class EndpointRouteExtensions
{
    public static IEndpointRouteBuilder MapModelManager(this IEndpointRouteBuilder endpoints)
    {
        var modelManager = endpoints
            .MapGroup("/api/v1/model-manager")
            .RequireAuthorization()
            .WithTags("模型管理")
            .AddEndpointFilter<ResultFilter>()
            .WithDescription("模型管理");

        modelManager.MapPost(string.Empty,
                async (ModelManagerService modelManagerService, CreateModelManagerInput input) =>
                    await modelManagerService.CreateAsync(input))
            .WithDescription("创建模型 倍率")
            .WithName("创建模型");

        modelManager.MapPut(string.Empty, async (ModelManagerService modelManagerService,UpdateModelManagerInput input) => await modelManagerService.UpdateAsync(input))
            .WithDescription("更新模型 倍率")
            .WithName("更新模型");

        modelManager.MapDelete("{id}", async (ModelManagerService modelManagerService, Guid id) =>await modelManagerService.DeleteAsync(id))
            .WithDescription("删除模型 倍率")
            .WithName("删除模型");

        modelManager.MapGet(string.Empty, async (ModelManagerService modelManagerService, string? model, int page, int pageSize) =>
                  await  modelManagerService.GetListAsync(model, page, pageSize))
            .WithDescription("获取模型列表")
            .WithName("获取模型列表");

        modelManager.MapPut("{id}/enable", async (ModelManagerService modelManagerService, Guid id) =>
                   await modelManagerService.EnableAsync(id))
            .WithDescription("启用/禁用模型")
            .WithName("启用/禁用模型");

        return endpoints;
    }
}