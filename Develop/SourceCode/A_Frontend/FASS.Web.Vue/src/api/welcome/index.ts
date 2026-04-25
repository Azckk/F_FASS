import { http } from "@/utils/http";
import { baseUrlApi } from "../utils";
import type { ReponseResult } from "../types";

export const GetIndexData = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Report/Home/GetIndexData"),
    {
      params
    }
  );
};
