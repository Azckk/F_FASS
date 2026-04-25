import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect"),
  );
};
// "/Data/CarAction/GetListToSelectByCarTypeCode?carTypeCode=Car",
export const GetListToSelectByCarTypeCode = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("/Data/CarAction/GetListToSelectByCarTypeCode?carTypeCode=Car"),
  );
};

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarAction/GetPage"),
    {
      params
    }
  );
};

export const getNodeList  = (params?: object) => {
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
    baseUrlApi("Data/CarAction/addOrUpdate"),
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
    baseUrlApi("Data/CarAction/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Data/CarAction/Enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/CarAction/disable"),
    {
      data
    }
  );
};


