using Thor.Abstractions.Realtime;

namespace Thor.OpenAI.Realtime;

public class OpenAIRealtimeService :IThorRealtimeService
{
    public IRealtimeClient CreateClient()
    {
        return new OpenAIRealtimeClient();
    }
}