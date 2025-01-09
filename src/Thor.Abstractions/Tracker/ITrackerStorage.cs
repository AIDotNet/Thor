using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.Tracker;

public interface ITrackerStorage
{
    List<TrackerDto> TrackerData { get; }
    
    void Add(TrackerDto trackerDto);
}