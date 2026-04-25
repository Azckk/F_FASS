import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 获取页面数据
export const getData = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("signal/GeneralSafetySignal/GetSafetySignals"),
    {
      params
    }
  );
};
