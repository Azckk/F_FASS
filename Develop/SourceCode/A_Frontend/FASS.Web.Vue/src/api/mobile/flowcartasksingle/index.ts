import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// 移动端

export const getPage = (params?: object,) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Mobile/FlowCarTaskSingle/GetPage"),
    {
       params
    }
  );
};

export const Add = (params?: object,data?: object) => {
    return http.request<ReponseResult>(
      "put",
      baseUrlApi(`Mobile/FlowCarTaskSingle/Add`),
    //   baseUrlApi(`Mobile/FlowCarTaskSingle/Add?carId=${carId}&targetNodeId=${targetNodeId}`),
      {
        params,
        data
      }
    );
  };