import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect"),
  );
};


export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Container/GetPage"),
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

export const getWarehouseAreaList  = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/Area/GetListToSelect"),
    {
      params
    }
  );
};

export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Container/addOrUpdate"),
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
    baseUrlApi("Warehouse/Container/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Warehouse/Container/Enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Container/disable"),
    {
      data
    }
  );
};

export const SelectGetPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Container/SelectGetPage`),
    {
      params
    }
  );
};


export const StorageGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Container/StorageGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};

export const StorageAdd = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Warehouse/Container/StorageAdd?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const StorageDelete = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(`Warehouse/Container/StorageDelete?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const MaterialGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Container/MaterialGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};

export const MaterialAdd = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Warehouse/Container/MaterialAdd?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const MaterialDelete = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(`Warehouse/Container/MaterialDelete?keyValue=${keyValue}`),
    {
      data
    }
  );
};
