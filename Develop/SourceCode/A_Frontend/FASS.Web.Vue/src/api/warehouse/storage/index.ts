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
    baseUrlApi("Warehouse/Storage/GetPage"),
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

export const getWarehouseAreaList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Area/GetListToSelect"),
    {
      params
    }
  );
};

export const GetListByAreaToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Storage/GetListByAreaToSelect"),
    {
      params
    }
  );
};

export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Storage/addOrUpdate"),
    {
      params,
      data
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Warehouse/Storage/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Storage/Enable"),
    {
      data
    }
  );
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Storage/disable"),
    {
      data
    }
  );
};
export const lock = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Storage/Lock"),
    {
      data
    }
  );
};
export const unlock = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Storage/UnLock"),
    {
      data
    }
  );
};

export const ContainerGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Storage/ContainerGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};

export const MaterialGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Storage/MaterialGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};

export const ContainerDelete = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(`Warehouse/Storage/ContainerDelete?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const ContainerAdd = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Warehouse/Storage/ContainerAdd?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const MaterialAdd = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Warehouse/Storage/MaterialAdd?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const MaterialDelete = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(`Warehouse/Storage/MaterialDelete?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const GetStorageByNode = (data?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Storage/GetStorageByNode?code=" + data)
  );
};