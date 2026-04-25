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
    baseUrlApi("Data/Standby/GetPage"),
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
    baseUrlApi("Data/Standby/addOrUpdate"),
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
    baseUrlApi("Data/Standby/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Data/Standby/Enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Standby/disable"),
    {
      data
    }
  );
};

export const StandbyAdd = (keyValue: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Standby/CarAdd?keyValue=" + keyValue),
    {
      data
    }
  );
};

export const getStandbyCarPage = (keyValue: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Standby/CarGetPage?keyValue=" + keyValue),
    {
      params
    }
  );
};

export const StandbyCarDelete = (keyValue: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/Standby/CarDelete?keyValue=" + keyValue),
    {
      data
    }
  );
};
