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
    baseUrlApi("Warehouse/Material/GetPage"),
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
    baseUrlApi("Warehouse/Material/addOrUpdate"),
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
    baseUrlApi("Warehouse/Material/Delete"),
    {
      data
    }
  );
};
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Warehouse/Material/Enable"), {
    data
  });
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/Material/disable"),
    {
      data
    }
  );
};

export const SelectGetPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Material/SelectGetPage`),
    {
      params
    }
  );
};

export const StorageAdd = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Warehouse/Material/StorageAdd?keyValue=${keyValue}`),
    {
      data
    }
  );
};


export const  StorageDelete = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(`Warehouse/Material/StorageDelete?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const  ContainerDelete = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi(`Warehouse/Material/ContainerDelete?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const ContainerAdd = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Warehouse/Material/ContainerAdd?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const StorageGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Material/StorageGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};

export const ContainerGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Warehouse/Material/ContainerGetPage?keyValue=${keyValue}`),
    {
      params
    }
  );
};
