import { del, get,  postJson, put, putJson } from "../utils/fetch"

const prefix = '/api/v1/model-manager'

const CreateModelManager = async (params: any) => {
    return postJson(prefix, params);
}

const UpdateModelManager = async (params: any) => {
    return putJson(prefix, params);
}

const DeleteModelManager = async (id: string) => {
    return del(prefix + "/" + id);
}

const GetModelManagerList = async (model:string, page: number, pageSize: number, isPublic:boolean = false, type:string = '', modelType:string = '', tags?: string[]) => {
    let url = prefix + `?page=${page}&pageSize=${pageSize}&model=${model}&isPublic=${isPublic}`;
    
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

const EnableModelManager = async (id: string) => {
    return put(prefix + `/${id}/enable`);
}

/**
 * 获取模型统计信息
 */
const GetModelStats = async () => {
    return get(prefix + '/stats');
}

/**
 * 获取所有模型类型
 */
const GetModelTypes = async () => {
    return get(prefix + '/types');
}

/**
 * 获取所有标签
 */
const GetAllTags = async () => {
    return get(prefix + '/tags');
}

/**
 * 获取模型库元数据信息（包含tags、types、providers、icons等）
 */
const GetModelMetadata = async () => {
    return get(prefix + '/metadata');
}

export {
    CreateModelManager,
    UpdateModelManager,
    DeleteModelManager,
    GetModelManagerList,
    EnableModelManager,
    GetModelStats,
    GetModelTypes,
    GetAllTags,
    GetModelMetadata,
}