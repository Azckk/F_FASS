import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 预定物料信息

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/PreMaterial/GetPage"),
    {
      params
    }
  );
};

export const getEntity = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/PreMaterial/GetEntity"),
    {
      params
    }
  );
};

export const deleteApi = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Mobile/PreMaterial/Delete"),
    {
      data
    }
  );
};
