import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 站点管理
// 获取页面数据
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Record/PlcRawData/GetPage"),
    {
      params
    }
  );
};
// /api/v1/Record/PlcRawData/DeleteAll
export const DeleteAll = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/PlcRawData/DeleteAll"),
    {
      params
    }
  );
};
//   /api/v1/Record/PlcRawData/DeleteD1
export const DeleteD1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/PlcRawData/DeleteD1"),
    {
      params
    }
  );
};
//   /api/v1/Record/PlcRawData/DeleteM1
export const DeleteM1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/PlcRawData/DeleteM1"),
    {
      params
    }
  );
};
//   /api/v1/Record/PlcRawData/DeleteM3
export const DeleteM3 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/PlcRawData/DeleteM3"),
    {
      params
    }
  );
};
export const DeleteW1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/PlcRawData/DeleteW1"),
    {
      params
    }
  );
};
