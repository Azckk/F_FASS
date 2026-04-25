import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/LogisticsRoute/GetListToSelect")
  );
};
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/LogisticsRoute/GetPage"),
    {
      params
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/LogisticsRoute/Delete"),
    {
      data
    }
  );
};
export const addOrUpdate = (params?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/LogisticsRoute/AddOrUpdate?keyValue=" + params),
    {
      data
    }
  );
};

// /api/v1/Warehouse/Area/GetPage
export const getAreaList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Area/GetPage"),
    {
      params
    }
  );
};

export const GetAreaListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Area/GetListToSelect")
  );
};
