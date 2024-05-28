import { get } from "../utils/fetch"


export const GetStatistics = () => {
    return get('/api/v1/statistics');
}