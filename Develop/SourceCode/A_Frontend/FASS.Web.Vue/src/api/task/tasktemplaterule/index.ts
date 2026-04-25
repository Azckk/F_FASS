import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// 规则
// /api/v1/Flow/TaskTemplateRule/GetPage
export const getPage = (taskTemplateId: string, params?: object,) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplateRule/GetPage?taskTemplateId=" + taskTemplateId),
    {
      params
    }
  );
};
// /api/v1 / Flow / TaskTemplateRule / AddOrUpdate
export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskTemplateRule/AddOrUpdate"),
    {
      params,
      data
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskTemplateRule/Delete"),
    {
      data
    }
  );
};
export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect"),
  );
};