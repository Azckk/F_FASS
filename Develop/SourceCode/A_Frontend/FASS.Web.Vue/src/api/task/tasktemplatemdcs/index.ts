import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect"),
  );
};
// /api/v1 / Flow / TaskTemplateRule / AddOrUpdate
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplateMDCS/GetPage"),
    {
      params
    }
  );
};
// /api/v1/Flow/TaskTemplateMDCS/GetListToSelect
export const GetListToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplateMDCS/GetListToSelect"),
    {
      params
    }
  );
};
///api/v1/Flow/TaskTemplateMDCS/GetPage
export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskTemplateMDCS/AddOrUpdate"),
    {
      params,
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Flow/TaskTemplateMDCS/enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskTemplateMDCS/disable"),
    {
      data
    }
  );
};

// /api/v1/Flow/TaskTemplateMDCS/Delete
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskTemplateMDCS/Delete"),
    {
      data
    }
  );
};
