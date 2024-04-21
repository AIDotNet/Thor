using AIDotNet.API.Service.Domain;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.DataAccess;

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
        /// 模型倍率Prompt
        /// </summary>
        public const string ModelPromptRate = Default + ":ModelPromptRate";

        /// <summary>
        /// 模型倍率Completion
        /// </summary>
        public const string ModelCompletionRate = Default + ":ModelCompletionRate";

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
            Key = GeneralSetting.ModelPromptRate,
            Value =
                "{ \"360GPT_S2_V9\": 0.8572, \"BLOOMZ-7B\": 0.284, \"Baichuan2-53B\": 1.42, \"Baichuan2-Turbo\": 0.568, \"Baichuan2-Turbo-192k\": 1.136, \"ChatPro\": 7.1, \"ChatStd\": 0.71, \"ERNIE-3.5-4K-0205\": 0.852, \"ERNIE-3.5-8K\": 0.852, \"ERNIE-3.5-8K-0205\": 1.704, \"ERNIE-3.5-8K-1222\": 0.852, \"ERNIE-4.0-8K\": 8.52, \"ERNIE-Bot\": 0.8572, \"ERNIE-Bot-4\": 8.572, \"ERNIE-Bot-8K\": 1.704, \"ERNIE-Bot-8k\": 1.704, \"ERNIE-Bot-turbo\": 0.5715, \"ERNIE-Lite-8K-0308\": 0.213, \"ERNIE-Lite-8K-0922\": 0.568, \"ERNIE-Speed-128K\": 0.284, \"ERNIE-Speed-8K\": 0.284, \"ERNIE-Tiny-8K\": 0.071, \"Embedding-V1\": 0.1429, \"PaLM-2\": 1, \"SparkDesk\": 1.2858, \"SparkDesk-v1.1\": 1.2858, \"SparkDesk-v2.1\": 1.2858, \"SparkDesk-v3.1\": 1.2858, \"SparkDesk-v3.5\": 1.2858, \"abab5.5-chat\": 1.065, \"abab5.5s-chat\": 0.355, \"abab6-chat\": 7.1, \"ada\": 10, \"ali-stable-diffusion-v1.5\": 8, \"ali-stable-diffusion-xl\": 8, \"babbage\": 10, \"babbage-002\": 0.2, \"bge-large-8k\": 0.142, \"bge-large-en\": 0.142, \"bge-large-zh\": 0.142, \"chatglm_lite\": 0.1429, \"chatglm_pro\": 0.7143, \"chatglm_std\": 0.3572, \"chatglm_turbo\": 0.3572, \"claude-2\": 5.51, \"claude-2.0\": 5.51, \"claude-2.1\": 5.51, \"claude-3-haiku-20240307\": 0.125, \"claude-3-opus-20240229\": 15, \"claude-3-sonnet-20240229\": 5, \"claude-instant-1\": 0.815, \"claude-instant-1.2\": 0.4, \"code-davinci-edit-001\": 10, \"cogview-3\": 17.75, \"curie\": 10, \"dall-e-2\": 8, \"dall-e-3\": 20, \"davinci\": 10, \"davinci-002\": 1, \"embedding-2\": 0.0355, \"embedding-bert-512-v1\": 0.0715, \"embedding_s1_v1\": 0.0715, \"gemini-1.0-pro-001\": 1, \"gemini-1.0-pro-vision-001\": 1, \"gemini-1.5-pro\": 1, \"gemini-pro\": 1, \"gemini-pro-vision\": 1, \"gemma-7b-it\": 0.05, \"glm-3-turbo\": 0.355, \"glm-4\": 7.1, \"glm-4v\": 7.1, \"gpt-3.5-turbo\": 0.25, \"gpt-3.5-turbo-0125\": 0.25, \"gpt-3.5-turbo-0301\": 0.75, \"gpt-3.5-turbo-0613\": 0.75, \"gpt-3.5-turbo-1106\": 0.75, \"gpt-3.5-turbo-16k\": 1.5, \"gpt-3.5-turbo-16k-0613\": 1.5, \"gpt-3.5-turbo-instruct\": 0.75, \"gpt-4\": 15, \"gpt-4-0125-preview\": 5, \"gpt-4-0314\": 15, \"gpt-4-0613\": 15, \"gpt-4-1106-preview\": 5, \"gpt-4-turbo-2024-04-09\": 5, \"gpt-4-32k\": 30, \"gpt-4-32k-0314\": 30, \"gpt-4-32k-0613\": 30, \"gpt-4-all\": 15, \"gpt-4-gizmo-*\": 15, \"gpt-4-turbo-preview\": 5, \"gpt-4-vision-preview\": 5, \"hunyuan\": 7.143, \"llama2-70b-4096\": 0.35, \"llama2-7b-2048\": 0.05, \"mistral-embed\": 0.05, \"mistral-large-latest\": 4, \"mistral-medium-latest\": 1.35, \"mistral-small-latest\": 1, \"mixtral-8x7b-32768\": 0.135, \"moonshot-v1-128k\": 4.26, \"moonshot-v1-32k\": 1.704, \"moonshot-v1-8k\": 0.852, \"open-mistral-7b\": 0.125, \"open-mixtral-8x7b\": 0.35, \"qwen-max\": 1.4286, \"qwen-max-longcontext\": 1.4286, \"qwen-plus\": 1.4286, \"qwen-turbo\": 0.5715, \"search-serper\": 0.00001, \"semantic_similarity_s1_v1\": 0.0715, \"step-1-200k\": 10.65, \"step-1-32k\": 1.704, \"step-1v-32k\": 1.704, \"tao-8k\": 0.142, \"text-ada-001\": 0.2, \"text-babbage-001\": 0.25, \"text-curie-001\": 1, \"text-davinci-002\": 10, \"text-davinci-003\": 10, \"text-davinci-edit-001\": 10, \"text-embedding-3-large\": 0.065, \"text-embedding-3-small\": 0.5, \"text-embedding-ada-002\": 0.1, \"text-embedding-v1\": 0.05, \"text-moderation-latest\": 0.1, \"text-moderation-stable\": 0.1, \"text-search-ada-doc-001\": 10, \"tts-1\": 7.5, \"tts-1-1106\": 7.5, \"tts-1-hd\": 15, \"tts-1-hd-1106\": 15, \"wanx-v1\": 8, \"whisper-1\": 15, \"yi-34b-chat-0205\": 0.1775, \"yi-34b-chat-200k\": 0.852, \"yi-vl-plus\": 0.426, \"llama3:8b\": 1, \"llama3:70b\": 1 }",
            Description = "模型倍率Prompt",
            Private = true
        });

        settings.Add(new Setting
        {
            Key = GeneralSetting.ModelCompletionRate,
            Value = "{}",
            Description = "模型倍率Completion",
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
            Private = true
        });

        settings.Add(new Setting
        {
            Key = SystemSetting.GithubClientSecret,
            Value = "",
            Description = "Github Client Secret",
            Private = true
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