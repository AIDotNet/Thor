import { del, fetch, get, postJson, putJson } from "../uitls/fetch";

const prefix = "/api/v1/channel";

export const getChannels = (page: number, pageSize: number) => {
  return fetch(prefix + "/list", {
    method: "GET",
    query: {
      page,
      pageSize,
    },
  });
};

export const Remove = (id: string) => {
  return del(prefix + "/" + id);
};

export const Add = (data: any) => {
  return postJson(prefix, data);
};

export const Update = (data: any) => {
  return putJson(prefix, data);
};

export const getChannel = (id: string) => {
  return get(prefix + "/" + id);
};
