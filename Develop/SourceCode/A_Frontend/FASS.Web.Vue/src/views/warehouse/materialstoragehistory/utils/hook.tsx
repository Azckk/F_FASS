import { type Ref, h, ref, reactive, onMounted } from "vue";
import dayjs from "dayjs";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { ElMessageBox } from "element-plus";
import { message } from "@/utils/message";
import {
  getPage,
  DeleteAll,
  DeleteD1,
  DeleteM1,
  DeleteM3,
  DeleteW1
} from "@/api/warehouse/materialstoragehistory/index";
// import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    materialCode: "",
    storageCode: "",
    storageName: ""
  });
  const CarControlTypeList = ref();
  const CarAvoidTypeList = ref(); //避让类型
  const nodeList = ref(); //站点
  const loading = ref(true);
  const dataList = ref([]);
  const MaterialStorageHistoryStateList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const isEnglish = new RegExp("[A-Za-z]+");
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });
  const columns: TableColumnList = [
    { type: "selection", align: "left" },
    {
      headerRenderer: () => t("table.materialCode"),
      label: "table.materialCode",
      prop: "materialCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.storageCode"),
      label: "table.storageCode",
      prop: "storageCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.storage"),
      label: "table.storage",
      prop: "storageName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.status"),
      label: "table.status",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        let dictResult = MaterialStorageHistoryStateList.value.filter(
          x => x.code === state
        );
        if (dictResult && dictResult[0]) {
          if (!isEnglish.test(t("table.type"))) {
            return dictResult[0].name;
          } else {
            return dictResult[0].code;
          }
        }
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      minWidth: 120,
      sortable: true,
      formatter: ({ createAt }) => dayjs(createAt).format("YYYY-MM-DD HH:mm:ss")
    }
  ];

  function handleSelection(val) {
    selection.value = val;
  }

  function handlePageSize() {
    handleSearch();
  }

  function handlePageCurrent() {
    handleSearch();
  }

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
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await getPage(pageParam.value);
    dataList.value = data ? data.rows : [];
    pagination.total = data ? data.total : 0;
    loading.value = false;
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }

  async function GetMaterialStorageHistoryStateList() {
    try {
      const data = await GetDictItemInLocal("MaterialStorageHistoryState");
      MaterialStorageHistoryStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }
  async function handleDeleteAll(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteAll();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  function handleDeleteD1(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteD1();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  function handleDeleteW1(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteW1();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  function handleDeleteM1(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteM1();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
  function handleDeleteM3(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteM3();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  onMounted(async () => {
    handleSearch();
    GetMaterialStorageHistoryStateList();
  });

  return {
    query,
    loading,
    columns,
    nodeList,
    dataList,
    pagination,
    CarControlTypeList,
    CarAvoidTypeList,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDeleteM3,
    handleDeleteM1,
    handleDeleteW1,
    handleDeleteD1,
    handleDeleteAll
  };
}
