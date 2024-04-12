import { del, get, post, postJson, put, putJson } from "../uitls/fetch";

const prefix = "/api/v1/redeemCode";

/**
 * 添加兑换码
 * @param data 兑换码数据
 * @returns Promise
 */
export const Add = (data: any) => {
    return postJson(prefix, data);
};

/**
 * 删除兑换码
 * @param id 兑换码ID
 * @returns Promise
 */
export const Remove = (id: number) => {
    return del(prefix + "/" + id);
};

/**
 * 更新兑换码
 * @param data 兑换码数据
 * @returns Promise
 */
export const Update = (data: any) => {
    return putJson(prefix, data);
};

/**
 * 获取兑换码列表
 * @param page 当前页码
 * @param pageSize 每页显示数量
 * @param keyword 关键词
 * @returns Promise
 */
export const getRedeemCodes = (page: number, pageSize: number, keyword: string) => {
    return get(prefix + "?page=" + page + "&pageSize=" + pageSize + "&keyword=" + keyword);
};

/**
 * 启用/禁用兑换码
 * @param id 兑换码ID
 * @returns Promise
 */
export const Enable = (id: number) => {
    return put(prefix + "/enable/" + id);
}

/**
 * 使用兑换码
 * @param code 兑换码
 * @returns Promise
 */
export const Use = (code: string) => {
    return post(prefix + "/use/" + code);
}