import { get } from "../uitls/fetch"

const prefix = "/api/v1/model"

/**
 * 获取类型列表
 * @returns Promise
 */
export const getTypes = () => {
    return get(prefix + '/types')
}

/**
 * 获取模型列表
 * @returns Promise
 */
export const getModels = () => {
    return get(prefix + '/models')
}