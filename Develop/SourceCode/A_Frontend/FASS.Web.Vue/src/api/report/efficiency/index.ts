import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 效率报表--任务 /api/v1/Report/Efficiency/GetTaskReport
export const GetTaskReport = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Report/Efficiency/GetTaskReport"),
    {
      params
    }
  );
};
