import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { type PaginationProps } from "@pureadmin/table";
import { allowMouseEvent, deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { Logout } from "@/api/mobile/home/index";
import { useUserStore } from "@/store/modules/user";
import { router, resetRouter } from "@/router";

import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef?: Ref) {
  const { t } = useMyI18n();
  const switchLoadMap = ref({});
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: "",
    kind: "",
    type: ""
  });
  const formRef = ref();
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
    { type: "selection", align: "left" },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.type"),
      label: "table.type",
      prop: "type",
      sortable: true
    },
    {
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code",
      sortable: true
    },
    {
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "name",
      sortable: true
    },
    {
      headerRenderer: () => t("table.isLock"),
      label: "table.isLock",
      prop: "isLock",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.isLock ? "green" : "red"} class="no-inherit">
          {row.isLock ? (
            <div style="display: flex; align-items: center">
              <iconify-icon-online icon="ep:check" />
              <span style="margin-left: 10px">{row.date}</span>
            </div>
          ) : (
            <div style="display: flex; align-items: center">
              <iconify-icon-online icon="ep:close" />
              <span style="margin-left: 10px">{row.date}</span>
            </div>
          )}
        </el-icon>
      )
    },
    {
      headerRenderer: () => t("table.sequenceId"),
      label: "table.sequenceId",
      prop: "sequenceId",
      sortable: true
    },
    {
      headerRenderer: () => t("table.release"),
      label: "table.release",
      prop: "isEnable",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.isEnable ? "green" : "red"} class="no-inherit">
          {row.isEnable ? (
            <div style="display: flex; align-items: center">
              <iconify-icon-online icon="ep:check" />
              <span style="margin-left: 10px">{row.date}</span>
            </div>
          ) : (
            <div style="display: flex; align-items: center">
              <iconify-icon-online icon="ep:close" />
              <span style="margin-left: 10px">{row.date}</span>
            </div>
          )}
        </el-icon>
      )
    }
    // { label: "操作", slot: "operation", minWidth: 120 }
  ];
  const store = useUserStore();
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
    // loading.value = true;
    // pageParam.value = handlePageParam();
    // const { data } = await getPage(pageParam.value);
    // dataList.value = data.rows;
    // pagination.total = data.total;
    // loading.value = false;
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
  // 查看
  function handleDetail(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
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

  onMounted(async () => {
    // handleSearch();
  });

  return {
    // query,
    // loading,
    // columns,
    // dataList,
    // pagination,
    // deviceDetection,
    // handleSelection,
    // handlePageSize,
    // handlePageCurrent,
    // handleSearch,
    // handleReset,
    // handleDetail,
    // handleLogout,
    // handleGoMenu,
    handleRouter
  };
}
