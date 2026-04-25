import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 储位容器列表

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Warehouse/StorageContainerHistory/GetPage"),
    {
      params
    }
  );
};
//删除全部储位容器历史
export const DeleteAll = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/StorageContainerHistory/DeleteAll"),
    {
      params
    }
  );
};
//保留近一天储位容器历史
export const DeleteD1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/StorageContainerHistory/DeleteD1"),
    {
      params
    }
  );
};
//保留近一个月储位容器历史
export const DeleteM1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/StorageContainerHistory/DeleteM1"),
    {
      params
    }
  );
};
//保留近三个月储位容器历史
export const DeleteM3 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/StorageContainerHistory/DeleteM3"),
    {
      params
    }
  );
};
//保留近一周储位容器历史
export const DeleteW1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Warehouse/StorageContainerHistory/DeleteW1"),
    {
      params
    }
  );
};
