import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 物料储位历史

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/MaterialStorageHistory/GetPage"),
    {
      params
    }
  );
};

//删除全部物料储位历史
export const DeleteAll = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/MaterialStorageHistory/DeleteAll"),
    {
      params
    }
  );
};
//保留近一天物料储位历史
export const DeleteD1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/MaterialStorageHistory/DeleteD1"),
    {
      params
    }
  );
};
//保留近一个月物料储位历史
export const DeleteM1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/MaterialStorageHistory/DeleteM1"),
    {
      params
    }
  );
};
//保留近三个月物料储位历史
export const DeleteM3 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/MaterialStorageHistory/DeleteM3"),
    {
      params
    }
  );
};
//保留近一周物料储位历史
export const DeleteW1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/MaterialStorageHistory/DeleteW1"),
    {
      params
    }
  );
};
