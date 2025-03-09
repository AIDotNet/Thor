import { get, postJson, putJson, del } from "../utils/fetch";

const prefix = '/api/v1/modelmap';

/**
 * 模型映射实体类型定义
 */
export interface ModelMapItem {
  modelId: string;
  order: number;
}

export interface ModelMap {
  id?: string;
  updatedAt?: string;
  modifier?: string;
  createdAt?: string;
  creator?: string;
  modelId: string;
  modelMapItems: ModelMapItem[];
  group: string[];
}

/**
 * 获取模型映射列表
 * @returns 
 */
export const getModelMapList = async () => {
    return get(prefix);
}

/**
 * 创建模型映射
 * @param data 模型映射数据
 * @returns 
 */
export const createModelMap = async (data: ModelMap) => {
    return postJson(prefix, data);
}

/**
 * 更新模型映射
 * @param data 模型映射数据
 * @returns 
 */
export const updateModelMap = async (data: ModelMap) => {
    return putJson(prefix, data);
}

/**
 * 删除模型映射
 * @param id 模型映射ID
 * @returns 
 */
export const deleteModelMap = async (id: string) => {
    return del(prefix + `/${id}`);
}

