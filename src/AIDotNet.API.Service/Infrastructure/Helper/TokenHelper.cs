using SharpToken;

namespace AIDotNet.API.Service.Infrastructure.Helper;

public static class TokenHelper
{
    private static GptEncoding _gptEncoding;

    static TokenHelper()
    {
        _gptEncoding ??= GptEncoding.GetEncodingForModel("gpt-4");
    }

    public static int GetTotalTokens(params string[] content)
    {
        int token = 0;
        foreach (var item in content)
        {
            token += _gptEncoding.Encode(item).Count;
        }

        return token;
    }

    public static int GetTokens(string content)
    {
        return _gptEncoding.Encode(content).Count;
    }
}