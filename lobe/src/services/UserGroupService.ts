import { del, get, postJson, put, putJson } from "../utils/fetch";

const prefix = '/api/v1/userGroup';

/**
 * 获取用户分组列表
 * @returns 用户分组列表
 */
export const getList = () => {
    return get(prefix);
}

/**
 * 创建用户分组
 * @param userGroup 用户分组信息
 * @returns 创建结果
 */
export const create = (userGroup: any) => {
    return postJson(prefix, userGroup);
}

/**
 * 更新用户分组
 * @param userGroup 用户分组信息
 * @returns 更新结果
 */
export const update = (userGroup: any) => {
    return putJson(prefix, userGroup);
}

/**
 * 删除用户分组
 * @param id 用户分组ID
 * @returns 删除结果
 */
export const remove = (id: string) => {
    return del(prefix + `/${id}`);
}

/**
 * 启用/禁用用户分组
 * @param id 用户分组ID
 * @param enable 是否启用
 * @returns 操作结果
 */
export const enableUserGroup = (id: string, enable: boolean) => {
    return put(prefix + `/enable/${id}?enable=${enable}`);
}


/**
 * 获取用户分组列表
 * @returns 用户分组列表
 */
export const getCurrentList = () => {
    return get(prefix + "/user");
}