import { type Ref, ref, reactive, onMounted } from "vue";
// import { ElMessageBox } from "element-plus";
import { useMyI18n } from "@/plugins/i18n";
// import dayjs from "dayjs";
import { message } from "@/utils/message";
// import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { getPage, GetNodePosition, getSitePage } from "@/api/base/node/index";
import editForm from "../form.vue";
import locationFrom from "../location.vue";
import actionFrom from "../action/index.vue";
import extendFrom from "../extend/index.vue";
export function useHook(tableRef: Ref, zoneId: string) {
  const { t } = useMyI18n();
  // const switchLoadMap = ref({});
  const query = reactive({
    code: "",
    name: "",
    // kind: "",
    type: ""
  });
  // const formRef = ref();
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
      prop: "type"
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
      cellRenderer: ({ row }) => (
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
      cellRenderer: ({ row }) => (
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
    const res = ref();
    if (zoneId != "" && zoneId != null) {
      res.value = await getSitePage(zoneId, pageParam.value);
    } else {
      res.value = await getPage(pageParam.value);
    }
    dataList.value = res.value.data.rows;
    pagination.total = res.value.data.total;
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
      mapId: row?.mapId ?? null,
      id: row?.id ?? null,
      nodeId: row?.nodeId ?? null
    };
  }
  // 查看
  function handleDetail(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "查看",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow(row) },
      contentRenderer: () => editForm
    });
  }
  // 站点位置
  async function handleLocation(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    let { data } = await GetNodePosition({ KeyValue: row.nodeId });
    addDialog({
      title: "站点位置",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow(data) },
      contentRenderer: () => locationFrom
    });
  }
  // 动作
  async function handleAction(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    // console.log(row);
    addDialog({
      title: "动作",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow(row) },
      contentRenderer: () => actionFrom
    });
  }
  // 扩展
  function handleExtension(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "扩展",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow(row) },
      contentRenderer: () => extendFrom
    });
  }

  /*
  async function handleResetPassword(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", { type: "warning", draggable: true })
      .then(async () => {
        await resetPassword(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
  */

  onMounted(async () => {
    handleSearch();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    pagination,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDetail,
    handleLocation,
    handleAction,
    handleExtension
  };
}
