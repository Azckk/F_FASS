import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

export const GetCarListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/ChargingStation/GetListToSelect")
  );
};
//安全光栅列表
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Object/SafetyLightGrids/GetPage"),
    {
      params
    }
  );
};

//安全光栅地址栏列表
export const getPageItem = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Object/SafetyLightGrids/ItemsGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};

export const ContainerGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      `Warehouse/VisualizationTask/ContainerGetPage?keyValue=${keyValue}`
    ),
    {
      params
    }
  );
};

//安全光栅添加和编辑接口
export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Object/SafetyLightGrids/AddOrUpdate"),
    {
      params,
      data
    }
  );
};

//安全光栅地址栏添加和编辑接口
export const addOrUpdateItem = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Object/SafetyLightGridsItem/AddOrUpdate`),
    {
      params,
      data
    }
  );
};

//安全光栅删除
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Object/SafetyLightGrids/Delete"),
    {
      data
    }
  );
};

//安全光栅地址位删除
export const deletesItem = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Object/SafetyLightGridsItem/Delete"),
    {
      data
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

// export const deletes = (data?: object) => {
//   return http.request<ReponseResult>(
//     "delete",
//     baseUrlApi("Data/ChargingStation/Delete"),
//     {
//       data
//     }
//   );
// };

// 字典项
export const GetListToSelect = (params: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Frame/DictItem/GetListToSelect?dictCode=" + params),
    {}
  );
};

export const getData = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Setting/ConfigCharge/GetData"),
    {
      params
    }
  );
};
