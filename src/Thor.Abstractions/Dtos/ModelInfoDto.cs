namespace Thor.Abstractions.Dtos;

public class ModelInfoDto
{
    public List<ModelTypeCountDto> ModelTypeCounts { get; set; } = new();
    
    public List<ModelTypeDto> ModelTypes { get; set; } = new();
}