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
  return http.request<ReponseResult>("get", baseUrlApi("Data/Charge/GetPage"), {
    params
  });
};

export const GetListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Charge/GetListToSelect")
  );
};
// http://localhost:10101/api/v1/Data/Charge/GetListToSelect

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
    baseUrlApi("Data/Charge/addOrUpdate"),
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
    baseUrlApi("Data/Charge/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Data/Charge/Enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Data/Charge/disable"), {
    data
  });
};

export const getChargeCarPage = (keyValue: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Charge/CarGetPage?keyValue=" + keyValue),
    {
      params
    }
  );
};

export const ChargeCarDelete = (keyValue: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/Charge/CarDelete?keyValue=" + keyValue),
    {
      data
    }
  );
};

export const ChargeAdd = (keyValue: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Charge/CarAdd?keyValue=" + keyValue),
    {
      data
    }
  );
};
