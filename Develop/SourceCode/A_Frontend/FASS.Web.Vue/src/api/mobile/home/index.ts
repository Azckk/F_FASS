import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

// 移动端

export const Logout = (data?: object) => {
  return http.request<ReponseResult>(
    "post",
    baseUrlApi("Mobile/Home/Logout"),
    {
      data
    }
  );
};