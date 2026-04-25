import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

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
export const GetListToSelectByTaskTemplateId = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Car/GetListToSelect")
    // baseUrlApi(
    //   "Data/Car/GetListToSelectByTaskTemplateId?taskTemplateId" + value
    // )
  );
};

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskInstance/GetPage"),
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
    baseUrlApi("Flow/TaskInstance/AddOrUpdate"),
    {
      params,
      data
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstance/Delete"),
    {
      data
    }
  );
};

export const ForceDelete = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstance/ForceDelete"),
    {
      data
    }
  );
};

export const Release = (data?: object) => {
  // 发布
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskInstance/Release"),
    {
      data
    }
  );
};

export const Pause = (data?: object) => {
  // 暂停
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskInstance/Pause"),
    {
      data
    }
  );
};

export const Resume = (data?: object) => {
  // 暂停
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskInstance/Resume"),
    {
      data
    }
  );
};

export const Cancel = (data?: object) => {
  // 暂停
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskInstance/Cancel"),
    {
      data
    }
  );
};

export const DeleteM3 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstance/DeleteM3"),
    {
      data
    }
  );
};

export const DeleteM1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstance/DeleteM1"),
    {
      data
    }
  );
};

export const DeleteW1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstance/DeleteW1"),
    {
      data
    }
  );
};

export const DeleteD1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstance/DeleteD1"),
    {
      data
    }
  );
};

export const DeleteAll = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskInstance/DeleteAll"),
    {
      data
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
