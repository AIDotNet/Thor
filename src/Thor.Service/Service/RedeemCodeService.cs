using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure;

namespace Thor.Service.Service;

public class RedeemCodeService(
    IServiceProvider serviceProvider,
    LoggerService loggerService,
    UserService userService)
    : ApplicationService(serviceProvider),IScopeDependency
{
    public async Task<IEnumerable<string>> CreateAsync(RedeemCodeInput input)
    {
        if (input.Count <= 0) throw new ArgumentException("生成数量必须大于0");

        if (input.Quota <= 0) throw new ArgumentException("额度必须大于0");

        var codes = new List<RedeemCode>();
        for (var i = 0; i < input.Count; i++)
        {
            var code = new RedeemCode(input.Name, input.Quota)
            {
                Id = Guid.NewGuid().ToString("N")
            };
            codes.Add(code);
        }

        await DbContext.RedeemCodes.AddRangeAsync(codes);

        return codes.Select(x => x.Code);
    }

    public async ValueTask<PagingDto<RedeemCode>> GetAsync(int page, int pageSize, string? keyword)
    {
        var total = await DbContext.RedeemCodes.CountAsync(x =>
            string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword));

        if (total > 0)
        {
            var result = await DbContext.RedeemCodes
                .AsNoTracking()
                .Where(x => string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword))
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDto<RedeemCode>(total, result);
        }

        return new PagingDto<RedeemCode>(0, new List<RedeemCode>());
    }

    public async ValueTask EnableAsync(string id)
    {
        await DbContext.RedeemCodes
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Disabled, x => !x.Disabled));
    }

    public async ValueTask RemoveAsync(string id)
    {
        await DbContext.RedeemCodes
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    public async ValueTask UpdateAsync(string id, RedeemCodeInput input)
    {
        await DbContext.RedeemCodes
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(item => item.Name, item => input.Name)
                    .SetProperty(item => item.Quota, item => input.Quota));
    }

    public async ValueTask UseAsync(string code)
    {
        var redeemCode = await DbContext.RedeemCodes
            .FirstOrDefaultAsync(x => x.Code == code);

        if (redeemCode == null) throw new Exception("兑换码不存在或已禁用");

        if (redeemCode.State == RedeemedState.Redeemed) throw new Exception("兑换码已使用");

        if (redeemCode.Disabled) throw new Exception("兑换码已禁用");

        await DbContext.RedeemCodes
            .Where(x => x.Code == code && x.State == RedeemedState.NotRedeemed && !x.Disabled)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.State, RedeemedState.Redeemed)
                .SetProperty(x => x.RedeemedTime, DateTime.Now)
                .SetProperty(x => x.RedeemedUserId, UserContext.CurrentUserId)
                .SetProperty(x => x.RedeemedUserName, UserContext.CurrentUserName));

        await userService.UpdateResidualCreditAsync(UserContext.CurrentUserId, redeemCode.Quota);

        await loggerService.CreateAsync(new ChatLogger
        {
            Type = ThorChatLoggerType.System,
            ModelName = string.Empty,
            Content =
                $"用户 {UserContext.CurrentUserName} 使用了兑换码 {redeemCode.Code}，兑换额度 {RenderHelper.RenderQuota(redeemCode.Quota)}"
        });
    }
}