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
    baseUrlApi("Warehouse/Area/GetPage"),
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

export const GetListByTypeToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Area/GetListByTypeToSelect"),
    {
      params
    }
  );
};

export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Area/addOrUpdate"),
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
    baseUrlApi("Warehouse/Area/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Area/Enable"),
    {
      data
    }
  );
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Area/disable"),
    {
      data
    }
  );
};
