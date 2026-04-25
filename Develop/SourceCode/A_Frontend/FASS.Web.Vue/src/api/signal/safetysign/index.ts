import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 获取页面数据
export const getData = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    // "/scheduler/SecuritySignal/SecuritySignal",
    baseUrlApi("signal/SecuritySignal/SecuritySignal"),
    // :20101/api/v1/SecuritySignal/SecuritySignal?storageName=One
    {
      params
    }
  );
};

export const StorageGetPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      `Warehouse/Storage/GetListByAreaCodeToSelect?areaCode=${keyValue}`
    ),
    {
      params
    }
  );
};
