using System.Globalization;
using System.Text.Json;
using StackExchange.Redis;

namespace Thor.RedisMemory.Cache;

public class RedisValueConvert
{
    public static T ConvertTo<T>(RedisValue value)
    {
        // 将RedisValue转换为指定类型
        if (value.IsNull)
        {
            return default!;
        }

        if (typeof(T) == typeof(string))
        {
            return (T)(object)value.ToString();
        }

        if (typeof(T) == typeof(int))
        {
            return (T)(object)int.Parse(value.ToString());
        }

        if (typeof(T) == typeof(long))
        {
            return (T)(object)long.Parse(value.ToString());
        }

        if (typeof(T) == typeof(double))
        {
            return (T)(object)double.Parse(value.ToString(), CultureInfo.CurrentCulture);
        }

        if (typeof(T) == typeof(float))
        {
            return (T)(object)float.Parse(value.ToString(), CultureInfo.CurrentCulture);
        }

        if (typeof(T) == typeof(decimal))
        {
            return (T)(object)decimal.Parse(value.ToString(), CultureInfo.CurrentCulture);
        }

        if (typeof(T) == typeof(byte[]))
        {
            return (T)(object)value;
        }

        return JsonSerializer.Deserialize<T>(value);
    }

    public static RedisValue ConvertToRedisValue(object value)
    {
        // 将对象转换为RedisValue，它可能是字符串、二进制数据或其他类型
        if (value == null)
        {
            return RedisValue.Null;
        }

        if (value is string str)
        {
            return str;
        }

        if (value is int intValue)
        {
            return new RedisValue(intValue.ToString());
        }

        if (value is long longValue)
        {
            return new RedisValue(longValue.ToString());
        }

        if (value is double doubleValue)
        {
            return new RedisValue(doubleValue.ToString(CultureInfo.CurrentCulture));
        }

        if (value is float floatValue)
        {
            return new RedisValue(floatValue.ToString(CultureInfo.CurrentCulture));
        }

        if (value is decimal decimalValue)
        {
            return new RedisValue(decimalValue.ToString(CultureInfo.CurrentCulture));
        }

        if (value is byte[] bytes)
        {
            return bytes;
        }

        return new RedisValue(JsonSerializer.Serialize(value));
    }
}