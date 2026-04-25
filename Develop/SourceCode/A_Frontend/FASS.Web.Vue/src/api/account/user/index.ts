import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("account/user/getPage"),
    {
      params
    }
  );
};
export const addOrUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`account/user/addOrUpdate?keyValue=${keyValue}`),
    {
      data
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("account/user/delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("account/user/enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("account/user/disable"),
    {
      data
    }
  );
};
export const resetPassword = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("account/user/resetPassword"),
    {
      data
    }
  );
};
