using Thor.Abstractions.Dtos;
using Thor.Abstractions.Tracker;
using Thor.Service.Domain.Core;

namespace Thor.Service.Service;

/// <summary>
/// 服务状态跟踪
/// </summary>
/// <param name="trackerStorage"></param>
public class TrackerService(ITrackerStorage trackerStorage, IServiceProvider serviceProvider)
    : ApplicationService(serviceProvider)
{
    public async Task<List<TrackerDto>> GetAsync()
    {
        var trackerData = await trackerStorage.GetTrackerData();

        return trackerData;
    }

    /// <summary>
    /// 获取最近一年的请求统计
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserRequestDto>> GetUserRequest()
    {
        // 获取这个用户最近一年的请求统计
        var endDate = DateTime.Now.AddDays(1).Date;
        var startDate = endDate.AddDays(-365);

        var value = await LoggerDbContext.StatisticsConsumesNumbers.Where(x =>
            x.Creator == UserContext.CurrentUserId &&
            x.Type == StatisticsConsumesNumberType.Requests &&
            x.CreatedAt >= startDate && x.CreatedAt <= endDate).ToArrayAsync();

        var userRequest = new List<UserRequestDto>(value.Length);

        // 需要包含每一天的数据，如果没有数据，需要补0
        for (var i = 0; i < 365; i++)
        {
            var date = startDate.AddDays(i);
            var item = value.FirstOrDefault(x => x.CreatedAt.Date == date);
            if (item == null)
            {
                userRequest.Add(new UserRequestDto
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Count = 0
                });
            }
            else
            {
                userRequest.Add(new UserRequestDto
                {
                    Date = item.CreatedAt.ToString("yyyy-MM-dd"),
                    Count = item.Value,
                    Level = GetLevel(item.Value)
                });
            }
        }

        return userRequest;
    }
    
    /// <summary>
    /// 根据请求次数返回等级
    /// </summary>
    /// <returns></returns>
    public int GetLevel(long count)
    {
        if (count < 10)
        {
            return 1;
        }
        else if (count < 50)
        {
            return 2;
        }
        else if (count < 100)
        {
            return 3;
        }
        else if (count < 200)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }
}