import { type Ref, ref, reactive, onMounted } from "vue";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { GetListByTypeToSelect } from "@/api/warehouse/area";
import { GetListByAreaToSelect } from "@/api/warehouse/storage";
import {
  GetDictItemListToSelect,
  GetMaterialListToSelect,
  AddWork
} from "@/api/mobile/pda";
import { message } from "@/utils/message";
// GetMaterialListToSelect
import { useMyI18n } from "@/plugins/i18n";
///  1.传递 PdaCall 拿到区域
///  2.传递区域ID获取储位
///  3.呼叫模式，直接传递区域code
export function useHook(tableRef?: Ref) {
  const { t } = useMyI18n();
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    areaId: "",
    storageId: "",
    callMode: "",
    materialId: ""
  });
  const loading = ref(true);
  const areaList = ref([]);
  const MaterialList = ref([]);
  const storageList = ref([]);
  const modelList = ref([]);
  // GetDictItemListToSelect

  async function handleSearch() {
    loading.value = true;
    loading.value = false;
  }

  async function handleConfirm(formRef) {
    loading.value = true;
    await formRef.validate(async (valid, fields) => {
      if (valid) {
        try {
          await AddWork(query);
          message("操作成功！", { type: "success" });
          let res = await GetMaterialListToSelect();
          MaterialList.value = res.data;
        } catch (error) {
          loading.value = false;
          console.error("Error:", error);
        }
        query.materialId = "";
        // let res = await AddWork(query);
        // if (res.code == 200) {
        //   message("操作成功！", { type: "success" });
        //   handleReset(formRef);
        // } else {
        //   loading.value = true;
        //   message("操作失败！", { type: "warning" });
        // }
      } else {
        console.log("error submit!", fields);
      }
    });
    loading.value = false;
  }

  async function FunctionGetListByTypeToSelect() {
    let data = await GetListByTypeToSelect({ type: "PdaCall" });
    areaList.value = data.data;
    let res = await GetMaterialListToSelect();
    MaterialList.value = res.data;
  }

  async function AreaChange(value: any) {
    query.storageId = "";
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
    handleSearch();
  }

  onMounted(async () => {
    handleSearch();
    FunctionGetListByTypeToSelect();
  });

  return {
    query,
    loading,
    areaList,
    modelList,
    MaterialList,
    storageList,
    deviceDetection,
    AreaChange,
    handleSearch,
    handleReset,
    handleConfirm
  };
}
