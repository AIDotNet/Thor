
import { get } from "../utils/fetch";

const prefix = "/api/v1/usage";

export const GetUsage = (token?: string, startDate?: string, endDate?: string) => {
  return get(prefix + "?token=" + token + "&startDate=" + startDate + "&endDate=" + endDate);
};

