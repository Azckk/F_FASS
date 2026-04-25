import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";


// 获取页面数据
export const getData = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Screen/Data/GetData"),
    {
      params
    }
  );
};
// 获取页面数据2222
export const GetIndexData = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Report/Home/GetIndexData"),
    {
      params
    }
  );
};
