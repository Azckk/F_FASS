import { type Ref, ref, reactive, onMounted } from "vue";
import { message } from "@/utils/message";
import { ElMessageBox } from "element-plus";

import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { GetDictItemInLocal } from "@/utils/auth";
import { getPage, deleteApi } from "@/api/mobile/prework";

import { useMyI18n } from "@/plugins/i18n";
const isEnglish = new RegExp("[A-Za-z]+");
export function useHook(tableRef?: Ref) {
  const { t } = useMyI18n();
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: ""
  });
  // const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const carList = ref();
  const nodeList = ref();
  const selection = ref([]);
  const pageParam = ref({});
  const DictItemInLocalList = ref();
  const workStateList = ref([]);
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });
  const columns: TableColumnList = [
    { type: "selection", align: "left" },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code",
      sortable: true
    },
    {
      headerRenderer: () => t("table.materialCode"),
      label: "table.materialCode",
      prop: "materialCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "name",
      sortable: true
    },
    {
      headerRenderer: () => t("table.srcStorageId"),
      label: "table.srcStorageName",
      prop: "srcStorageName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.destStorageId"),
      label: "table.destStorageName",
      prop: "destStorageName",
      sortable: true
    },

    {
      headerRenderer: () => t("table.state"),
      label: "table.state",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        let dictResult = workStateList.value.filter(x => x.code === state);
        if (dictResult && dictResult[0]) {
          if (!isEnglish.test(t("table.type"))) {
            return dictResult[0].name;
          } else {
            return dictResult[0].code;
          }
        }
        return state;
      }
    },

    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      sortable: true
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
    dataList.value = data.rows;
    pagination.total = data.total;
    loading.value = false;
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }
  async function GetWorkStateList() {
    try {
      const data = await GetDictItemInLocal("PreWorkState");
      workStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  function handleRow(row = undefined) {
    return {
      name: row?.name ?? null,
      code: row?.code ?? null,
      isEnable: row?.isEnable ?? true,
      isLock: row?.isLock ?? true,
      sequenceId: row?.sequenceId ?? null,
      nodeDescription: row?.nodeDescription ?? null,
      kind: row?.kind ?? null,
      type: row?.type ?? null,
      x: row?.x ?? null,
      y: row?.y ?? null,
      theta: row?.theta ?? null,
      allowedDeviationXY: row?.allowedDeviationXY ?? null,
      allowedDeviationTheta: row?.allowedDeviationTheta ?? null,
      mapId: row?.mapId ?? null
    };
  }

  // 删除
  async function handleDelete(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        console.log(rows.map(e => e.id));
        await deleteApi(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  onMounted(async () => {
    handleSearch();
    GetWorkStateList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    pagination,
    carList,
    nodeList,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDelete,
    DictItemInLocalList
  };
}
