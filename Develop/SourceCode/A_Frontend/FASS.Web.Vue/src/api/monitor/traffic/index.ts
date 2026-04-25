import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// /api/v1/Record/Traffic/GetPage
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Record/Traffic/GetPage"),
    {
      params
    }
  );
};

export const DeleteAll = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/Traffic/DeleteAll"),
    {
      params
    }
  );
};
export const DeleteD1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/Traffic/DeleteD1"),
    {
      params
    }
  );
};
export const DeleteM1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/Traffic/DeleteM1"),
    {
      params
    }
  );
};
export const DeleteM3 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/Traffic/DeleteM3"),
    {
      params
    }
  );
};
export const DeleteW1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/Traffic/DeleteW1"),
    {
      params
    }
  );
};