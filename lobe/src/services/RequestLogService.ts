import { get } from "../utils/fetch"

const prefix = "/api/v1/request-log"
const systemPrefix = "/api/v1/system"

/**
 * 获取请求日志是否启用
 * @returns Promise
 */
export const isRequestLogEnabled = () => {
    return get(systemPrefix + '/request-log-enabled')
}

/**
 * 获取请求日志列表
 * @param page 页码
 * @param pageSize 页面大小
 * @param model 模型名称
 * @param userName 用户名
 * @param startTime 开始时间
 * @param endTime 结束时间
 * @returns Promise
 */
export const getRequestLogs = async (page: number = 1, pageSize: number = 20,
    model: string = '', userName: string = '', startTime: string = '', endTime: string = '') => {
    let url = `${prefix}?page=${page}&pageSize=${pageSize}`;

    if (model) {
        url += `&model=${encodeURIComponent(model)}`;
    }

    if (userName) {
        url += `&userName=${encodeURIComponent(userName)}`;
    }

    if (startTime) {
        url += `&startTime=${encodeURIComponent(startTime)}`;
    }

    if (endTime) {
        url += `&endTime=${encodeURIComponent(endTime)}`;
    }

    return get(url);
}

/**
 * 获取请求日志详情
 * @param id 日志ID
 * @returns Promise
 */
export const getRequestLogDetail = async (id: string) => {
    return get(`${prefix}/${id}`);
}