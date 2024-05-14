using AIDotNet.API.Service.Domain.Core;

namespace AIDotNet.API.Service.Domain;

public class StatisticsConsumesNumber : Entity<string>
{
    public ushort Year { get; set; }
    
    public ushort Month { get; set; }
    
    public ushort Day { get; set; }

    public long Number { get; set; }
    
    public StatisticsConsumesNumberType Type { get; set; }

    public long Value { get; set; }
}