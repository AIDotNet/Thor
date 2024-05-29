declare var window: any;

// vite判断是否是开发环境
export const isDev = import.meta.env.DEV;

export const isOnServerSide = typeof window === 'undefined';
