import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetCarListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/ChargingStation/GetListToSelect"),
  );
};


export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/ChargingStation/GetPage"),
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
    baseUrlApi("Data/ChargingStation/addOrUpdate"),
    {
      params,
      data
    }
  );
};


export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/ChargingStation/Delete"),
    {
      data
    }
  );
};
// /api/v1/Frame/DictItem/GetListToSelect
// 字典项
export const GetListToSelect = (params: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Frame/DictItem/GetListToSelect?dictCode=" + params),
    {
    }
  );
};

// 充电策略  /api/v1/Setting/ConfigCharge/SetData
export const setData = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Setting/ConfigCharge/SetData"),
    {
      data
    }
  );
};
export const getData = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Setting/ConfigCharge/GetData"),
    {
      params
    }
  );
};
