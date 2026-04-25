import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { v4 as uuidv4 } from "uuid";
import {
  TaskTemplateMDCS,
  GetNodePage,
  GetCarPage,
  AddTaskRecord,
  LogisticsRoute,
  GetAreaListToSelect
} from "@/api/warehouse/visualization";

// import editForm from "../form.vue";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef?: Ref | undefined) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: ""
  });
  const pageParam = ref();
  const TaskTemplateDataList = ref();
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
  const columns: TableColumnList = [
    { type: "selection", align: "left" },
    { label: "编号", prop: "id", hide: true }
  ];

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

  async function handleSearch() {
    // loading.value = true;
    // pageParam.value = handlePageParam();
    // const { data } = await getPage(pageParam.value);
    // dataList.value = data.rows;
    // pagination.total = data.total;
    // loading.value = false;
  }

  function handleRow(row = undefined) {
    return {
      areaId: row?.areaId ?? "",
      nodeId: row?.nodeId ?? "",
      nodeCode: row?.nodeCode ?? "",
      code: row?.code ?? "",
      name: row?.name ?? "",
      type: row?.type ?? "Default",
      isLock: row?.isLock !== undefined ? row.isLock : true,
      state: row?.state ?? "NoneContainer",
      barcode: row?.barcode ?? ""
    };
  }
  let formRef = ref();
  async function handleSendWorkFn(row) {
    await formRef.value.validate(async valid => {
      if (valid) {
        let data = {
          taskTemplateId: row.taskTemplateId ? row.taskTemplateId : "123456",
          // carId: selectedCar.value.carId,
          carId: row.carId,
          // formLabelAlign.carId
          carTypeId: selectedCar.value?.carTypeId ?? "",
          // code: uuidv4(),
          code: selectedCar.value?.code ?? "",
          type: row.srcStorageId ? "Template" : "LogisticsRoute",
          srcStorageId: row.srcStorageId ? row.srcStorageId : "",
          destStorageId: row.destStorageId ? row.destStorageId : "",
          srcAreaId: row.srcAreaId ? row.srcAreaId : "",
          destAreaId: row.destAreaId ? row.destAreaId : ""
        };
        let res = await AddTaskRecord(data);
        if (res.code == 200 && res.data == "OK") {
          message("发送成功！", { type: "success" });
        }
      } else {
        // console.log("error submit!");
      }
    });
  }
  async function getLogisticsRoute() {
    pageParam.value = handlePageParam();
    const { data } = await LogisticsRoute(pageParam.value);
    LogisticsRouteDataList.value = data.rows;
  }
  async function getTaskTemplateMDCS() {
    pageParam.value = handlePageParam();
    const { data } = await TaskTemplateMDCS(pageParam.value);
    TaskTemplateDataList.value = data;
  }
  async function getNodePage() {
    pageParam.value = handlePageParam();
    const { data } = await GetNodePage(pageParam.value);
    nodeDataList.value = data;
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
    getTaskTemplateMDCS();
    getNodePage();
    getCarPage();
    getLogisticsRoute();
    GetListToSelectFn();
  });

  return {
    nodeDataList,
    TaskTemplateDataList,
    carDataList,
    LogisticsRouteDataList,
    areaDataList,
    formRef,
    handleSendWorkFn,
    handleCarChange
  };
}
