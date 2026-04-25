import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("frame/config/getPage"),
    {
      params
    }
  );
};
export const addOrUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`frame/config/addOrUpdate?keyValue=${keyValue}`),
    {
      data
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("frame/config/delete"),
    {
      data
    }
  );
};
