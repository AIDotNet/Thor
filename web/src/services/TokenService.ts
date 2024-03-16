import { del, get, postJson } from "../uitls/fetch";

const prefix = "/api/v1/token";

export const Add = (data: any) => {
  return postJson(prefix, data);
};

export const Remove = (id: string) => {
  return del(prefix + "/" + id);
};

export const Update = (data: any) => {
  return postJson(prefix, data);
};

export const getTokens = (page: number, pageSize: number) => {
  return get(prefix + "/list?page=" + page + "&pageSize=" + pageSize);
};

export const getToken = (id: string) => {
  return get(prefix + "/" + id);
};
