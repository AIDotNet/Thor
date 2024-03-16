// 获取环境变量
export const config = {
  FAST_API_URL: import.meta.env.VITE_API_URL ?? "",
  NODE_ENV: import.meta.env.MODE,
  DEV: import.meta.env.DEV,
  VITE_GITHUB: import.meta.env.VITE_GITHUB ?? "https://gitee.com/hejiale010426/token-api",
};
