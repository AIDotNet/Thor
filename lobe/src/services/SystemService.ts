import { get, } from "../utils/fetch"

export const InviteInfo = () => {
    return get('/api/v1/system/info')
}

/**
 * 获取请求日志是否启用
 * @returns Promise
 */
export const isRequestLogEnabled = () => {
    return get('/api/v1/system/request-log-enabled')
}