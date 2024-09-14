namespace System;

public static class TimeExtensions
{
    public static long ToUnixTimeSeconds(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
    
    public static long ToUnixTimeMilliseconds(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }
    
    public static DateTime FromUnixTimeSeconds(this long seconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
    }
}