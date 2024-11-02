namespace Thor.Service.Infrastructure;

public class RenderHelper
{
    public static string RenderQuota(decimal quota, int digits = 2)
    {
        var quotaPerUnit = "500000";
        return "$" + (quota / decimal.Parse(quotaPerUnit)).ToString("F" + digits);
    }
}