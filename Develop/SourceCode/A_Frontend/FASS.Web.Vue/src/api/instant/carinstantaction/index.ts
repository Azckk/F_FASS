import { http } from "@/utils/http";
import { baseUrlApi } from "../../utils";
import { ReponseResult } from "../../types";

export const GetTypeListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/CarType/GetListToSelect")
  );
};
// "/api/v1/Instant/CarInstantAction/AddOrUpdate
export const GetListToSelectByCarTypeCode = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(
      "/Instant/CarInstantAction/GetListToSelectByCarTypeCode?carTypeCode=Car"
    )
  );
};

export const getPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Instant/CarInstantAction/GetPage"),
    {
      params
    }
  );
};
// /api/v1/Data/Car/GetPage   /api/v1/Instant/CarInstantAction/GetPage
export const getCarPage = (params?: object) => {
  return http.request<ReponseResult>("get", baseUrlApi("Data/Car/GetPage"), {
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
export const GetListToSelectByCarId = (params?: String) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi(`Data/CarAction/GetListToSelectByCarId?carId=${params}`)
  );
};

export const addOrUpdate = (params?: object, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Instant/CarInstantAction/addOrUpdate"),
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
    baseUrlApi("Instant/CarInstantAction/Delete"),
    {
      data
    }
  );
};

export const ForceDelete = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Instant/CarInstantAction/ForceDelete"),
    {
      data
    }
  );
};

export const enable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Instant/CarInstantAction/Enable"),
    {
      data
    }
  );
};
export const disable = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Instant/CarInstantAction/disable"),
    {
      data
    }
  );
};
// /api/v1/Instant/CarInstantAction/Delete

export const DeleteM3 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Instant/CarInstantAction/DeleteM3"),
    {
      data
    }
  );
};

export const DeleteM1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Instant/CarInstantAction/DeleteM1"),
    {
      data
    }
  );
};

export const DeleteW1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Instant/CarInstantAction/DeleteW1"),
    {
      data
    }
  );
};

export const DeleteD1 = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Instant/CarInstantAction/DeleteD1"),
    {
      data
    }
  );
};

export const DeleteAll = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Instant/CarInstantAction/DeleteAll"),
    {
      data
    }
  );
};
export const Release = (data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi("Instant/CarInstantAction/Release"),
    {
      data
    }
  );
};
//  参数 /api/v1/Instant/CarInstantParameter/GetPage
// export const GetParametersPage = (
//   carlnstantActionld: string,
//   params?: object
// ) => {
//   return http.request<ReponseResult>(
//     "get",
//     baseUrlApi(
//       "Instant/CarInstantParameter/GetPage?carInstantActionId=" +
//         carlnstantActionld
//     ),
//     {
//       params
//     }
//   );
// };

export const GetParametersPage = (params?: object) => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Instant/CarInstantParameter/GetPage"),
    {
      params
    }
  );
};

export const addOrUpdateParameters = (keyValue: String, data?: object) => {
  return http.request<ReponseResult>(
    "put",
    baseUrlApi(`Instant/CarInstantParameter/addOrUpdate?keyValue=${keyValue}`),
    {
      data
    }
  );
};

export const DeleteParameters = (data?: object) => {
  return http.request<ReponseResult>(
    "delete",
    baseUrlApi("Instant/CarInstantParameter/Delete"),
    {
      data
    }
  );
};

export const GetCarListToSelect = () => {
  return http.request<ReponseResult>(
    "get",
    baseUrlApi("Data/Car/GetListToSelect")
  );
};
