import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

export const getPage = (params?: object) => {
  return http.request<ReponseResult>("get", baseUrlApi("Base/Edge/GetPage"), {
    params
  });
};

export const GetListToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Edge/GetListToSelect"),
    {
      params
    }
  );
};
// http://127.0.0.1:10201/Base/Edge/GetPage?pageParam=%7B%22where%22%3A%5B%5D%2C%22order%22%3A%
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
// 扩展
export const GetExtends = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Edge/GetExtends?keyValue=" + keyValue),
    {
      params
    }
  );
};
// 路线轨迹
export const GetTrajectory = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Edge/GetTrajectory?keyValue=" + keyValue),
    {
      params
    }
  );
};

// 动作
export const GetActions = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Edge/GetActions?keyValue=" + keyValue),
    {
      params
    }
  );
};
