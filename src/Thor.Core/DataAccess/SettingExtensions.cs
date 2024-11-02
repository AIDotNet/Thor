using Microsoft.EntityFrameworkCore;
using Thor.Service.Domain;

namespace Thor.Service.DataAccess;

public static class SettingExtensions
{
    private const string Prefix = "Setting:";

    /// <summary>
    /// 业务设置
    /// </summary>
    public class GeneralSetting
    {
        public const string Default = Prefix + nameof(GeneralSetting);

        /// <summary>
        /// 充值地址
        /// </summary>
        public const string RechargeAddress = Default + ":RechargeAddress";

        /// <summary>
        /// 对话链接
        /// </summary>
        public const string ChatLink = Default + ":ChatLink";

        /// <summary>
        /// Vidol 链接
        /// </summary>
        public const string VidolLink = Default + ":VidolLink";

        /// <summary>
        /// 新用户初始额度
        /// </summary>
        public const string NewUserQuota = Default + ":NewUserQuota";

        /// <summary>
        /// 请求预扣额度
        /// </summary>
        public const string RequestQuota = Default + ":RequestQuota";

        /// <summary>
        /// 邀请奖励额度
        /// </summary>
        public const string InviteQuota = Default + ":InviteQuota";

        /// <summary>
        /// 启用定时清理日志
        /// </summary>
        public const string EnableClearLog = Default + ":EnableClearLog";

        /// <summary>
        /// 间隔天数
        /// </summary>
        public const string IntervalDays = Default + ":IntervalDays";

        /// <summary>
        /// 启用自动检测渠道策略
        /// </summary>
        public const string EnableAutoCheckChannel = Default + ":EnableAutoCheckChannel";

        /// <summary>
        /// 检测间隔 (分钟)
        /// </summary>
        public const string CheckInterval = Default + ":CheckInterval";

        /// <summary>
        /// 自动禁用异常渠道
        /// </summary>
        public const string AutoDisableChannel = Default + ":AutoDisableChannel";

        /// <summary>
        /// 支付宝支付回调地址
        /// </summary>
        public const string AlipayNotifyUrl = Default + ":AlipayNotifyUrl";

        /// <summary>
        /// 支付宝应用APPID
        /// </summary>
        public const string AlipayAppId = Default + ":AlipayAppId";

        /// <summary>
        /// 支付宝应用私钥
        /// </summary>
        public const string AlipayPrivateKey = Default + ":AlipayPrivateKey";

        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public const string AlipayPublicKey = Default + ":AlipayPublicKey";

        /// <summary>
        /// 支付宝AppCertPath
        /// </summary>
        public const string AlipayAppCertPath = Default + ":AlipayAppCertPath";

        /// <summary>
        /// 支付宝根证书路径
        /// </summary>
        public const string AlipayRootCertPath = Default + ":AlipayRootCertPath";

        /// <summary>
        /// 支付宝公钥证书路径
        /// </summary>
        public const string AlipayPublicCertPath = Default + ":AlipayPublicCertPath";
    }

    /// <summary>
    /// 系统设置
    /// </summary>
    public class SystemSetting
    {
        public const string Default = Prefix + nameof(SystemSetting);

        /// <summary>
        /// 服务器地址
        /// </summary>
        public const string ServerAddress = Default + ":ServerAddress";

        /// <summary>
        /// 启用账号注册
        /// </summary>
        public const string EnableRegister = Default + ":EnableRegister";

        /// <summary>
        /// 允许Github登录
        /// </summary>
        public const string EnableGithubLogin = Default + ":EnableGithubLogin";

        /// <summary>
        /// Github Client Id
        /// </summary>
        public const string GithubClientId = Default + ":GithubClientId";

        /// <summary>
        /// Github Client Secret
        /// </summary>
        public const string GithubClientSecret = Default + ":GithubClientSecret";

        /// <summary>
        /// 允许Gitee登录
        /// </summary>
        public const string EnableGiteeLogin = Default + ":EnableGiteeLogin";

        /// <summary>
        /// Gitee Client Id
        /// </summary>
        public const string GiteeClientId = Default + ":GiteeClientId";

        /// <summary>
        /// Gitee Client Secret
        /// </summary>
        public const string GiteeClientSecret = Default + ":GiteeClientSecret";

        /// <summary>
        /// Gitee redirect_uri
        /// </summary>
        public const string GiteeRedirectUri = Default + ":GiteeRedirectUri";

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public const string EmailAddress = Default + ":EmailAddress";

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public const string EmailPassword = Default + ":EmailPassword";

        /// <summary>
        /// SMTP地址
        /// </summary>
        public const string SmtpAddress = Default + ":SmtpAddress";

        /// <summary>
        /// 启用邮箱验证注册
        /// </summary>
        public const string EnableEmailRegister = Default + ":EnableEmailRegister";
    }

    /// <summary>
    /// 其他设置
    /// </summary>
    public class OtherSetting
    {
        public const string Default = Prefix + nameof(OtherSetting);

        /// <summary>
        /// 网站标题
        /// </summary>
        public const string WebTitle = Default + ":WebTitle";

        /// <summary>
        /// 网站Logo地址
        /// </summary>
        public const string WebLogo = Default + ":WebLogo";

        /// <summary>
        /// 首页内容
        /// </summary>
        public const string IndexContent = Default + ":IndexContent";
    }

    public static void InitSetting(this ModelBuilder modelBuilder)
    {
        var settings = new List<Setting>();

        #region 业务设置

        settings.Add(new Setting
        {
            Key = GeneralSetting.RechargeAddress,
            Value = "",
            Description = "充值地址"
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.ChatLink,
            Value = "",
            Description = "对话链接"
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.VidolLink,
            Value = "",
            Description = "Vidol 链接"
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.NewUserQuota,
            Value = "100000",
            Description = "新用户初始额度",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.RequestQuota,
            Value = "2000",
            Description = "请求预扣额度",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.InviteQuota,
            Value = "100000",
            Description = "邀请奖励额度",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.EnableClearLog,
            Value = "true",
            Description = "启用定时清理日志",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.IntervalDays,
            Value = "90",
            Description = "间隔天数",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.EnableAutoCheckChannel,
            Value = "false",
            Description = "启用自动检测渠道策略",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.CheckInterval,
            Value = "60",
            Description = "检测间隔 (分钟)",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.AutoDisableChannel,
            Value = "false",
            Description = "自动禁用异常渠道",
            Private = true
        });


        settings.Add(new Setting
        {
            Key = GeneralSetting.AlipayNotifyUrl,
            Value = "https://您的服务器地址/",
            Description = "支付宝支付回调地址"
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.AlipayAppId,
            Value = "",
            Description = "支付宝应用APPID",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.AlipayPrivateKey,
            Value = "",
            Description = "支付宝应用私钥",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.AlipayPublicKey,
            Value = "",
            Description = "支付宝公钥",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.AlipayAppCertPath,
            Value = "",
            Description = "支付宝AppCertPath",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.AlipayRootCertPath,
            Value = "",
            Description = "支付宝AlipayRootCertPath",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.AlipayPublicCertPath,
            Value = "",
            Description = "支付宝公钥证书路径",
            Private = true
        });

        #endregion

        #region 系统设置

        settings.Add(new Setting
        {
            Key = SystemSetting.ServerAddress,
            Value = "",
            Description = "服务器地址"
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.EnableRegister,
            Value = "true",
            Description = "启用账号注册"
        });
        
        settings.Add(new Setting
        {
            Key = SystemSetting.EnableGithubLogin,
            Value = "true",
            Description = "允许Github登录"
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.GithubClientId,
            Value = "",
            Description = "Github Client Id",
            Private = false
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.GithubClientSecret,
            Value = "",
            Description = "Github Client Secret",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.EnableGiteeLogin,
            Value = "true",
            Description = "允许Gitee登录"
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.GiteeClientId,
            Value = "",
            Description = "Gitee Client Id",
            Private = false
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.GiteeClientSecret,
            Value = "",
            Description = "Gitee Client Secret",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.GiteeRedirectUri,
            Value = "",
            Description = "Gitee redirect_uri",
            Private = false
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.EmailAddress,
            Value = "",
            Description = "邮箱地址",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.EmailPassword,
            Value = "",
            Description = "邮箱密码",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.SmtpAddress,
            Value = "",
            Description = "SMTP地址",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.EnableEmailRegister,
            Value = "false",
            Description = "启用邮箱验证注册",
            Private = false,
        });

        #endregion

        #region 其他

        settings.Add(new Setting
        {
            Key = OtherSetting.WebTitle,
            Value = "AIDtoNet API",
            Description = "网站标题"
        });

        settings.Add(new Setting
        {
            Key = OtherSetting.WebLogo,
            Value = "/logo.png",
            Description = "网站Logo地址"
        });

        settings.Add(new Setting
        {
            Key = OtherSetting.IndexContent,
            Value = "AI DotNet API 提供更强的兼容，将更多的AI平台接入到AI DotNet API中，让AI集成更加简单。",
            Description = "首页内容"
        });

        #endregion

        modelBuilder.Entity<Setting>().HasData(settings);
    }
}