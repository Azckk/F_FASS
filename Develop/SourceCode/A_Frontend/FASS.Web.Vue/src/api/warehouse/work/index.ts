import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect")
  );
};

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Work/GetPage"),
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
    baseUrlApi("Warehouse/Work/addOrUpdate"),
    {
      params,
      data
    }
  );
};
// keyValue

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Warehouse/Work/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Work/Enable"),
    {
      data
    }
  );
};
export const getContainer = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Container/GetPage"),
    {
      params
    }
  );
};
export const GetListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Container/GetListToSelect")
  );
};
