import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect")
  );
};

export const getPage = (params?: object) => {
  return http.request<ReponseResult>("get", baseUrlApi(`Data/Car/GetPage`), {
    params
  });
};

export const ZoneGetPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Data/Car/ZoneGetPage`),
    {
      params
    }
  );
};

export const ZoneAdd = (params?: object, data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Data/Car/ZoneAdd"), {
    params,
    data
  });
};

export const ZoneDelete = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/Car/ZoneDelete"),
    {
      params,
      data
    }
  );
};

export const getCarList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Car/GetListToSelect"),
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
    baseUrlApi("Data/Car/addOrUpdate"),
    {
      params,
      data
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>("delete", baseUrlApi("Data/Car/Delete"), {
    data
  });
};

export const getSimpleCars = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Car/GetSimpleCars")
  );
};

/**
 * 下面方法还未调用
 */

export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Data/Car/Enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Data/Car/Disable"), {
    data
  });
};
