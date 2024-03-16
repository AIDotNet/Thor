namespace AIDotNet.OpenAI;

public class OpenAIOptions
{
    public const string ServiceName = "OpenAI";

    public IServiceProvider ServiceProvider { get; set; }
    
    public HttpClient Client { get; set; }
}