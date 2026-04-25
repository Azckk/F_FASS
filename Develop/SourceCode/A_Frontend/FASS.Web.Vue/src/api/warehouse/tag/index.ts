import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Tag/GetListToSelect"),
  );
};
// /api/v1 / Warehouse / Tag / AddOrUpdate
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Tag/GetPage"),
    {
      params
    }
  );
};
export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Tag/addOrUpdate"),
    {
      params,
      data
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Warehouse/Tag/Delete"),
    {
      data
    }
  );
};


