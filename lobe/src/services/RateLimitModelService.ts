import { del, get, postJson, put, putJson } from "../utils/fetch"

export const getRateLimitModel = (page:number,pageSize:number) => {
    return get('/api/v1/rateLimitModel?page='+page+'&pageSize='+pageSize)
}

export const createRateLimitModel = (data: any) => {
    return postJson('/api/v1/rateLimitModel', data)
}

export const putRateLimitModel = (data: any) => {
    return putJson('/api/v1/rateLimitModel', data)
}

export const removeRateLimitModel = (id: string) => {
    return del(`/api/v1/rateLimitModel/${id}`)
}

export const disableRateLimitModel = (id: string) => {
    return put(`/api/v1/rateLimitModel/disable/${id}`)
}