import { get } from "../uitls/fetch"

const prefix = "/api/v1/model"

export const getTypes = () => {
    return get(prefix + '/types')
}

export const getModels = () => {
    return get(prefix + '/models')
}