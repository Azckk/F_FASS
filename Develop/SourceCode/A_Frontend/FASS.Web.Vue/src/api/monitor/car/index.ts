import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect"),
  );
};

// /api/v1/Monitor/Car/GetPage
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Car/GetPage"),
    {
      params
    }
  );
};

export const getNodeList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetListToSelect"),
    {
      params
    }
  );
};

export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/addOrUpdate"),
    {
      params,
      data
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/Car/Delete"),
    {
      data
    }
  );
};
/**
 * 下面方法还未调用
 */

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
// GetListToSelect
// 获取站点列表
export const GetListToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetListToSelect"),
    {
      params
    }
  );
};
// 获取充电点
export const getChargeList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Charge/GetListToSelect"),
    {
      params
    }
  );
};
