import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// /api/v1/Monitor/Car/GetPage
export const getCarPage = (params?: object) => {
  return http.request<ReponseResult>("get", baseUrlApi("Monitor/Car/GetPage"), {
    params
  });
};

export const getAlarmPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Record/AlarmMDCS/GetPage"),
    {
      params
    }
  );
};

export const getTaskPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskRecord/GetPage"),
    {
      params
    }
  );
};

// /api/v1/Record/AlarmMDCS/GetPage
export const GetSimpleMap = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Runtime/GetSimpleMap"),
    {
      params
    }
  );
};

export const GetStorageListToSelect = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Runtime/GetStorageListToSelect"),
    {
      params
    }
  );
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
export const GetUpdate = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Runtime/GetUpdate"),
    {
      params
    }
  );
};

export const GetPointCloudMap = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Runtime/GetPointCloudMap"),
    {
      params
    }
  );
};

export const GetSideInfo = (params?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetEntity?keyValue=" + params)
  );
};

export const GetCarMethods = (carCode?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Car/GetCarMethods?carCode=" + carCode)
  );
};
export const ExecuteCarMethod = data => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(
      `Monitor/Car/ExecuteCarMethod?carCode=${data.carCode}&method=${data.method}&param=${data.param}`
    )
  );
};
