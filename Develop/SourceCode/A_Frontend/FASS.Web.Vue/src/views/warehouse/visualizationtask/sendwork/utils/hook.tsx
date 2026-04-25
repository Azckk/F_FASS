import type { Ref } from "vue";
import { ref, reactive, onMounted } from "vue";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { GetDictItemInLocal } from "@/utils/auth";
import { GetAreaListToSelect, GetCarPage } from "@/api/warehouse/visualization";
import { AddWork } from "@/api/warehouse/visualizationTask";
import { useHook as panelHook } from "../../panel/utils/hook";
const panelHooks = panelHook();
// import editForm from "../form.vue";
export function useHook(
  keyValue?: Ref | undefined,
  aaa?: Ref | undefined,
  abc?: Ref | undefined
) {
  const formRefs = ref();
  const query = reactive({
    code: "",
    name: ""
  });
  const pageParam = ref();
  // const TaskTemplateDataList = ref();
  const CommonCallMode = ref();
  const nodeDataList = ref();
  const areaDataList = ref();
  const carDataList = ref();
  const LogisticsRouteDataList = ref();
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 9999,
    currentPage: 1,
    background: true
  });

  function handlePageParam() {
    return {
      pageParam: JSON.stringify({
        where: Object.keys(query)
          .filter(e => query[e])
          .map(e => ({
            logic: "And",
            field: e,
            operator: "Contains",
            value: query[e]
          })),
        order: [{ field: "createAt", sequence: "DESC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }

  // async function handleSearch() {
  // loading.value = true;
  // pageParam.value = handlePageParam();
  // const { data } = await getPage(pageParam.value);
  // dataList.value = data.rows;
  // pagination.total = data.total;
  // loading.value = false;
  // }
  async function GetCommonCallMode() {
    try {
      const data = await GetDictItemInLocal("CommonCallMode");
      CommonCallMode.value = [...data];
      console.log("CommonCallMode", data);
    } catch (error) {
      console.error("Error:", error);
    }
  }

  // function handleRow(row = undefined) {
  //   return {
  //     areaId: row?.areaId ?? "",
  //     nodeId: row?.nodeId ?? "",
  //     nodeCode: row?.nodeCode ?? "",
  //     code: row?.code ?? "",
  //     name: row?.name ?? "",
  //     type: row?.type ?? "Default",
  //     isLock: row?.isLock !== undefined ? row.isLock : true,
  //     state: row?.state ?? "NoneContainer",
  //     barcode: row?.barcode ?? ""
  //   };
  // }
  let formRef = ref();
  async function handleSendWorkFn(row) {
    // 任务下发
    await formRef.value.validate(async valid => {
      if (valid) {
        console.log(row.carTypeId);
        let data = {
          callMode: row.taskTemplateId ? row.taskTemplateId : "123456",
          // carId: selectedCar.value.carId,
          carId: row.carId,
          // formLabelAlign.carId
          carTypeId: row.carTypeId ? row.carTypeId : "",
          // code: uuidv4(),
          // code: selectedCar.value?.code ?? "",
          // type: row.srcStorageId ? "Template" : "LogisticsRoute",
          callStorageId: row.callStorageId ? row.callStorageId : "",
          srcStorageId: row.srcStorageId ? row.srcStorageId : "",
          destStorageId: row.destStorageId ? row.destStorageId : ""
          // srcAreaId: row.srcAreaId ? row.srcAreaId : "",
          // destAreaId: row.destAreaId ? row.destAreaId : ""
        };

        //校验起点库位和终点库位
        if (row.srcStorageId == row.destStorageId) {
          message("起点库位和终点库位重复", { type: "error" });
          return;
        }

        //校验呼叫库位和其他库位
        if (
          row.callStorageId === row.srcStorageId ||
          row.callStorageId === row.destStorageId
        ) {
          message("呼叫库位和其他库位重复！", { type: "error" });
          return;
        }

        let res = await AddWork(data);
        if (res.code == 200 && res.data == "OK") {
          panelHooks.updateColor();
          message("发送成功！", { type: "success" });
        }
      } else {
        // console.log("error submit!");
      }
    });
  }

  async function getCarPage() {
    pageParam.value = handlePageParam();
    const { data } = await GetCarPage(pageParam.value);
    carDataList.value = data;
  }
  async function GetListToSelectFn() {
    const { data } = await GetAreaListToSelect();
    areaDataList.value = data;
  }
  const selectedCar = ref();
  function handleCarChange(carId) {
    selectedCar.value = carDataList.value.find(car => car.id === carId);
    console.log("selectedCar.value is", selectedCar.value);
  }

  onMounted(async () => {
    getCarPage();
    GetListToSelectFn();
    GetCommonCallMode();
  });

  return {
    nodeDataList,
    CommonCallMode,
    carDataList,
    LogisticsRouteDataList,
    areaDataList,
    formRef,
    handleSendWorkFn,
    handleCarChange,
    formRefs
  };
}
