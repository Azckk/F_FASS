import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/PlanRules/GetListToSelect"),
  );
};
///api/v1/Data/PlanRules/AddOrUpdate
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/PlanRules/GetPage"),
    {
      params
    }
  );
};
export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/PlanRules/addOrUpdate"),
    {
      params,
      data
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/PlanRules/Delete"),
    {
      data
    }
  );
};

