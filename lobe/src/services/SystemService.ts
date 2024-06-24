import { post, } from "../utils/fetch"

export const Share = (userId: string) => {
    return post('/api/v1/system/share?userId=' + userId)
}