namespace Thor.Service.Service;

public class RateLimitModelService(IServiceProvider serviceProvider) : ApplicationService(serviceProvider)
{
    private static List<RateLimitModel> _rateLimitModels = new();

    public static async ValueTask LoadAsync(AIDotNetDbContext context)
    {
        _rateLimitModels = await context.RateLimitModels
            .AsNoTracking()
            .Where(x => x.Enable)
            .ToListAsync();
    }

    /// <summary>
    /// 模型速率检测
    /// </summary>
    /// <param name="model"></param>
    /// <param name="context"></param>
    /// <param name="serviceCache"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="RateLimitException"></exception>
    public async ValueTask CheckAsync(string model, HttpContext context, IServiceCache serviceCache)
    {
        // 获取IP
        var ip = context.Connection.RemoteIpAddress?.ToString();

        //获取头
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var header))
        {
            ip = header;
        }

        foreach (var rateLimitModel in _rateLimitModels.Where(x => x.Model.Contains(model)))
        {
            if (rateLimitModel.WhiteList.Contains(ip)) return;

            if (rateLimitModel.BlackList.Contains(ip)) throw new Exception("IP is in the blacklist");

            var key = $"{model}:{ip}{rateLimitModel.Id}";


            // 判断是否存在缓存
            var cache = await serviceCache.GetAsync<int?>(key);
            if (cache.HasValue)
            {
                if (cache.Value >= rateLimitModel.Value) throw new RateLimitException("Rate limit exceeded");

                await serviceCache.IncrementAsync(key);
            }
            else
            {
                // Strategy
                var strategy = rateLimitModel.Strategy switch
                {
                    "s" => TimeSpan.FromSeconds(rateLimitModel.Limit),
                    "m" => TimeSpan.FromMinutes(rateLimitModel.Limit),
                    "h" => TimeSpan.FromHours(rateLimitModel.Limit),
                    "d" => TimeSpan.FromDays(rateLimitModel.Limit),
                    _ => throw new Exception("Invalid strategy")
                };
                await serviceCache.IncrementAsync(key, 1, strategy);
            }
        }
    }

    public async ValueTask<PagingDto<RateLimitModel>> GetAsync(int page, int pageSize)
    {
        var total = await DbContext.RateLimitModels.CountAsync();

        if (total > 0)
        {
            var result = await DbContext.RateLimitModels
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDto<RateLimitModel>(total, result);
        }

        return new PagingDto<RateLimitModel>(total, new List<RateLimitModel>());
    }

    public async ValueTask CreateAsync(RateLimitModel rateLimitModel)
    {
        rateLimitModel.Id = Guid.NewGuid().ToString();
        await DbContext.RateLimitModels.AddAsync(rateLimitModel);

        await DbContext.SaveChangesAsync();

        await LoadAsync(DbContext);
    }

    public async ValueTask<bool> RemoveAsync(string id)
    {
        var result = await DbContext.RateLimitModels.Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        await LoadAsync(DbContext);
        return result > 0;
    }

    public async ValueTask<bool> UpdateAsync(RateLimitModel rateLimitModel)
    {
        var result = await DbContext.RateLimitModels.Where(x => x.Id == rateLimitModel.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.Name, rateLimitModel.Name)
                .SetProperty(x => x.Description, rateLimitModel.Description)
                .SetProperty(x => x.WhiteList, rateLimitModel.WhiteList)
                .SetProperty(x => x.BlackList, rateLimitModel.BlackList)
                .SetProperty(x => x.Enable, rateLimitModel.Enable)
                .SetProperty(x => x.Model, rateLimitModel.Model)
                .SetProperty(x => x.Value, rateLimitModel.Value)
                .SetProperty(x => x.Strategy, rateLimitModel.Strategy)
                .SetProperty(x => x.Limit, rateLimitModel.Limit));

        await LoadAsync(DbContext);
        return result > 0;
    }

    public async ValueTask Disable(string id)
    {
        await DbContext.RateLimitModels.Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.Enable, x => !x.Enable));

        await LoadAsync(DbContext);
    }
}