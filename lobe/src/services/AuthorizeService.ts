import {  post, postJson } from "../utils/fetch";

const prefix = "/api/v1/authorize";

export const login = (value:any) => {
  return postJson(prefix + "/token", value);
};

export const getGithubToken = (code: string) => {
  return post(prefix + "/github?code=" + code);
}