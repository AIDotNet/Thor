using Microsoft.AspNetCore.Authorization;
using Thor.Service.Filters;
using Thor.Service.Service;

namespace Thor.Service.Extensions;

public static class EndpointRouteExtensions
{
    public static IEndpointRouteBuilder MapModelManager(this IEndpointRouteBuilder endpoints)
    {
        var modelManager = endpoints
            .MapGroup("/api/v1/model-manager")
            .WithTags("模型管理")
            .AddEndpointFilter<ResultFilter>()
            .WithDescription("模型管理");

        modelManager.MapPost(string.Empty,
                async (ModelManagerService modelManagerService, CreateModelManagerInput input) =>
                    await modelManagerService.CreateAsync(input))
            .WithDescription("创建模型 倍率")
            .RequireAuthorization(new AuthorizeAttribute()
            {
                Roles = RoleConstant.Admin
            })
            .WithName("创建模型");

        modelManager.MapPut(string.Empty,
                async (ModelManagerService modelManagerService, UpdateModelManagerInput input) =>
                    await modelManagerService.UpdateAsync(input))
            .WithDescription("更新模型 倍率")
            .RequireAuthorization(new AuthorizeAttribute()
            {
                Roles = RoleConstant.Admin
            })
            .WithName("更新模型");

        modelManager.MapDelete("{id}",
                async (ModelManagerService modelManagerService, Guid id) => await modelManagerService.DeleteAsync(id))
            .WithDescription("删除模型 倍率")
            .RequireAuthorization(new AuthorizeAttribute()
            {
                Roles = RoleConstant.Admin
            })
            .WithName("删除模型");

        modelManager.MapGet(string.Empty,
                async (ModelManagerService modelManagerService, string? model, int page, int pageSize, string? type,
                        string[]? tags = null, bool isPublic = false, bool? enabled = null) =>
                    await modelManagerService.GetListAsync(model, page, pageSize, isPublic, type, tags, enabled))
            .WithDescription("获取模型列表")
            .AllowAnonymous()
            .WithName("获取模型列表");

        modelManager.MapPut("{id}/enable", async (ModelManagerService modelManagerService, Guid id) =>
                await modelManagerService.EnableAsync(id))
            .RequireAuthorization(new AuthorizeAttribute()
            {
                Roles = RoleConstant.Admin
            })
            .WithDescription("启用/禁用模型")
            .WithName("启用/禁用模型");

        // 新增统计信息端点
        modelManager.MapGet("stats", async (ModelManagerService modelManagerService) =>
                await modelManagerService.GetModelStatsAsync())
            .WithDescription("获取模型统计信息")
            .RequireAuthorization(new AuthorizeAttribute()
            {
                Roles = RoleConstant.Admin
            })
            .WithName("获取模型统计信息");

        // 新增获取模型类型端点
        modelManager.MapGet("types", async (ModelManagerService modelManagerService) =>
                await modelManagerService.GetModelTypesAsync())
            .WithDescription("获取所有模型类型")
            .RequireAuthorization(new AuthorizeAttribute()
            {
                Roles = RoleConstant.Admin
            })
            .WithName("获取所有模型类型");

        // 新增获取所有标签端点
        modelManager.MapGet("tags", async (ModelManagerService modelManagerService) =>
                await modelManagerService.GetAllTagsAsync())
            .WithDescription("获取所有标签")
            .WithName("获取所有标签");

        // 新增获取模型库元数据端点
        modelManager.MapGet("metadata", async (ModelManagerService modelManagerService) =>
                await modelManagerService.GetMetadataAsync())
            .WithDescription("获取模型管理器元数据信息")
            .AllowAnonymous()
            .WithName("获取模型管理器元数据");

        return endpoints;
    }
}