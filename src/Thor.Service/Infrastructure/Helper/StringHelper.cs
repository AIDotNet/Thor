using System.Security.Cryptography;
using System.Text;

namespace Thor.Service.Infrastructure.Helper;

/// <summary>
/// 字符串工具
/// </summary>
public static class StringHelper
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    /// <summary>
    /// 加密密码
    /// </summary>
    /// <param Name="password"></param>
    /// <param Name="salt"></param>
    /// <returns></returns>
    public static string HashPassword(string password, string salt)
    {
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var saltedPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];
        Array.Copy(saltBytes, 0, saltedPasswordBytes, 0, saltBytes.Length);
        Array.Copy(passwordBytes, 0, saltedPasswordBytes, saltBytes.Length, passwordBytes.Length);
        var hashedBytes = MD5.HashData(saltedPasswordBytes);
        var sb = new StringBuilder();
        foreach (var t in hashedBytes)
        {
            sb.Append(t.ToString("x2"));
        }

        return sb.ToString();
    }

    // 生成指定长度的随机字符串
    public static string GenerateRandomString(int length)
    {
        return new string(Enumerable.Repeat(Chars, length)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}