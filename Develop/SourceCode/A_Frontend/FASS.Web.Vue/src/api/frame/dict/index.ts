import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

export const getPage = (params?: object) => {
  return http.request<ReponseResult>("get", baseUrlApi("frame/dict/getPage"), {
    params
  });
};
export const addOrUpdate = (keyValue?: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`frame/dict/addOrUpdate?keyValue=${keyValue}`),
    {
      data
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("frame/dict/delete"),
    {
      data
    }
  );
};

export const SetSync = (data?: object) => {
  return http.request<ReponseResult>("post", baseUrlApi("Frame/Dict/SetSync"), {
    data
  });
};
