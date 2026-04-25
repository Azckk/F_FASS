import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// /api/v1/Monitor/Alarm/GetPage
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Alarm/GetPage"),
    {
      params
    }
  );
};

export const DeleteAll = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/AlarmMDCS/DeleteAll"),
    {
      params
    }
  );
};
export const DeleteD1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/AlarmMDCS/DeleteD1"),
    {
      params
    }
  );
};
export const DeleteM1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/AlarmMDCS/DeleteM1"),
    {
      params
    }
  );
};
export const DeleteM3 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/AlarmMDCS/DeleteM3"),
    {
      params
    }
  );
};
export const DeleteW1 = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Record/AlarmMDCS/DeleteW1"),
    {
      params
    }
  );
};

export const ExportExcel = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Alarm/ExportExcel"),
    {
      headers: {
        accept: "*/*"
      },
      responseType: "arraybuffer" // 确保设置为 arraybuffer
    }
  );
};
