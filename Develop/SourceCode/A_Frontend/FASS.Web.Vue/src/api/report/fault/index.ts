import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 获取故障页面数据 /api/v1/Report/Efficiency/GetAlarmReport
export const GetAlarmReport = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Report/Efficiency/GetAlarmReport"),
    {
      params
    }
  );
};
