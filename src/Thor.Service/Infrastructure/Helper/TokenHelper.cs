﻿using SharpToken;

namespace Thor.Service.Infrastructure.Helper;

public static class TokenHelper
{
    private static readonly GptEncoding GptEncoding;

    static TokenHelper()
    {
        GptEncoding ??= GptEncoding.GetEncodingForModel("gpt-4");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static int GetTotalTokens(params string[] content)
    {
        return content.Sum(GetTokens);
    }

    public static int GetTokens(string content)
    {
        return GptEncoding.CountTokens(content);
    }
}