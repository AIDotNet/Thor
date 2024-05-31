using AIDotNet.API.Service.Domain.Core;

namespace AIDotNet.API.Service.Domain;

public class StatisticsConsumesNumber : Entity<string>
{
    public int Year { get; set; }
    
    public int Month { get; set; }
    
    public int Day { get; set; }

    public long Number { get; set; }
    
    public StatisticsConsumesNumberType Type { get; set; }

    public long Value { get; set; }
}