import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetCarListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Car/GetListToSelect")
  );
};

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarTraffic/GetPage"),
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
    baseUrlApi("Data/CarTraffic/addOrUpdate"),
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
    baseUrlApi("Data/CarTraffic/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/CarTraffic/FinishEnable"),
    {
      data
    }
  );
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/CarTraffic/FinishDisable"),
    {
      data
    }
  );
};
