namespace AIDotNet.MetaGLM;

public class MetaGLMOptions
{
    public const string ServiceName = "MetaGLM";

    public IServiceProvider ServiceProvider { get; set; }

    public MetaGLMClientV4? Client { get; set; }
}