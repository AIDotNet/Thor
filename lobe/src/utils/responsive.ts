import UAParser from 'ua-parser-js';
/**
 * check mobile device in server
 */
export const isMobileDevice = () => {
    // 获取浏览器的user-agent
    const ua = navigator.userAgent;
    const device = new UAParser(ua || '').getDevice();
    return device.type === 'mobile';
  };
  