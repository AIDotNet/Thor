namespace Thor.Abstractions.Images.Dtos;

public class ThorImageCreateResponse
{
    public object data { get; set; }
    public int created { get; set; }
    
    public bool successful { get; set; }
}
