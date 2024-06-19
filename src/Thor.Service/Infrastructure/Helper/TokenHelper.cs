using SharpToken;

namespace Thor.Service.Infrastructure.Helper;

public static class TokenHelper
{
    private static readonly GptEncoding GptEncoding;

    static TokenHelper()
    {
        GptEncoding ??= GptEncoding.GetEncodingForModel("gpt-4");
    }

    public static int GetTotalTokens(params string[] content)
    {
        return content.Sum(item => GptEncoding.Encode(item).Count);
    }

    public static int GetTokens(string content)
    {
        return GptEncoding.Encode(content).Count;
    }
}