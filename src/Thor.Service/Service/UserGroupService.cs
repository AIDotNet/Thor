using Thor.Abstractions.Exceptions;
using Thor.Domain.Users;
using Thor.Service.Infrastructure;
using Thor.Service.Model;

namespace Thor.Service.Service;

public sealed class UserGroupService(
    IServiceProvider serviceProvider,
    IServiceCache serviceCache,
    IUserContext userContext)
    : ApplicationService(serviceProvider)
{
    public async Task<ResultDto> CreateAsync(UserGroup userGroup)
    {
        if (await DbContext.UserGroups.AnyAsync(x => x.Name == userGroup.Name))
        {
            throw new BusinessException("分组名称已存在", "400");
        }

        if (await DbContext.UserGroups.AnyAsync(x => x.Code == userGroup.Code))
        {
            throw new BusinessException("分组编码已存在", "400");
        }

        await DbContext.UserGroups.AddAsync(userGroup);
        await serviceCache.RemoveAsync("UserGroupCache");

        return ResultDto.CreateSuccess("创建成功");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await DbContext.UserGroups.FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            throw new BusinessException("分组不存在", "400");
        }

        if (entity.Code == "default")
        {
            throw new BusinessException("默认分组不能删除", "400");
        }

        await DbContext.UserGroups.Where(x => x.Id == id).ExecuteDeleteAsync();
        await serviceCache.RemoveAsync("UserGroupCache");

        return ResultDto.CreateSuccess("删除成功");
    }

    public async Task<ResultDto> UpdateAsync(UserGroup userGroup)
    {
        var entity = await DbContext.UserGroups.FirstOrDefaultAsync(x => x.Id == userGroup.Id);
        if (entity == null)
        {
            throw new BusinessException("分组不存在", "400");
        }

        if (await DbContext.UserGroups.AnyAsync(x => x.Name == userGroup.Name && x.Id != userGroup.Id))
        {
            throw new BusinessException("分组名称已存在", "400");
        }

        if (await DbContext.UserGroups.AnyAsync(x => x.Code == userGroup.Code && x.Id != userGroup.Id))
        {
            throw new BusinessException("分组编码已存在", "400");
        }

        if (entity.Code == "default" && userGroup.Code != "default")
        {
            throw new BusinessException("默认分组不能修改编码", "400");
        }

        entity.Name = userGroup.Name;
        entity.Description = userGroup.Description;
        entity.Code = userGroup.Code;
        entity.Rate = userGroup.Rate;
        entity.Enable = userGroup.Enable;
        entity.Order = userGroup.Order;
        await serviceCache.RemoveAsync("UserGroupCache");

        return ResultDto.CreateSuccess("更新成功");
    }

    public async Task<ResultDto> EnableAsync(Guid id, bool enable)
    {
        var entity = await DbContext.UserGroups.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null)
        {
            throw new BusinessException("分组不存在", "400");
        }

        if (entity.Code == "default")
        {
            throw new BusinessException("默认分组不能修改状态", "400");
        }

        entity.Enable = enable;

        await serviceCache.RemoveAsync("UserGroupCache");

        return ResultDto.CreateSuccess("操作成功");
    }

    public async Task<List<UserGroup>> GetListAsync()
    {
        return await DbContext.UserGroups.ToListAsync();
    }

    /// <summary>
    /// 获取可用的分组
    /// </summary>
    public async Task<List<UserGroup>> GetEnableListAsync()
    {
        return await serviceCache.GetOrCreateAsync("UserGroupCache",
                   async () => { return await DbContext.UserGroups.Where(x => x.Enable).ToListAsync(); }) ??
               new List<UserGroup>();
    }

    /// <summary>
    /// 获取分组，如果传入多个分组编码，返回最小的一个
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<UserGroup?> GetAsync(string[] code)
    {
        return (await GetEnableListAsync()).Where(x => code.Contains(x.Code)).OrderBy(x => x.Rate).FirstOrDefault();
    }

    /// <summary>
    /// 获取当前用户分组
    /// </summary>
    public async Task<UserGroup[]> GetCurrentUserGroupAsync()
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == userContext.CurrentUserId);

        if (user == null)
        {
            return null;
        }

        var groups = user.Groups;


        return await DbContext.UserGroups.Where(x => groups.Contains(x.Code)).ToArrayAsync();
    }
}