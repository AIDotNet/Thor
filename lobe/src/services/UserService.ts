import { del, get, post, postJson, putJson } from "../utils/fetch";

const prefix = '/api/v1/user';

/**
 * 获取用户列表
 * @param page 页码
 * @param pageSize 每页数量
 * @param keyword 关键词
 * @returns 用户列表
 */
export const getUsers = (page: number, pageSize: number, keyword: string) => {
    return get(prefix + `?page=${page}&pageSize=${pageSize}&keyword=${keyword}`);
}

/**
 * 删除用户
 * @param id 用户ID
 * @returns 删除结果
 */
export const Remove = (id: string) => {
    return del(prefix + `/${id}`);
}

/**
 * 创建用户
 * @param user 用户信息
 * @returns 创建结果
 */
export const create = (user: any) => {
    return postJson(prefix, user);
}

/**
 * 启用用户
 * @param id 用户ID
 * @returns 启用结果
 */
export const enable = (id: string) => {
    return post(prefix + `/enable/${id}`);
}

/**
 * 获取当前用户信息
 * @returns 当前用户信息
 */
export const info = () => {
    return get(prefix + `/info`);
}

/**
 * 更新用户信息
 * @param user 用户信息
 * @returns 更新结果
 */
export const update = (user: any) => {
    return putJson(prefix, user);
}

/**
 * 更新用户密码
 * @param data 密码信息
 */
export const updatePassword = (data:any) => {
    return putJson(prefix + '/update-password', data);
}

export const GetEmailCode = (email: string) => {
    return get(prefix + `/email-code?email=${email}`);
}