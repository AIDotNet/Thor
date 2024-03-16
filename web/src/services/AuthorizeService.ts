import { fetch } from "../uitls/fetch";

const prefix = "/api/v1/authorize";

export const login = (account: string, password: string) => {
  return fetch(prefix + "/token", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    query: {
      account,
      password,
    },
  });
};
