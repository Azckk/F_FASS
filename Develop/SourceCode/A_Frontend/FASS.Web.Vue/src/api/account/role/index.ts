import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("account/role/getPage"),
    {
      params
    }
  );
};
export const addOrUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`account/role/addOrUpdate?keyValue=${keyValue}`),
    {
      data
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("account/role/delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("account/role/enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("account/role/disable"),
    {
      data
    }
  );
};
export const userGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`account/role/userGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};
export const userAdd = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`account/role/userAdd?keyValue=${keyValue}`),
    {
      data
    }
  );
};
export const userDeletes = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(`account/role/userDelete?keyValue=${keyValue}`),
    {
      data
    }
  );
};
export const permissionGetTree = (keyValue?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`account/role/permissionGetTree?keyValue=${keyValue}`)
  );
};
export const permissionUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`account/role/permissionUpdate?keyValue=${keyValue}`),
    {
      data
    }
  );
};
