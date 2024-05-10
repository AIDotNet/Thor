import {  postJson } from "../uitls/fetch";

const prefix = "/api/v1/authorize";

export const login = (value:any) => {
  return postJson(prefix + "/token", value);
};
