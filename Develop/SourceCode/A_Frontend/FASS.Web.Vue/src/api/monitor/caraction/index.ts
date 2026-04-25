import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import type { ReponseResult } from "../../types";

// 获取站点
export const getNodeList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/SelectGetPage"),
    {
      params
    }
  );
};
// 获取充电点
export const getChargeList = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Charge/SelectGetPage"),
    {
      params
    }
  );
};
// 初始化 /api/v1/Data/Car/ActionInit
export const ActionInit = (params?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("Data/Car/ActionInit"), {
    params
  });
};
// 强制结束任务  //结束任务 //强制停止车辆
export const ActionForceStop = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionForceStop"),
    {
      params
    }
  );
};
//  现场维修【完全下线】 接口变更
export const ActionRepair = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionRepair"),
    {
      params
    }
  );
};
//  返厂维修 // 未使用
//  【完全下线】变更为这个接口
export const ActionBlown = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionBlown"),
    {
      params
    }
  );
};
// 去某处
export const ActionCarGoToSomePlace = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionCarGoToSomePlace"),
    // baseUrlApi("car/gosite"),
    {
      params
    }
  );
};
// 重启
export const ActionReStart = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionReStart"),
    {
      params
    }
  );
};

// 禁用站点
export const EnableNode = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Base/Node/EnableNode"),
    {
      params
    }
  );
};
// 启用站点
export const DisableNode = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Base/Node/DisableNode"),
    {
      params
    }
  );
};
// 查询站点状态
export const GetNodeInfo = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetNodeInfo"),
    {
      params
    }
  );
};

export const GetCarInfo = (params?: string | number) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Monitor/Car/GetCarInfo?keyValue=" + params),
    {
      // params
    }
  );
};

export const GetPcHardware = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Monitor/Car/GetPcHardware"),
    {
      params
    }
  );
};

// 停接/续接任务
export const ActionStopReceiveTask = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionStopReceiveTask"),
    {
      params
    }
  );
};
// 前往待命点
export const ActionGoToStandby = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionGoToStandby"),
    {
      params
    }
  );
};

// 前往充电点
export const ActionGoToCharge = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionGoToCharge"),
    {
      params
    }
  );
};
// 暂停/恢复车辆
export const ActionStopOrStart = (params?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Car/ActionStopOrStart"),
    {
      params
    }
  );
};
