using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.Tracker;

public interface ITrackerStorage
{
    Task<List<TrackerDto>> GetTrackerData();
    
    Task AddAsync(TrackerDto trackerDto);
}