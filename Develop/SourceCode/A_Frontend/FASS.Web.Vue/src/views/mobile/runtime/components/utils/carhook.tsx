import { type Ref, h, ref, reactive, onMounted, createApp } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { router, resetRouter } from "@/router";

import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { getCarPage, GetUpdate } from "@/api/monitor/runtime";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef?: Ref | undefined) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  // TaskInstanceType
  const TaskInstanceTypeList = ref([]); // 任务实例类型列表
  const TaskInstanceStateList = ref([]); // 任务实例类型列表
  const nodeList = ref(); //站点
  const loading = ref(true);
  const carTypeList = ref([]);
  const TaskTemplateList = ref([]); //模板选择
  const ListToSelectByTaskTemplateId = ref([]); //模板获取车辆
  const dataList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });

  const columns: TableColumnList = [
    // { type: "selection", align: "left" },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "name",
      sortable: true
    },
    {
      headerRenderer: () => t("table.state"),
      label: "table.state",
      prop: "state",
      sortable: true
    },
    {
      headerRenderer: () => t("table.electricity"),
      label: "table.electricity",
      prop: "electricity",
      sortable: true
    },
    { label: "操作", slot: "operation" }
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
    const { data } = await GetUpdate();
    dataList.value = data.car;
    pagination.total = data.car.total ? data.car.total : pagination.total;
    loading.value = false;
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }

  function handleRow(row = undefined) {
    return {
      code: row?.code ?? null,
      name: row?.name ?? null
    };
  }

  function handleRouter(routerLink: string) {
    router.push(routerLink);
  }

  onMounted(async () => {
    handleSearch();
  });

  return {
    query,
    loading,
    columns,
    carTypeList,
    TaskTemplateList,
    TaskInstanceTypeList,
    ListToSelectByTaskTemplateId,
    nodeList,
    dataList,
    pagination,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleRouter
  };
}
