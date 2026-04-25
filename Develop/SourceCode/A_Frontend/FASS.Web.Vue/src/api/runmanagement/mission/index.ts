import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";
//获取进程左侧列表
export const MissionGetPage = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("RunManagement/Mission/GetPage")
  );
};

//获取进程状态
export const GetMissionStatus = (keyValue?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`RunManagement/Mission/GetMissionStatus?missionId=${keyValue}`)
  );
};
//获取进程属性
export const GetMissionFields = (keyValue?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`RunManagement/Mission/GetMissionFields?missionId=${keyValue}`)
  );
};
//获取进程属性
export const GetMissionMethods = (keyValue?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`RunManagement/Mission/GetMissionMethods?missionId=${keyValue}`)
  );
};

//新增和编辑属性
export const SetMissionFields = (
  keyValue?: string,
  key?: string,
  value?: string
) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(
      `RunManagement/Mission/SetMissionFields?missionId=${keyValue}&key=${key}&value=${value}`
    )
  );
};
//删除属性
export const DeleteMissionField = (keyValue?: string, key?: string) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(
      `RunManagement/Mission/DeleteMissionField?missionId=${keyValue}&key=${key}`
    )
  );
};

//动作执行
export const ExecuteMissionMethod = (
  keyValue?: string,
  method?: string,
  param?: string
) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(
      `RunManagement/Mission/ExecuteMissionMethod?missionId=${keyValue}&method=${method}&param=${param}`
    )
  );
};
