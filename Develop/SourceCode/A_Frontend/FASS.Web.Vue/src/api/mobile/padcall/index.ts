import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

export const GetDictItemListToSelect = (params: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/PadCall/GetDictItemListToSelect"),
    {
      params
    }
  );
};
export const GetPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/PadCall/GetPage"),
    {
      params
    }
  );
};

export const AddWork = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Mobile/PadCall/AddWork`),
    {
      params,
      data
    }
  );
};

export const AddWork2 = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Mobile/PadCall/AddWork2`),
    {
      data
    }
  );
};

export const AddWork3 = (storageId?: string, type?: string) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Mobile/PadCall/AddWork3?storageId=${storageId}&type=${type}`)
  );
};
