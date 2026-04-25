import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const getPage = (dictId?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`frame/dictItem/getPage?dictId=${dictId}`),
    {
      params
    }
  );
};
export const addOrUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`frame/dictItem/addOrUpdate?keyValue=${keyValue}`),
    {
      data
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("frame/dictItem/delete"),
    {
      data
    }
  );
};
export const getListToSelect = (dictCode?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`frame/dictItem/getListToSelect?dictCode=${dictCode}`)
  );
};
