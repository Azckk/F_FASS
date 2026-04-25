import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// 站点管理
// 获取页面数据
export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Record/Diary/GetPage"),
    {
      params
    }
  );
};
// /api/v1/Record/Diary/DeleteAll
export const DeleteAll = (params?: object) => {
    return http.request<ReponseResult>(
      "put",
      baseUrlApi("Record/Diary/DeleteAll"),
      {
        params
      }
    );
  };
//   /api/v1/Record/Alarm/DeleteD1
export const DeleteD1 = (params?: object) => {
    return http.request<ReponseResult>(
      "put",
      baseUrlApi("Record/Diary/DeleteD1"),
      {
        params
      }
    );
  };
//   /api/v1/Record/Alarm/DeleteM1
export const DeleteM1 = (params?: object) => {
    return http.request<ReponseResult>(
      "put",
      baseUrlApi("Record/Diary/DeleteM1"),
      {
        params
      }
    );
  };
//   /api/v1/Record/Alarm/DeleteM3
export const DeleteM3 = (params?: object) => {
    return http.request<ReponseResult>(
      "put",
      baseUrlApi("Record/Diary/DeleteM3"),
      {
        params
      }
    );
  };
export const DeleteW1 = (params?: object) => {
    return http.request<ReponseResult>(
      "put",
      baseUrlApi("Record/Diary/DeleteW1"),
      {
        params
      }
    );
  };