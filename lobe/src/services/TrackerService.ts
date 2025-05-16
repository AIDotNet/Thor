
import { get } from "../utils/fetch";

const prefix = "/api/v1/tracker";

/**
 * 获取服务器负载
 */
export const GetServerLoad = (loggerId: string) => {
  return get(prefix + "/" + loggerId);
};

/**
 * GetUserRequest
 */
export const GetUserRequest = () => {
  return get(prefix + "/request-user");
};

