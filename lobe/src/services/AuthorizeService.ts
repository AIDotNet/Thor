import {  post, postJson } from "../utils/fetch";

const prefix = "/api/v1/authorize";

export const login = (value:any) => {
  return postJson(prefix + "/token", value);
};

export const getGithubToken = (code: string) => {
  return post(prefix + "/github?code=" + code);
}

export const getGiteeToken = (code:string,redirectUri:string) =>{
  return post(prefix+"/gitee?code="+code+"&redirectUri="+redirectUri);
}

export const getCasdoorToken = (code:string) =>{
  return post(prefix+"/casdoor?code="+code);
}