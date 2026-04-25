import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const getData = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("setting/configService/getData")
  );
};
export const setData = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("setting/configService/setData"),
    {
      data
    }
  );
};
