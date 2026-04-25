import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetCarListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Car/GetListToSelect")
  );
};
// http://127.0.0.1:10201/Data/Avoid/GetPage?pageParam=%7B%22where%22%3A%5B%5D%2C%22order%22%3A%5B%7B%22field%22%3A%22createAt%22%2C%22sequence%22%3A%22desc%22%7D%5D%2C%22number%22%3A1%2C%22size%22%3A10%7D
export const getPage = (params?: object) => {
  return http.request<ReponseResult>("get", baseUrlApi("Data/Avoid/GetPage"), {
    params
  });
};

export const getNodeList = (params?: object) => {
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
    baseUrlApi("Data/Avoid/addOrUpdate"),
    {
      params,
      data
    }
  );
};
// keyValue

export const deletes = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/Avoid/Delete"),
    {
      data
    }
  );
};

// 避让点
export const getAvoidCarPage = (keyValue: string, params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Avoid/CarGetPage?keyValue=" + keyValue),
    {
      params
    }
  );
};

export const AvoidCarAdd = (keyValue: string, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Data/Avoid/CarAdd?keyValue=" + keyValue),
    {
      data
    }
  );
};

export const AvoidCarDelete = (keyValue: string, data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Data/Avoid/CarDelete?keyValue=" + keyValue),
    {
      data
    }
  );
};

// http://127.0.0.1:10201/Data/Avoid/CarDelete?keyValue=3a1234cc-22b8-3bf1-c159-f17ece5e5245
