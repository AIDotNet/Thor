import { Setting } from '../index.d'

import { get, putJson } from "../uitls/fetch";

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
     */
    static readonly RechargeAddress: string = GeneralSetting.Default + ":RechargeAddress";

    /**
     * 对话链接
     */
    static readonly ChatLink: string = GeneralSetting.Default + ":ChatLink";

    /**
     * 新用户初始额度
     */
    static readonly NewUserQuota: string = GeneralSetting.Default + ":NewUserQuota";

    /**
     * 请求预扣额度
     */
    static readonly RequestQuota: string = GeneralSetting.Default + ":RequestQuota";

    /**
     * 邀请奖励额度
     */
    static readonly InviteQuota: string = GeneralSetting.Default + ":InviteQuota";

    /**
     * 启用定时清理日志
     */
    static readonly EnableClearLog: string = GeneralSetting.Default + ":EnableClearLog";

    /**
     * 间隔天数
     */
    static readonly IntervalDays: string = GeneralSetting.Default + ":IntervalDays";

    /**
     * 启用自动检测渠道策略
     */
    static readonly EnableAutoCheckChannel: string = GeneralSetting.Default + ":EnableAutoCheckChannel";

    /**
     * 检测间隔 (分钟)
     */
    static readonly CheckInterval: string = GeneralSetting.Default + ":CheckInterval";

    /**
     * 自动禁用异常渠道
     */
    static readonly AutoDisableChannel: string = GeneralSetting.Default + ":AutoDisableChannel";

    /**
     * 模型倍率Prompt
     */
    static readonly ModelPromptRate: string = GeneralSetting.Default + ":ModelPromptRate";

    /**
     * 模型倍率Completion
     */
    static readonly ModelCompletionRate: string = GeneralSetting.Default + ":ModelCompletionRate";
}

/**
 * 系统设置
 */
class SystemSetting {
    static readonly Default: string = SettingPrefix.Prefix + "SystemSetting";

    /**
     * 服务器地址
     */
    static readonly ServerAddress: string = SystemSetting.Default + ":ServerAddress";

    /**
     * 启用账号注册
     */
    static readonly EnableRegister: string = SystemSetting.Default + ":EnableRegister";

    /**
     * 允许Github登录
     */
    static readonly EnableGithubLogin: string = SystemSetting.Default + ":EnableGithubLogin";

    /**
     * Github Client Id
     */
    static readonly GithubClientId: string = SystemSetting.Default + ":GithubClientId";

    /**
     * Github Client Secret
     */
    static readonly GithubClientSecret: string = SystemSetting.Default + ":GithubClientSecret";
}

/**
 * 其他设置
 */
class OtherSetting {
    static readonly Default: string = SettingPrefix.Prefix + "OtherSetting";

    /**
     * 网站标题
     */
    static readonly WebTitle: string = OtherSetting.Default + ":WebTitle";

    /**
     * 网站Logo地址
     */
    static readonly WebLogo: string = OtherSetting.Default + ":WebLogo";

    /**
     * 首页内容
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

let InitSetting: Setting[];

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

export {
    GeneralSetting,
    SystemSetting,
    OtherSetting,
    InitSetting
}