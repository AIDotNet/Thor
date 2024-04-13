import { get } from "../uitls/fetch"


export const GetStatistics = () => {
    return get('/api/v1/statistics');
}