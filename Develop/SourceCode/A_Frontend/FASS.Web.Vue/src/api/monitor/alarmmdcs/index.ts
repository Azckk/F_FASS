import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// /api/v1/Record/AlarmMDCS/GetPage
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Record/AlarmMDCS/GetPage"),
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

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Record/AlarmMDCS/Delete"),
    {
      data
    }
  );
};
export const ExportExcel = (data?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Record/AlarmMdcs/ExportExcel"),
    {
      headers: {
        accept: "*/*"
      },
      responseType: "arraybuffer" // 确保设置为 arraybuffer
    }
  );
};
