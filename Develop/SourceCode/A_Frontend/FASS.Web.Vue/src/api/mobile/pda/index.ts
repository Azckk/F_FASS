import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 移动端

export const GetDictItemListToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/GetDictItemListToSelect"),
    {
      params
    }
  );
};

export const GetPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/GetPage"),
    {
      params
    }
  );
};

export const GetMaterialListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/GetMaterialListToSelect")
  );
};

export const AddWork = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Mobile/WarehouseWorkPdaCall/AddWork`),
    {
      params
    }
  );
};

// 物料登记接口
export const MaterialAddBand = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Warehouse/Material/MaterialAddBand?keyValue=${keyValue}`),
    {
      data
    }
  );
};

//缺料呼叫删除接口（单选或多选）
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/Delete"),
    {
      data
    }
  );
};

//删除全部缺料呼叫历史
export const DeleteAll = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/DeleteAll"),
    {
      params
    }
  );
};
//保留近一天缺料呼叫历史
export const DeleteD1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/DeleteD1"),
    {
      params
    }
  );
};
//保留近一个月缺料呼叫历史
export const DeleteM1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/DeleteM1"),
    {
      params
    }
  );
};
//保留近三个月缺料呼叫历史
export const DeleteM3 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/DeleteM3"),
    {
      params
    }
  );
};
//保留近一周缺料呼叫历史
export const DeleteW1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Mobile/WarehouseWorkPdaCall/DeleteW1"),
    {
      params
    }
  );
};
