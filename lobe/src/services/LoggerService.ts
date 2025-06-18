import { get } from "../utils/fetch"

const prefix = '/api/v1/logger'
export const getLoggers = async (params: any) => {
    let url = prefix + "?";
    if (params.page) {
        url += "page=" + params.page + "&";
    }
    if (params.pageSize) {
        url += "pageSize=" + params.pageSize + "&";
    }
    if (params.type) {
        url += "type=" + params.type + "&";
    }
    if (params.model) {
        url += "model=" + params.model + "&";
    }
    if (params.startTime) {
        url += "startTime=" + params.startTime + "&";
    }
    if (params.endTime) {
        url += "endTime=" + params.endTime + "&";
    }
    if (params.organizationId) {
        url += "organizationId=" + params.organizationId + "&";
    }
    if (params.userId) {
        url += "userId=" + params.userId + "&";
    }
    if (params.keyword) {
        url += "keyword=" + params.keyword;
    }
    return get(url);
}

export const viewConsumption = async (params: any) => {

    let url = prefix + "/view-consumption?";
    if (params.type) {
        url += "type=" + params.type + "&";
    }
    if (params.model) {
        url += "model=" + params.model + "&";
    }
    if (params.startTime) {
        url += "startTime=" + params.startTime + "&";
    }
    if (params.endTime) {
        url += "endTime=" + params.endTime + "&";
    }
    if (params.userId) {
        url += "userId=" + params.userId + "&";
    }
    if (params.keyword) {
        url += "keyword=" + params.keyword;
    }
    return get(url);
}


/**
 * 下载导入模板
 */
export const Export = (params: any) => {
    
    let url = prefix + "/export?";
    if (params.type) {
        url += "type=" + params.type + "&";
    }
    if (params.model) {
        url += "model=" + params.model + "&";
    }
    if (params.startTime) {
        url += "startTime=" + params.startTime + "&";
    }
    if (params.endTime) {
        url += "endTime=" + params.endTime + "&";
    }
    if (params.userId) {
        url += "userId=" + params.userId + "&";
    }
    if (params.keyword) {
        url += "keyword=" + params.keyword;
    }
    return get(url, {
      responseType: "blob",
    });
  }
  
  
  
/**
 * 获取模型热度
 * @returns 
 */
export const modelHot = async () => {
    return get(prefix + "/model-hot");
}

/**
 * 获取日志详情的调用链路
 * @param loggerId 日志ID
 * @returns 
 */
export const getLoggerDetail = async (loggerId: string) => {
    return get(prefix + "/detail/" + loggerId);
}