namespace AIDotNet.Abstractions.Dto;

public class OpenAIErrorDto
{
    public string message { get; set; }
    public string type { get; set; }
    public object param { get; set; }
    public string code { get; set; }
}