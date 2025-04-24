import i18n from '../i18n';

/**
 * 获取国际化文本，可在非React组件中使用
 * @param key 翻译键
 * @param options 翻译选项，支持插值
 * @returns 翻译后的文本
 */
export const t = (key: string, options?: Record<string, any>): string => {
  return i18n.t(key, options);
};

/**
 * 切换当前语言
 * @param lng 目标语言代码，如 'zh-CN', 'en-US'
 */
export const changeLanguage = (lng: string): Promise<any> => {
  return i18n.changeLanguage(lng);
};

/**
 * 获取当前语言
 * @returns 当前语言代码
 */
export const getCurrentLanguage = (): string => {
  return i18n.language;
};

/**
 * 检查当前语言是否为中文
 * @returns 是否为中文
 */
export const isChinese = (): boolean => {
  const language = getCurrentLanguage();
  return language.startsWith('zh');
};

/**
 * 检查当前语言是否为英文
 * @returns 是否为英文
 */
export const isEnglish = (): boolean => {
  const language = getCurrentLanguage();
  return language.startsWith('en');
};

export default {
  t,
  changeLanguage,
  getCurrentLanguage,
  isChinese,
  isEnglish,
}; 