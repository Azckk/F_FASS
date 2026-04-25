import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// 站点管理
// 获取页面数据
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetPage"),
    {
      params
    }
  );
};
// 扩展
export const GetExtends = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetExtends?keyValue=" + keyValue),
    {
      params
    }
  );
};
// 站点位置
export const GetNodePosition = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetNodePosition"),
    {
      params
    }
  );
};
// 获取区域站点页面
export const getSitePage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Zone/NodeGetPage?keyValue=" + keyValue),
    {
      params
    }
  );
};

// 动作
export const GetActions = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetActions?keyValue=" + keyValue),
    {
      params
    }
  );
};

export const GetListToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetListToSelect"),
    {
      params
    }
  );
};
// 车辆类型
export const GetListToSelectByCarTypeCode = (carTypeCode?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      "Data/CarAction/GetListToSelectByCarTypeCode?carTypeCode=" + carTypeCode
    ),
    {
      params
    }
  );
};
