import { del, get, post, postJson, putJson } from "../uitls/fetch"

export const getProduct = () => {
    return get('/api/v1/product')
}

export const createProduct = (data: any) => {
    return postJson('/api/v1/product', data)
}

export const putProduct = (data: any) => {
    return putJson('/api/v1/product', data)
}

export const removeProduct = (id: string) => {
    return del(`/api/v1/product/${id}`)
}

export const startPayload = (id: string) => {
    return post(`/api/v1/product/start-pay-payload/${id}`)
}