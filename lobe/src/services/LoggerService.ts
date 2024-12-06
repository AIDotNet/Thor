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
    if (params.keyword) {
        url += "keyword=" + params.keyword;
    }
    return get(url);
}