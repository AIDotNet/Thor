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

const GetModelManagerList = async (model:string,page: number, pageSize: number,isPublic:boolean = false,type:string = '') => {
    return get(prefix + `?page=${page}&pageSize=${pageSize}&model=${model}&isPublic=${isPublic}&type=${type}`);
}

const EnableModelManager = async (id: string) => {
    return put(prefix + `/${id}/enable`);
}

export {
    CreateModelManager,
    UpdateModelManager,
    DeleteModelManager,
    GetModelManagerList,
    EnableModelManager
}