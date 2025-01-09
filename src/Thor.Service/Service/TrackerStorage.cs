using Thor.Abstractions.Dtos;
using Thor.Abstractions.Tracker;

namespace Thor.Service.Service;

public class TrackerStorage : ITrackerStorage, ISingletonDependency
{
    public List<TrackerDto> TrackerData { get; set; }
    
    public TrackerStorage()
    {
        TrackerData = new List<TrackerDto>();

        for (int i = 0; i < 20; i++)
        {
            TrackerData.Add(new TrackerDto
            {
                Tooltip = "服务状态 暂无数据",
                Percentage = 100,
                Time = DateTime.Now.AddMinutes(-i)
            });
        }
    }

    public void Add(TrackerDto trackerDto)
    {
        // 如果超过30条数据，删除第一条
        if (TrackerData.Count > 20)
        {
            TrackerData.RemoveAt(0);
        }

        TrackerData.Add(trackerDto);
    }
}