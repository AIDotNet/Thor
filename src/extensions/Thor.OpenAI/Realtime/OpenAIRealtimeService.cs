using Thor.Abstractions.Realtime;

namespace Thor.DeepSeek.Realtime;

public class OpenAIRealtimeService :IThorRealtimeService
{
    public IRealtimeClient CreateClient()
    {
        return new OpenAIRealtimeClient();
    }
}