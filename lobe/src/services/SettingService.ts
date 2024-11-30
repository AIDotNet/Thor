
import { get, putJson } from "../utils/fetch";

class SettingPrefix {
    static readonly Prefix: string = "Setting:";
}

/**
 * 业务设置
 */
class GeneralSetting {
    static readonly Default: string = SettingPrefix.Prefix + "GeneralSetting";

    /**
     * 充值地址
     * @type {string}
     */
    static readonly RechargeAddress: string = GeneralSetting.Default + ":RechargeAddress";

    /**
     * 对话链接
     * @type {string}
     */
    static readonly ChatLink: string = GeneralSetting.Default + ":ChatLink";

    /**
     * 新用户初始额度
     * @type {string}
     */
    static readonly NewUserQuota: string = GeneralSetting.Default + ":NewUserQuota";

    /**
     * 请求预扣额度
     * @type {string}
     */
    static readonly RequestQuota: string = GeneralSetting.Default + ":RequestQuota";

    /**
     * 邀请奖励额度
     * @type {string}
     */
    static readonly InviteQuota: string = GeneralSetting.Default + ":InviteQuota";

    /**
     * 启用定时清理日志
     * @type {string}
     */
    static readonly EnableClearLog: string = GeneralSetting.Default + ":EnableClearLog";

    /**
     * 间隔天数
     * @type {string}
     */
    static readonly IntervalDays: string = GeneralSetting.Default + ":IntervalDays";

    /**
     * 启用自动检测渠道策略
     * @type {string}
     */
    static readonly EnableAutoCheckChannel: string = GeneralSetting.Default + ":EnableAutoCheckChannel";

    /**
     * 检测间隔 (分钟)
     * @type {string}
     */
    static readonly CheckInterval: string = GeneralSetting.Default + ":CheckInterval";

    /**
     * 自动禁用异常渠道
     * @type {string}
     */
    static readonly AutoDisableChannel: string = GeneralSetting.Default + ":AutoDisableChannel";

    /**
     * 支付宝回调地址
     * @type {string}
     */
    static readonly AlipayNotifyUrl: string = GeneralSetting.Default + ":AlipayNotifyUrl";

    /**
     * 支付宝AppId
     * @type {string}
     */
    static readonly AlipayAppId: string = GeneralSetting.Default + ":AlipayAppId";

    /**
     * 支付宝应用私钥
     * @type {string}
     */
    static readonly AlipayPrivateKey: string = GeneralSetting.Default + ":AlipayPrivateKey";

    /**
     * 支付宝公钥
     * @type {string}
     */
    static readonly AlipayPublicKey: string = GeneralSetting.Default + ":AlipayPublicKey";

    /**
     * 支付宝应用公钥证书路径
     * @type {string}
     */
    static readonly AlipayAppCertPath: string = GeneralSetting.Default + ":AlipayAppCertPath";

    /**
     * 支付宝根证书路径
     * @type {string}
     */
    static readonly AlipayRootCertPath: string = GeneralSetting.Default + ":AlipayRootCertPath";

    /**
     * 支付宝公钥证书路径
     * @type {string}
     */
    static readonly AlipayPublicCertPath: string = GeneralSetting.Default + ":AlipayPublicCertPath";
}

/**
 * 系统设置
 */
class SystemSetting {
    static readonly Default: string = SettingPrefix.Prefix + "SystemSetting";

    /**
     * 服务器地址
     * @type {string}
     */
    static readonly ServerAddress: string = SystemSetting.Default + ":ServerAddress";

    /**
     * 主题
     * @type {string}
     */
    static readonly Theme: string = SystemSetting.Default + ":Theme";

    /**
     * 启用账号注册
     * @type {string}
     */
    static readonly EnableRegister: string = SystemSetting.Default + ":EnableRegister";

    /**
     * 允许Github登录
     * @type {string}
     */
    static readonly EnableGithubLogin: string = SystemSetting.Default + ":EnableGithubLogin";

    /**
     * Github Client Id
     * @type {string}
     */
    static readonly GithubClientId: string = SystemSetting.Default + ":GithubClientId";

    /**
     * Github Client Secret
     * @type {string}
     */
    static readonly GithubClientSecret: string = SystemSetting.Default + ":GithubClientSecret";
    
    /**
     * 启用Gitee登录
     * @type {string}
     */
    static readonly EnableGiteeLogin: string = SystemSetting.Default + ":EnableGiteeLogin";

    /**
     * Gitee Client Id
     * @type {string}
     */
    static readonly GiteeClientId: string = SystemSetting.Default + ":GiteeClientId";

    /**
     * Gitee Client Secret
     * @type {string}
     * @static
     * @memberof SystemSetting
     */
    static readonly GiteeClientSecret: string = SystemSetting.Default + ":GiteeClientSecret";

    /**
     * Gitee redirect_uri
     * @type {string}
     * @static
     * @memberof SystemSetting
     */
    static readonly GiteeRedirectUri: string = SystemSetting.Default + ":GiteeRedirectUri";

    /**
     * 邮箱发送地址
     * @type {string}
     */
    static readonly EmailAddress: string = SystemSetting.Default + ":EmailAddress";

    /**
     * 邮箱发送密码
     * @type {string}
     */
    static readonly EmailPassword: string = SystemSetting.Default + ":EmailPassword";

    /**
     * 邮箱SMTP地址
     * @type {string}
     */
    static readonly SmtpAddress: string = SystemSetting.Default + ":SmtpAddress";

    /**
     * 启用邮箱注册验证
     * @type {string}
     */
    static readonly EnableEmailRegister: string = SystemSetting.Default + ":EnableEmailRegister";

    
    /**
     * Casdoor Client Id
     * @type {string}
     */
    static readonly CasdoorClientId: string = SystemSetting.Default + ":CasdoorClientId";

    
    /**
     * 启用Casdoor授权
     * @type {string}
     */
    static readonly EnableCasdoorAuth: string = SystemSetting.Default + ":EnableCasdoorAuth";

    
    /**
     * Casdoor 自定义端点
     * @type {string}
     */
    static readonly CasdoorEndipoint: string = SystemSetting.Default + ":CasdoorEndipoint";

    
    /**
     * Casdoor Client Secret
     * @type {string}
     */
    static readonly CasdoorClientSecret: string = SystemSetting.Default + ":CasdoorClientSecret";

    
}

/**
 * 其他设置
 */
class OtherSetting {
    static readonly Default: string = SettingPrefix.Prefix + "OtherSetting";

    /**
     * 网站标题
     * @type {string}
     */
    static readonly WebTitle: string = OtherSetting.Default + ":WebTitle";

    /**
     * 网站Logo地址
     * @type {string}
     */
    static readonly WebLogo: string = OtherSetting.Default + ":WebLogo";

    /**
     * 首页内容
     * @type {string}
     */
    static readonly IndexContent: string = OtherSetting.Default + ":IndexContent";
}

const prefix = "/api/v1/setting";

/**
 * 获取设置
 * @returns Promise
 */
export const GetSetting = () => {
    return get(prefix)
}

/**
 * 更新设置
 * @param data 设置数据
 * @returns Promise
 */
export const UpdateSetting = (data: any) => {
    return putJson(prefix, data);
}

let InitSetting: any[];

try {

    InitSetting = (await GetSetting()).data;

    // 初始化title
    const title = InitSetting?.find(s => s.key === OtherSetting.WebTitle)?.value;

    if (title) {
        document.title = title;
    }
} catch (e) {
    console.log(e);

}

/**
 * 是否启用了支付宝支付
 */
function IsEnableAlipay() {
    const v = InitSetting?.find(s => s.key === GeneralSetting.AlipayNotifyUrl);
    return v?.value !== undefined && v?.value !== "";
}

function IsEnableEmailRegister() {
    const v = InitSetting?.find(s => s.key === SystemSetting.EnableEmailRegister);
    return v?.value === "true";
}

export {
    GeneralSetting,
    SystemSetting,
    OtherSetting,
    IsEnableEmailRegister,
    InitSetting,
    IsEnableAlipay
}