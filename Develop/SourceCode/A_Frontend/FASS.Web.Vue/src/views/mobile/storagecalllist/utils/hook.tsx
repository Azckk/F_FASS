import { type Ref, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import { message } from "@/utils/message";
import { GetDictItemInLocal } from "@/utils/auth";
import dayjs from "dayjs";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { useUserStore } from "@/store/modules/user";
import { router, resetRouter } from "@/router";
import { GetPage } from "@/api/mobile/pda";
import { useMyI18n } from "@/plugins/i18n";
const isEnglish = new RegExp("[A-Za-z]+");
export function useHook(tableRef?: Ref) {
  const { t } = useMyI18n();
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: "",
    kind: "",
    type: ""
  });
  const TaskInstanceStateList = ref([]); // 任务实例类型列表
  const loading = ref(true);
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
      headerRenderer: () => t("table.callarea"),
      label: "table.callarea",
      prop: "areaName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.callmodel"),
      label: "table.callmodel",
      prop: "name",
      sortable: true,
      formatter: ({ name }) => {
        const parts = name.split("-")[1];
        if (parts) return parts;
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.callstorage"),
      label: "table.callstorage",
      prop: "name",
      sortable: true,
      formatter: ({ name }) => {
        const parts = name.split("-")[0];
        if (parts) return parts;
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.state"),
      label: "table.state",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        let dictResult = TaskInstanceStateList.value.filter(
          x => x.code === state
        );
        if (dictResult && dictResult[0]) {
          if (!isEnglish.test(t("table.status"))) {
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
      formatter: ({ createAt }) => dayjs(createAt).format("YYYY-MM-DD HH:mm:ss")
    }
    // { label: "操作", slot: "operation", minWidth: 120 }
  ];
  const store = useUserStore();

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
    const { data } = await GetPage(pageParam.value);
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

  async function handleLogout(cancel = undefined) {
    ElMessageBox.confirm("是否确认退出？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        // await Logout();
        store.logOut();
        message("成功退出！", { type: "success" });
      })
      .catch(() => cancel?.());
  }

  function handleGoMenu() {
    // router.push("/mobile/menu/index");
    router.go(0);
  }
  function handleRouter(routerLink: string) {
    router.push(routerLink);
  }

  async function GetTaskInstanceStateList() {
    try {
      const data = await GetDictItemInLocal("TaskRecordState");
      TaskInstanceStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  onMounted(async () => {
    handleSearch();
    GetTaskInstanceStateList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    pagination,
    deviceDetection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleLogout,
    handleGoMenu,
    handleRouter
  };
}
