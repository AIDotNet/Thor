using Thor.Abstractions.Dtos;
using Thor.Abstractions.Tracker;

namespace Thor.Service.Service;

public class TrackerStorage(IServiceCache serviceCache) : ITrackerStorage
{
    private const string CacheKey = "TrackerData";

    public async Task<List<TrackerDto>> GetTrackerData()
    {
        return (await serviceCache.GetOrCreateAsync(CacheKey, async () =>
        {
            var data = new List<TrackerDto>();

            for (int i = 0; i < 20; i++)
            {
                data.Add(new TrackerDto
                {
                    Tooltip = "服务状态 暂无数据",
                    Percentage = 100,
                    Time = DateTime.Now.AddMinutes(-i)
                });
            }

            return await Task.FromResult(data);
        }, TimeSpan.FromMinutes(5), true))!;
    }

    public async Task AddAsync(TrackerDto trackerDto)
    {
        // 获取缓存数据
        var data = await serviceCache.GetOrCreateAsync(CacheKey, async () => await GetTrackerData());

        // 只需要20条数据，如果超过则删除第一条
        if (data.Count >= 20)
        {
            data.RemoveAt(0);
        }

        data.Add(trackerDto);

        // 更新缓存
        await serviceCache.CreateAsync(CacheKey, data);
    }
}