using System.Runtime.CompilerServices;
using SharpToken;

namespace Thor.Service.Infrastructure.Helper;

public static class TokenHelper
{
    private static readonly GptEncoding GptEncoding;

    static TokenHelper()
    {
        GptEncoding ??= GptEncoding.GetEncoding("cl100k_base");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTotalTokens(params string[] content)
    {
        return content.Sum((s => RefGetTokens(ref s)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RefGetTokens(ref string content)
    {
        return GptEncoding.CountTokens(content);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTokens(string content)
    {
        return GptEncoding.CountTokens(content);
    }
}