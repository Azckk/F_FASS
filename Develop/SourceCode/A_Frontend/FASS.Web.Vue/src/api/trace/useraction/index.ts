import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("trace/userAction/getPage"),
    {
      params
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("trace/userAction/delete"),
    {
      data
    }
  );
};
export const deleteM3 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("trace/userAction/deleteM3"),
    {
      data
    }
  );
};
export const deleteM1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("trace/userAction/deleteM1"),
    {
      data
    }
  );
};
export const deleteW1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("trace/userAction/deleteW1"),
    {
      data
    }
  );
};
export const deleteD1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("trace/userAction/deleteD1"),
    {
      data
    }
  );
};
export const deleteAll = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("trace/userAction/deleteAll"),
    {
      data
    }
  );
};
