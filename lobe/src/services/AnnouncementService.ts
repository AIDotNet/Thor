import { del, get, post, postJson, putJson } from "../utils/fetch";

const prefix = '/api/v1/announcement';
const adminPrefix = '/api/v1/announcement/admin';

/**
 * 获取有效公告（用户端）
 * @returns 有效公告列表
 */
export const getActiveAnnouncements = () => {
    return get(prefix + '/active');
}

/**
 * 获取公告列表（管理员）
 * @param page 页码
 * @param pageSize 每页数量
 * @param keyword 关键词
 * @returns 公告列表
 */
export const getAnnouncements = (page: number, pageSize: number, keyword?: string) => {
    const params = new URLSearchParams();
    params.append('page', page.toString());
    params.append('pageSize', pageSize.toString());
    if (keyword) {
        params.append('keyword', keyword);
    }
    return get(adminPrefix + `/list?${params.toString()}`);
}

/**
 * 获取公告详情
 * @param id 公告ID
 * @returns 公告详情
 */
export const getAnnouncement = (id: string) => {
    return get(adminPrefix + `/${id}`);
}

/**
 * 创建公告
 * @param announcement 公告信息
 * @returns 创建结果
 */
export const createAnnouncement = (announcement: any) => {
    return postJson(adminPrefix, announcement);
}

/**
 * 更新公告
 * @param id 公告ID
 * @param announcement 公告信息
 * @returns 更新结果
 */
export const updateAnnouncement = (id: string, announcement: any) => {
    return putJson(adminPrefix + `/${id}`, announcement);
}

/**
 * 删除公告
 * @param id 公告ID
 * @returns 删除结果
 */
export const deleteAnnouncement = (id: string) => {
    return del(adminPrefix + `/${id}`);
}

/**
 * 启用/禁用公告
 * @param id 公告ID
 * @param enabled 是否启用
 * @returns 操作结果
 */
export const toggleAnnouncement = (id: string, enabled: boolean) => {
    return putJson(adminPrefix + `/toggle/${id}?enabled=${enabled}`, {});
} 