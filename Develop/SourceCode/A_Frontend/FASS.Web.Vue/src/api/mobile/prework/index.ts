import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 物料预定记录 prework

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/PreWork/GetPage"),
    {
      params
    }
  );
};
export const getEntity = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/PreWork/GetEntity"),
    {
      params
    }
  );
};

export const deleteApi = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Mobile/PreWork/Delete"),
    {
      data
    }
  );
};
