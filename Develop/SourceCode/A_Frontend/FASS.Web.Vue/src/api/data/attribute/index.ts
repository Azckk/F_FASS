import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";


// /api/v1/Data/Attribute/AddOrUpdate
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Attribute/GetPage"),
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
    baseUrlApi("Data/Attribute/addOrUpdate"),
    {
      params,
      data
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/Attribute/Delete"),
    {
      data
    }
  );
};

export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Attribute/Disable"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Attribute/Enable"),
    {
      data
    }
  );
};



