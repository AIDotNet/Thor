using Aop.Api;
using Aop.Api.Request;
using Newtonsoft.Json;
using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Thor.Service.Service;

public class ProductService(IServiceProvider serviceProvider, LoggerService loggerService, UserService userService)
    : ApplicationService(serviceProvider), IScopeDependency
{
    public async ValueTask<List<Product>> GetProductsAsync()
    {
        return await DbContext.Products.ToListAsync();
    }

    public void Create(Product product)
    {
        product.Id = Guid.NewGuid().ToString();
        product.Price = Math.Round(product.Price, 2);

        DbContext.Products.Add(product);
    }

    public void Update(Product product)
    {
        product.Price = Math.Round(product.Price, 2);

        DbContext.Products.Update(product);
    }

    public async Task DeleteAsync(string id)
    {
        await DbContext.Products.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    /// <summary>
    ///     发起支付
    /// </summary>
    /// <param name="id">产品id</param>
    public async Task<StartPayPayloadResult> StartPayPayloadAsync(string id)
    {
        var product = await DbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product == null) throw new Exception("Product does not exist");

        var record = new ProductPurchaseRecord
        {
            ProductId = id,
            Id = Guid.NewGuid().ToString("N"),
            UserId = UserContext.CurrentUserId,
            Description = $"{UserContext.CurrentUserName} 创建订单：{product.Name} 订单金额：{product.Price}元",
            Status = ProductPurchaseStatus.Unpaid,
            Quantity = 1,
            RemainQuota = product.RemainQuota
        };

        DbContext.ProductPurchaseRecords.Add(record);

        // 获取支付宝回调地址
        var alipayNotifyUrl = SettingService.GetSetting(SettingExtensions.GeneralSetting.AlipayNotifyUrl);


        // 调用支付宝支付
        AlipayTradePrecreateRequest request = new();
        request.SetNotifyUrl(alipayNotifyUrl.TrimEnd('/') + "/api/v1/product/pay-complete-callback"); // 支付成功回调地址
        Dictionary<string, object> bizContent = new()
        {
            { "out_trade_no", record.Id },
            { "total_amount", product.Price },
            { "subject", product.Name }
        };

        var client = GetAopClient();

        request.BizContent = JsonConvert.SerializeObject(bizContent);
        var response = client.CertificateExecute(request);


        var body = JsonSerializer.Deserialize<AliPayResponseDto>(response.Body);

        return new StartPayPayloadResult
        {
            Qr = body?.alipay_trade_precreate_response.qr_code ?? "",
            Id = record.Id
        };
    }

    public async Task PayCompleteCallbackAsync(HttpContext context)
    {
        var form = context.Request.Form;
        Dictionary<string, string> dictionary = new();
        var keys = form.Keys;
        if (keys != null)
            foreach (var key in keys)
                dictionary.Add(key, form[key]);

        var logger = GetService<ILogger<ProductService>>();
        logger.LogWarning("支付成功回调：{Data}", JsonSerializer.Serialize(dictionary));

        if (keys.Count == 0)
        {
            logger.LogWarning("支付成功回调参数为空");
            return;
        }

        if (dictionary.TryGetValue("trade_status", out var tradeStatus) && tradeStatus == "TRADE_SUCCESS")
        {
            var outTradeNo = dictionary["out_trade_no"]; //获取ali传过来的参数的值

            var product = await DbContext.ProductPurchaseRecords.FirstOrDefaultAsync(x =>
                x.Id == outTradeNo && x.Status != ProductPurchaseStatus.Paid);

            if (product == null)
            {
                logger.LogWarning("支付成功回调订单不存在：{Data}", JsonSerializer.Serialize(dictionary));
                return;
            }

            await DbContext.ProductPurchaseRecords
                .Where(x => x.Id == outTradeNo && x.Status != ProductPurchaseStatus.Paid)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(x => x.Status, ProductPurchaseStatus.Paid));

            await DbContext.Users.Where(x => x.Id == product!.UserId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(x => x.ResidualCredit, y => y.ResidualCredit + product!.RemainQuota));

            logger.LogWarning("支付成功回调订单状态更新成功：{Data}", JsonSerializer.Serialize(dictionary));

            var user = await userService.RefreshUserAsync(product.UserId);

            await loggerService.CreateRechargeAsync(
                $"订单：{product.Description} 支付成功，充值{RenderHelper.RenderQuota(product.RemainQuota, 6)}额度，用户：{user?.UserName}",
                (int)product.RemainQuota, product.UserId);
        }
        else
        {
            logger.LogWarning("支付成功回调交易状态不是TRADE_SUCCESS：{Data}",
                JsonSerializer.Serialize(dictionary));
        }
    }

    public IAopClient GetAopClient()
    {
        var appid = SettingService.GetSetting(SettingExtensions.GeneralSetting.AlipayAppId);
        var privateKey = SettingService.GetSetting(SettingExtensions.GeneralSetting.AlipayPrivateKey);
        var publicKey = SettingService.GetSetting(SettingExtensions.GeneralSetting.AlipayPublicKey);
        var appCertPath = SettingService.GetSetting(SettingExtensions.GeneralSetting.AlipayAppCertPath);
        var rootCertPath = SettingService.GetSetting(SettingExtensions.GeneralSetting.AlipayRootCertPath);
        var alipayPublicCertPath = SettingService.GetSetting(SettingExtensions.GeneralSetting.AlipayPublicCertPath);

        var config = new AlipayConfig
        {
            //设置网关地址
            ServerUrl = "https://openapi.alipay.com/gateway.do",
            //设置应用APPID
            AppId = appid,
            //设置应用私钥
            PrivateKey = privateKey,
            //设置请求格式，固定值json
            Format = "json",
            //设置字符集
            Charset = "utf-8",
            //设置支付宝公钥
            AlipayPublicKey = publicKey,
            //设置签名类型
            SignType = "RSA2",
            AppCertPath = appCertPath,
            RootCertPath = rootCertPath,
            AlipayPublicCertPath = alipayPublicCertPath
        };

        return new DefaultAopClient(config);
    }
}