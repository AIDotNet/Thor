import { del, fetch, get, post, postJson, put, putJson } from "../utils/fetch";

const prefix = "/api/v1/channel";

export const getChannels = (page: number, pageSize: number,  keyword?: string,groups?:string[]) => {
  const params = new URLSearchParams();
  params.set("page", page.toString());
  params.set("pageSize", pageSize.toString());
  if (keyword) {
    params.set("keyword", keyword);
  }
  if (groups) {
    groups.forEach((group) => {
      params.append("groups", group);
    });
  }

  return fetch(prefix + "/list?" + params.toString(), {
    method: "GET"
  });
};

export const Remove = (id: string) => {
  return del(prefix + "/" + id);
};

export const Add = (data: any) => {
  return postJson(prefix, data);
};

export const Update = (id: string, data: any) => {
  return putJson(prefix + "/" + id, data);
};

export const getChannel = (id: string) => {
  return get(prefix + "/" + id);
};


export const disable = (id: string) => {
  return put(prefix + "/disable/" + id);
}

export const test = (id: string) => {
  return put(prefix + "/test/" + id);
}

export const controlAutomatically = (id: string) => {
  return put(prefix + "/control-automatically/" + id);
}

export const UpdateOrder = (id: string, order: number) => {
  return put(prefix + "/order/" + id + "?order=" + order);
}

export const getTags = () => {
  return get(prefix + "/tag");
}

/**
 * 导入渠道/api/v1/channel/import 文件
 */
export const importChannel = (file: File) => {
  const formData = new FormData();
  formData.append("file", file);
  return post(prefix + "/import", {
    body: formData,
  });
}


/**
 * 下载导入模板
 * /api/v1/channel/import/template
 */
export const downloadImportTemplate = () => {
  return get(prefix + "/import/template", {
    responseType: "blob",
  });
}





