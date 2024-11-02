
export function renderQuota(quota: number, digits = 2) {
    let quotaPerUnit = localStorage.getItem('quota_per_unit') ?? '500000';
    let displayInCurrency = localStorage.getItem('display_in_currency') ?? "true";
    if (displayInCurrency === 'true') {
        return '$' + (quota / parseFloat(quotaPerUnit as string)).toFixed(digits);
    }
    return renderNumber(quota);
}

export function renderQuotaWithPrompt(quota: any, digits: number | undefined) {
    let displayInCurrency = localStorage.getItem('display_in_currency');
    if ((displayInCurrency === 'true')) {
        return `（等价金额：${renderQuota(quota, digits)}）`;
    }
    return '';
}


export function renderNumber(num: number) {
    if (num >= 1000000000) {
        return (num / 1000000000).toFixed(1) + 'B';
    } else if (num >= 1000000) {
        return (num / 1000000).toFixed(1) + 'M';
    } else if (num >= 10000) {
        return (num / 1000).toFixed(1) + 'k';
    } else {
        return num;
    }
}


/**
 * 获取类型获取指定的提示
 * @param type 
 * @returns 
 */
export function getModelPrompt(type: string) {

    if (type === "SparkDesk") {
        return "密钥格式 AppId|AppKey|AppSecret";
    } else if (type === "Hunyuan") {
        return "密钥格式 secretId|secretKey";
    } else if (type === "ErnieBot") {
        return "密钥格式 ClientId|ClientSecret"
    }

    return "请输入密钥";
}

export function getCompletionRatio(name: string) {
    if (name.startsWith("gpt-3.5")) {
        if (name === "gpt-3.5-turbo" || name.endsWith("0125")) return 3;
        if (name.endsWith("1106")) return 2;
        return 4.0 / 3.0;
    }

    if (name.startsWith("gpt-4")) return name.startsWith("gpt-4-turbo") ? 3 : 2;

    if (name.startsWith("claude-")) return name.startsWith("claude-3") ? 5 : 3;

    if (name.startsWith("mistral-") || name.startsWith("gemini-")) return 3;

    switch (name) {
        case "llama2-70b-4096":
            return 0.8 / 0.7;
        default:
            return 1;
    }
}