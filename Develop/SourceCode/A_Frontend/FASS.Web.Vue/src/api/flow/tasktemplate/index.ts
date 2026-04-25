import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect"),
  );
};

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplate/GetPage"),
    {
      params
    }
  );
};

export const GetListToSelectByCarTypeCode = (carTypeCode?: string) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarAction/GetListToSelectByCarTypeCode?carTypeCode="+carTypeCode),
  );
};

export const putTaskTemplateAction = (carTypeCode?: string , data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskTemplateAction/AddOrUpdate?keyValue=" + carTypeCode),
     {
      data
    }
  );
};

//动作增加参数
export const putTaskTemplateParameter = (carTypeCode?: string , data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskTemplateParameter/AddOrUpdate?keyValue=" + carTypeCode),
    {
      data
    }
  );
};

//动作增加参数
export const DeleteTaskTemplateParameter = ( data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskTemplateParameter/Delete"),
    {
      data
    }
  );
};

//动作增加参数
export const DeleteTaskTemplateAction = ( data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskTemplateAction/Delete"),
    {
      data
    }
  );
};

// http://localhost:10201/Flow/TaskTemplateAction/Delete

// http://localhost:10201/Flow/TaskTemplateParameter/Delete

//动作参数列表
export const getTaskTemplateParameterPage = (taskTemplateActionId?:string,params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplateParameter/GetPage?taskTemplateActionId="+taskTemplateActionId),
    {
      params
    }
  );
};


export const getTaskTemplateActionPage = (keyValue?: string,params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplateAction/GetPage?taskTemplateProcessId="+ keyValue),
    {
      params
    }
  );
};

// Flow/TaskTemplateAction/GetPage?taskTemplateProcessId=3a12255a-63aa-f209-b189-478117ec7db0&pagePa

export const getNodeList  = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Base/Node/GetListToSelect"),
    {
      params
    }
  );
};

export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskTemplate/AddOrUpdate"),
    {
      params,
      data
    }
  );
};

export const TaskTemplateProcessAddOrUpdate = (keyValue?: string, data?: object) => {
  // console.log('111111111111111111111111111111')
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Flow/TaskTemplateProcess/AddOrUpdate?keyValue="+keyValue),
    {
      data
    }
  );
};
export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskTemplate/Delete"),
    {
      data
    }
  );
};

export const processDeletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Flow/TaskTemplateProcess/Delete"),
    {
      data
    }
  );
};


export const getProcessPage = (keyValue?: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Flow/TaskTemplateProcess/GetPage?taskTemplateId="+keyValue),
    {
      params
    }
  );
};


/**
 * 下面方法还未调用
 */

/*
export const enable = (data?: object) => {
  return http.request<ReponseResult>("put", baseUrlApi("account/role/enable"), {
    data
  });
};

export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("account/role/disable"),
    {
      data
    }
  );
};
*/
