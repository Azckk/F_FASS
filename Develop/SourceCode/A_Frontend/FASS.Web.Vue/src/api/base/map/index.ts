import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

export const getPage = (params?: object) => {
  return http.request<ReponseResult>("get", baseUrlApi("Base/Map/GetPage"), {
    params
  });
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

//保存simple地图
export const saveSimpleMap = (keyValue?: string) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Base/Map/SaveSimpleProject?map=${keyValue}`)
  );
};
