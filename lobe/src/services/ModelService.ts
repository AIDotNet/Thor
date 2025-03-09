import { get } from "../utils/fetch"

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

export const getUseModel = () => {
    return get(prefix + '/use-models')
}

/**
 * 获取模型列表
 * @returns 模型列表
 */
export const getModelList = async () => {
  return get('/api/v1/model/models');
}