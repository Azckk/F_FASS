import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
// import { useI18n } from "vue-i18n";
import { useMyI18n } from "@/plugins/i18n";
import { message } from "@/utils/message";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { GetDictItemInLocal } from "@/utils/auth";
import { getNodeList } from "@/api/data/avoid";
import {
  getPage,
  addOrUpdate,
  deletes,
  enable,
  disable
} from "@/api/base/edge";
import type { FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import Check from "@iconify-icons/ep/check";
import actionFrom from "../action/index.vue";
import extendFrom from "../extend/index.vue";
import routerTrajectoryFrom from "../routertrajectory/form.vue";
import operationForm from "../routertrajectory/operation/index.vue";
export function useHook(tableRef: Ref) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: "",
    // kind: "",
    type: ""
  });
  const EdgeOrientationTypeList = ref([]);
  const nodeList = ref([]);
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
      sortable: true,
      minWidth: 120
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
      headerRenderer: () => t("table.isOneway"),
      label: "table.isOneway",
      prop: "isOneway",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.isOneway ? "green" : "red"} class="no-inherit">
          {row.isOneway ? (
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
      prop: "released",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.released ? "green" : "red"} class="no-inherit">
          {row.released ? (
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
      headerRenderer: () => t("table.startingPointLandmark"),
      label: "table.startingPointLandmark",
      prop: "startNodeCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.endLandmark"),
      label: "table.endLandmark",
      prop: "endNodeCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.maxSpeed"),
      label: "table.maxSpeed",
      prop: "maxSpeed",
      sortable: true
    },
    {
      headerRenderer: () => t("table.maxHeight"),
      label: "table.maxHeight",
      prop: "maxHeight",
      sortable: true
    },
    {
      headerRenderer: () => t("table.minHeight"),
      label: "table.minHeight",
      prop: "minHeight",
      sortable: true
    },
    {
      headerRenderer: () => t("table.routeDirection"),
      label: "table.routeDirection",
      prop: "orientation",
      sortable: true
    },
    {
      headerRenderer: () => t("table.directionType"),
      label: "table.directionType",
      prop: "orientationType",
      sortable: true,
      formatter: ({ orientationType }) => {
        const dictResult = EdgeOrientationTypeList.value.filter(
          x => x.code === orientationType
        );
        if (dictResult && dictResult[0]) {
          if (isEnglish.test(t("table.directionType"))) {
            return dictResult[0].code;
          } else {
            return dictResult[0].name;
          }
        }
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.allowRotation"),
      label: "table.allowRotation",
      prop: "rotationAllowed",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <el-icon
          color={row.rotationAllowed ? "green" : "red"}
          class="no-inherit"
        >
          {row.rotationAllowed ? (
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
      headerRenderer: () => t("table.maxSteering"),
      label: "table.maxSteering",
      prop: "maxRotationSpeed",
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

  function handleRow(row = undefined) {
    return {
      kind: row?.kind ?? null,
      type: row?.type ?? null,
      code: row?.code ?? null,
      name: row?.name ?? null,
      isLock: row?.isLock !== undefined ? row.isLock : true,
      // maxCar: row?.maxCar ?? null,
      isOneway: row?.isOneway !== undefined ? row.isOneway : true,
      sequenceId: row?.sequenceId ?? null,
      released: row?.released !== undefined ? row.released : true,
      startNodeCode: row?.startNodeCode ?? null,
      startNodeId: row?.startNodeId ?? null,
      endNodeCode: row?.endNodeCode ?? null,
      endNodeId: row?.endNodeId ?? null,
      maxSpeed: row?.maxSpeed ?? null,
      maxHeight: row?.maxHeight ?? null,
      minHeight: row?.minHeight ?? null,
      orientation: row?.orientation ?? null,
      orientationType: changeName(row?.orientationType),
      rotationAllowed: row?.rotationAllowed ?? null,
      maxRotationSpeed: row?.maxRotationSpeed ?? null,
      length: row?.length ?? null,

      id: row?.id ?? null,
      nodeId: row?.nodeId ?? null,
      edgeId: row?.edgeId ?? null
    };
  }
  let changeName = orientationType => {
    // console.log(orientationType);
    const dictResult = EdgeOrientationTypeList.value.filter(
      x => x.code === orientationType
    );
    if (dictResult && dictResult[0]) {
      if (isEnglish.test(t("table.directionType"))) {
        return dictResult[0].code;
      } else {
        return dictResult[0].name;
      }
    }
    return "未知";
  };
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

  function handleUpdate(rows = selection.value) {
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

  async function getNodeListToSelect() {
    const { data } = await getNodeList();
    nodeList.value = [...data];
  }

  // EdgeOrientationType
  async function GetEdgeOrientationType() {
    try {
      const data = await GetDictItemInLocal("EdgeOrientationType");
      EdgeOrientationTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  // 动作
  async function handleAction(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
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
  function operation() {
    addDialog({
      title: "动作参数",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow() },
      contentRenderer: () => operationForm
    });
  }
  function handleRouteTrajectory(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "路线轨迹",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow(row), operation: operation },
      contentRenderer: () => routerTrajectoryFrom
    });
  }
  onMounted(async () => {
    handleSearch();
    getNodeListToSelect();
    GetEdgeOrientationType();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    nodeList,
    EdgeOrientationTypeList,
    pagination,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDetail,
    handleUpdate,
    handleAction,
    handleExtension,
    handleRouteTrajectory
  };
}
