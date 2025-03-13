import { get, } from "../utils/fetch"

export const InviteInfo = () => {
    return get('/api/v1/system/info')
}