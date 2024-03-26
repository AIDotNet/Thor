namespace AIDotNet.Abstractions.Dto;

public class OpenAIChatVisionContentInput
{
    public string type { get; set; }

    public string text { get; set; }

    public object image_url { get; set; }
    
}