import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 库区列表
export const GetAreaList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Visualization/GetListToSelect"),
    {
      params
    }
  );
};

// 库位列表
export const StorageGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Visualization/StorageGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};
// 库区列表 /api/v1/Warehouse/Storage/GetListToSelect
export const GetAreaListToSelect = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Storage/GetListToSelect`),
    {
      params
    }
  );
};

// 添加任务
export const AddTaskRecord = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Visualization/AddTaskRecord"),
    {
      data
    }
  );
};

// 获取标签列表 /api/v1/Warehouse/Tag/GetPage
export const GetTagPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Tag/GetPage"),
    {
      params
    }
  );
};
// 获取标签选择列表
export const GetListToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Tag/GetListToSelect"),
    {
      params
    }
  );
};
// 添加tag /api/v1/Warehouse/Visualization/TagAdd  ///api/v1/Warehouse/Tag/AddOrUpdate
export const AddTag = (params?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Visualization/TagUpdate?keyValue=" + params),
    {
      data
    }
  );
};
// 移除tag /api/v1/Warehouse/Visualization/TagDelete
export const TagDelete = (params?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Warehouse/Visualization/TagDelete?keyValue=" + params),
    {
      data
    }
  );
};

// 任务模版
export const TaskTemplateMDCS = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplateMDCS/GetListToSelect"),
    {
      params
    }
  );
};
// 动线模版
export const LogisticsRoute = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/LogisticsRoute/GetPage"),
    {
      params
    }
  );
};
// 获取站点
export const GetNodePage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetListToSelect"),
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
      // params
    }
  );
};
