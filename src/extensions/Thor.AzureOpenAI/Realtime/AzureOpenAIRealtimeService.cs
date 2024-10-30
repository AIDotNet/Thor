using Thor.Abstractions.Realtime;

namespace Thor.AzureOpenAI.Realtime;

public class AzureOpenAIRealtimeService :IThorRealtimeService
{
    public IRealtimeClient CreateClient()
    {
        return new AzureOpenAIRealtimeClient();
    }
}