namespace AIDotNet.Abstractions.Dto;

public class OpenAIChatVisionCompletionRequestInput
{
    public string role { get; set; }

    public OpenAIChatVisionContentInput[] content { get; set; }

    public string? name { get; set; }
}