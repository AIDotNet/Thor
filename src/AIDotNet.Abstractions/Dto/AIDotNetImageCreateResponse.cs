namespace AIDotNet.Abstractions.Dto;

public class AIDotNetImageCreateResponse
{
    public object data { get; set; }
    public int created { get; set; }
    public bool successful { get; set; }
}

public class Data
{
    public string url { get; set; }
    public string revised_prompt { get; set; }
}