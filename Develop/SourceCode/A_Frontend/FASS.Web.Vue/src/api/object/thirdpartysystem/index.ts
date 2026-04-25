import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// /api/v1/Object/ThirdpartySystem/GetPage
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Object/ThirdpartySystem/GetPage"),
    {
      params
    }
  );
};
// /api/v1/Object/ThirdpartySystem/AddOrUpdate
export const addOrUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Object/ThirdpartySystem/AddOrUpdate?keyValue=" + keyValue),
    {
      data
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Object/ThirdpartySystem/Delete"),
    {
      data
    }
  );
};
