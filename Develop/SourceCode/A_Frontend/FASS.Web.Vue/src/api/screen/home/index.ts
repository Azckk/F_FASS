import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";


export const GetDictItem = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Screen/Home/GetDictItem"),
    {
      params
    }
  );
};

export const GetHome = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Screen/Home/GetHome"),
    {
      params
    }
  );
};

// 
export const GetPermission = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Screen/Home/GetPermission"),
    {
      params
    }
  );
};
export const GetUser = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Screen/Home/GetUser"),
    {
      params
    }
  );
};
export const Logout = (data?: object) => {
  return http.request<ReponseResult>(
    "post",
    baseUrlApi("Screen/Home/Logout"),
    {
      data
    }
  );
};