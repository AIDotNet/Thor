import { del, get, postJson, put, putJson } from "../utils/fetch";

const prefix = "/api/v1/token";

/**
 * 添加令牌
 * @param data 令牌数据
 * @returns Promise
 */
export const Add = (data: any) => {
  return postJson(prefix, data);
};

/**
 * 删除令牌
 * @param id 令牌ID
 * @returns Promise
 */
export const Remove = (id: string) => {
  return del(prefix + "/" + id);
};

/**
 * 更新令牌
 * @param data 令牌数据
 * @returns Promise
 */
export const Update = (data: any) => {
  return putJson(prefix, data);
};

/**
 * 获取令牌列表
 * @param page 页码
 * @param pageSize 每页数量
 * @returns Promise
 */
export const getTokens = (page: number, pageSize: number) => {
  return get(prefix + "/list?page=" + page + "&pageSize=" + pageSize);
};

/**
 * 获取指定令牌
 * @param id 令牌ID
 * @returns Promise
 */
export const getToken = (id: string) => {
  return get(prefix + "/" + id);
};

/**
 * 禁用令牌
 * @param id 令牌ID
 * @returns Promise
 */
export const disable = (id: string) => {
  return put(prefix + "/disable/" + id);
}