import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Zone/GetPage"),
    {
      params
    }
  );
};
/**
 * 下面方法还未调用
 */
export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("account/role/addOrUpdate"),
    {
      params,
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

// 获取站点页面
export const getSitePage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Zone/NodeGetPage?keyValue=" + keyValue),
    {
      params
    }
  );
};
// 获取站点信息
export const GetNodePosition = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetNodePosition?keyValue=" + keyValue),
    {
      params
    }
  );
};
