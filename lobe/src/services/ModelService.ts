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

export const getProvider = () => {
    return get(prefix + '/provider')
}

export const getModelInfo = () => {
    return get(prefix + '/info' )
}

/**
 * 获取模型库列表
 * @param model 模型名称搜索
 * @param page 页码
 * @param pageSize 页面大小
 * @param type 供应商类型
 * @param modelType 模型类型
 * @param tags 标签数组
 * @returns Promise
 */
export const getModelLibrary = async (model: string = '', page: number = 1, pageSize: number = 20, 
    type: string = '', modelType: string = '', tags?: string[]) => {
    let url = prefix + `/library?page=${page}&pageSize=${pageSize}&model=${model}`;
    
    if (type) {
        url += `&type=${type}`;
    }
    
    if (modelType) {
        url += `&modelType=${modelType}`;
    }
    
    if (tags && tags.length > 0) {
        tags.forEach(tag => {
            url += `&tags=${encodeURIComponent(tag)}`;
        });
    }
    
    return get(url);
}

/**
 * 获取模型库元数据信息
 * @returns Promise
 */
export const getModelLibraryMetadata = () => {
    return get(prefix + '/library/metadata');
}
