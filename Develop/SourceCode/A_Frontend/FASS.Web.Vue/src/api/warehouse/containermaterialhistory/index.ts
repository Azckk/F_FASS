import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 容器物料历史

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/ContainerMaterialHistory/GetPage"),
    {
      params
    }
  );
};

//删除全部容器物料历史
export const DeleteAll = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/ContainerMaterialHistory/DeleteAll"),
    {
      params
    }
  );
};
//保留近一天容器物料历史
export const DeleteD1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/ContainerMaterialHistory/DeleteD1"),
    {
      params
    }
  );
};
//保留近一个月容器物料历史
export const DeleteM1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/ContainerMaterialHistory/DeleteM1"),
    {
      params
    }
  );
};
//保留近三个月容器物料历史
export const DeleteM3 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/ContainerMaterialHistory/DeleteM3"),
    {
      params
    }
  );
};
//保留近一周容器物料历史
export const DeleteW1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/ContainerMaterialHistory/DeleteW1"),
    {
      params
    }
  );
};
