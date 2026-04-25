import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

export const Save = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi(`/Map/Edit/Save`), {
    data
  });
};
export const GetMap = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Runtime/GetMap"),
    {
      params
    }
  );
};
