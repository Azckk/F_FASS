import { type Ref, ref, reactive, onMounted } from "vue";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { GetListByTypeToSelect } from "@/api/warehouse/area";
import { GetListByAreaToSelect } from "@/api/warehouse/storage";
import { getPage } from "@/api/mobile/warehousedatacontainer";
import {
  GetDictItemListToSelect,
  GetMaterialListToSelect,
  MaterialAddBand
} from "@/api/mobile/pda";
import { message } from "@/utils/message";
// GetMaterialListToSelect
import { useMyI18n } from "@/plugins/i18n";
import { v4 as uuidv4 } from "uuid";
import { size } from "lodash";
///  1.传递 PdaCall 拿到区域
///  2.传递区域ID获取储位
///  3.呼叫模式，直接传递区域code
export function useHook(tableRef?: Ref) {
  const { t } = useMyI18n();
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    name: "",
    code: "",
    callMode: "",
    area: "",
    barcode: "",
    type: "Default",
    state: "UnBind",
    isLock: false,
    batch: "",
    spec: "",
    unit: "",
    quantity: "1"
  });
  const loading = ref(true);
  const areaList = ref([]);
  const MaterialList = ref([]);
  const storageList = ref([]);
  const modelList = ref([]);
  const containerListData = ref([]);

  const pageParam = ref({});
  // const pagination = reactive<PaginationProps>({
  //   total: 0,
  //   pageSize: 10,
  //   currentPage: 1,
  //   background: true
  // });

  // GetDictItemListToSelect

  async function handleConfirm(formRef) {
    loading.value = true;
    await formRef.validate(async (valid, fields) => {
      if (valid) {
        const keyValue = query.callMode;
        query.code = uuidv4();
        await MaterialAddBand(keyValue, query)
          .then(res => {
            console.log(res);
            if (res.code == 200) {
              message("操作成功！", { type: "success" });
              handleReset(formRef);
            } else {
              message("操作失败！", { type: "warning" });
            }
          })
          .catch(err => {
            console.log(err); //捕获异常数据
          });
      } else {
        console.log("error submit!", fields);
        // loading.value = true;
      }
    });
    loading.value = false;
  }

  async function FunctionGetListByTypeToSelect() {
    loading.value = false;
    let data = await GetListByTypeToSelect({ type: "PdaCall" });
    areaList.value = data.data;
    let res = await GetMaterialListToSelect();
    MaterialList.value = res.data;
  }

  async function AreaChange(value: any) {
    query.callMode = "";
    let row = areaList.value.filter(area => area.id === value)[0];
    if (row) {
      let res = await GetListByAreaToSelect({ areaId: row.id });
      storageList.value = res.data;
      let data = await GetDictItemListToSelect({
        dictCode: "CallMode",
        param: row.code
      });
      modelList.value = data.data;
      // console.log("data is", data);
      // storageList.value;
    }
  }

  function handleReset(form) {
    // query.storageId = "";
    // query.callMode = "";
    if (!form) {
      return;
    }
    form.resetFields();
  }

  function handlePageParam() {
    // return {
    //   pageParam: JSON.stringify({
    //     where: Object.keys(query)
    //       .filter(e => query[e])
    //       .map(e => ({
    //         logic: "And",
    //         field: e,
    //         operator: "Contains",
    //         value: query[e]
    //       })),
    //     order: [{ field: "createAt", sequence: "DESC" }],
    //     number: pagination.currentPage,
    //     size: pagination.pageSize
    //   })
    // };
    return {
      pageParam: JSON.stringify({
        where: [],
        order: [{ field: "createAt", sequence: "DESC" }],
        number: 1,
        size: 1000
      })
    };
  }
  //获取所有容器
  async function containerList() {
    pageParam.value = handlePageParam();
    const { data } = await getPage(pageParam.value);
    const filterContainer = data.rows;
    containerListData.value = filterContainer.filter(item => {
      return item.state === "EmptyMaterial";
    });
    // containerListData.value = filterContainer
  }

  onMounted(async () => {
    FunctionGetListByTypeToSelect();
    containerList();
  });

  return {
    query,
    loading,
    areaList,
    // modelList,
    MaterialList,
    storageList,
    deviceDetection,
    AreaChange,
    handleReset,
    handleConfirm,
    containerListData
  };
}
