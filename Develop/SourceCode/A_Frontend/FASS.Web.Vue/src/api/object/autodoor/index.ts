import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// /api/v1/Object/AutoDoor/AddOrUpdate
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Object/AutoDoor/GetPage"),
    {
      params
    }
  );
};
// /api/v1/Object/AutoDoor/AddOrUpdate
export const addOrUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Object/AutoDoor/AddOrUpdate?keyValue=" + keyValue),
    {
      data
    }
  );
};

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Object/AutoDoor/Delete"),
    {
      data
    }
  );
};
