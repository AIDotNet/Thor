namespace AIDotNet.OpenAI;

public class OpenAIServiceOptions
{
    public const string ServiceName = "OpenAI";

    public IServiceProvider ServiceProvider { get; set; }
    
    public HttpClient? Client { get; set; }
}