import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 获取站点
export const GetNodeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetListToSelect")
  );
};
// 获取模版
export const GetTemplateListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplateMDCS/GetListToSelect")
  );
};
// /api/v1/Flow/TaskRecord/AddOrUpdate
export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskRecord/AddOrUpdate"),
    {
      params,
      data
    }
  );
};
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskRecord/GetPage"),
    {
      params
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskRecord/Delete"),
    {
      data
    }
  );
};
// forceDelete
export const ForceDelete = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskRecord/ForceDelete"),
    {
      data
    }
  );
};

export const Cancel = (data?: object) => {
  // 取消
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskRecord/Cancel"),
    {
      data
    }
  );
};

export const DeleteM3 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskRecord/DeleteM3"),
    {
      data
    }
  );
};

export const DeleteM1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskRecord/DeleteM1"),
    {
      data
    }
  );
};

export const DeleteW1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskRecord/DeleteW1"),
    {
      data
    }
  );
};

export const DeleteD1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskRecord/DeleteD1"),
    {
      data
    }
  );
};

export const DeleteAll = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskRecord/DeleteAll"),
    {
      data
    }
  );
};

export const Release = (data?: object) => {
  // 发布
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskRecord/Release"),
    {
      data
    }
  );
};

export const Pause = (data?: object) => {
  // 暂停
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskRecord/Pause"),
    {
      data
    }
  );
};

export const Resume = (data?: object) => {
  // 继续/恢复
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskRecord/Resume"),
    {
      data
    }
  );
};
export const Resend = (data?: object) => {
  // 重发
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskRecord/Resend"),
    {
      data
    }
  );
};

// 车辆类型选择
export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect")
  );
};
// 模板选择
export const TaskTemplateListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplate/GetListToSelect")
  );
};

// 模板获取车辆列表
export const GetListToSelectByTaskTemplateId = value => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      "Data/Car/GetListToSelectByTaskTemplateId?taskTemplateId=" + value
    )
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

// 任务实例 子任务
export const getTaskInstancePage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskInstanceProcess/GetPage?taskInstanceId=" + keyValue),
    {
      params
    }
  );
};
// http://localhost:10201/Flow/TaskInstance/GetPage?pageParam=%7B%22where%22%3A%5B%7B%22logic%22%3A%22And%22%2C%22field%22%3A%22create

export const addOrUpdateTaskInstanceProcess = (
  keyValue?: string,
  data?: object
) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskInstanceProcess/AddOrUpdate?keyValue=" + keyValue),
    {
      data
    }
  );
};

export const TaskInstanceProcessDelete = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstanceProcess/Delete"),
    {
      data
    }
  );
};
// 任务实例-动作
export const getTaskInstanceActionPage = (
  keyValue?: string,
  params?: object
) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      "Flow/TaskInstanceAction/GetPage?taskInstanceProcessId=" + keyValue
    ),
    {
      params
    }
  );
};

export const GetListToSelectByCarTypeCode = (carTypeCode?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      "Data/CarAction/GetListToSelectByCarTypeCode?carTypeCode=" + carTypeCode
    )
  );
};

export const addOrUpdateTaskInstanceAction = (
  keyValue?: string,
  data?: object
) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskInstanceAction/AddOrUpdate?keyValue=" + keyValue),
    {
      data
    }
  );
};

export const TaskInstanceActionDelete = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstanceAction/Delete"),
    {
      data
    }
  );
};
// 任务实例-动作-参数
export const getTaskInstanceParameterPage = (
  keyValue?: string,
  params?: object
) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      "Flow/TaskInstanceParameter/GetPage?taskInstanceActionId=" + keyValue
    ),
    {
      params
    }
  );
};

export const addOrUpdateTaskInstanceParameter = (
  keyValue?: string,
  data?: object
) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskInstanceParameter/AddOrUpdate?keyValue=" + keyValue),
    {
      data
    }
  );
};

export const TaskInstanceParameterDelete = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstanceParameter/Delete"),
    {
      data
    }
  );
};
