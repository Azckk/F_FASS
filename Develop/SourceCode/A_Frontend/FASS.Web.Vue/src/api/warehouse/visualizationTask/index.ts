import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 库区列表
export const GetAreaList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/VisualizationTask/GetListToSelect"),
    {
      params
    }
  );
};

// 库位列表
export const StorageGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      `Warehouse/VisualizationTask/StorageGetPage?keyValue=${keyValue}`
    ),
    {
      params
    }
  );
};
// 库区列表 /api/v1/Warehouse/Storage/GetListToSelect
export const GetAreaListToSelect = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/VisualizationTask/GetListToSelect`),
    {
      params
    }
  );
};

// 添加任务
export const AddWork = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/VisualizationTask/AddWork"),
    {
      data
    }
  );
};

// 获取标签选择列表
export const GetListToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/VisualizationTask/GetListToSelect"),
    {
      params
    }
  );
};

// 获取车辆
export const GetCarPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Car/GetListToSelect"),
    {
      params
    }
  );
};
//获取当前容器
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

//获取当前物料
export const MaterialGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      `Warehouse/VisualizationTask/MaterialGetPage?keyValue=${keyValue}`
    ),
    {
      params
    }
  );
};

//获取除当前容器之外的所有容器
export const GetListToSelectContainer = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/VisualizationTask/GetListToSelectContainer"),
    {
      params
    }
  );
};
//获取除当前物料之外的所有物料
export const GetListToSelectMaterial = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/VisualizationTask/GetListToSelectMaterial"),
    {
      params
    }
  );
};

//新增或编辑容器
export const ContainerAddOrUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(
      `Warehouse/VisualizationTask/ContainerAddOrUpdate?keyValue=${keyValue}`
    ),
    {
      data
    }
  );
};

//库位绑定的容器变更
export const StorageAddContainer = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(
      `Warehouse/VisualizationTask/StorageAddContainer?keyValue=${keyValue}`
    ),
    {
      data
    }
  );
};
//删除库位当前容器
export const ContainerDelete = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(
      `Warehouse/VisualizationTask/ContainerDelete?keyValue=${keyValue}`
    ),
    {
      data
    }
  );
};

//新增物料
export const ContainerAddMaterial = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(
      `Warehouse/VisualizationTask/ContainerAddMaterial?keyValue=${keyValue}`
    ),
    {
      data
    }
  );
};

//删除库位当前容器物料
export const MaterialDelete = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(
      `Warehouse/VisualizationTask/MaterialDelete?keyValue=${keyValue}`
    ),
    {
      data
    }
  );
};

// 保存
export const SaveApi = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/VisualizationTask/Save"),
    {
      data
    }
  );
};

// 初始化位置
export const ResetAll = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/VisualizationTask/ResetAll"),
    {
      data
    }
  );
};

// 工作状态
export const GetWorkState = (data?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/VisualizationTask/GetWorkState"),
    {
      data
    }
  );
};

// 工作状态
export const SetWorkState = (keyValue?: boolean) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/VisualizationTask/SetWorkState?keyValue=${keyValue}`)
  );
};
