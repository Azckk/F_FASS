import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  getPage,
  addOrUpdate,
  deletes,
  enable,
  disable,
  ForceDelete,
  GetListToSelectByCarId,
  getCarPage,
  DeleteM3,
  DeleteM1,
  DeleteW1,
  DeleteD1,
  DeleteAll,
  Release,
  GetCarListToSelect
} from "@/api/instant/carinstantaction";
// import { getList } from "@/api/account/org";
import type { FormItemProps } from "../utils/types";
// import { usePublicHooks } from "../../hooks";
import editForm from "../form.vue";
import parametersFrom from "../parameters/index.vue";
import { useMyI18n } from "@/plugins/i18n";
import { GetDictItemInLocal } from "@/utils/auth";
import actionTypeData from "./actionType.json";
export function useHook(tableRef?: Ref | undefined) {
  const { t, locale } = useMyI18n();
  // const switchLoadMap = ref({});
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    carId: "",
    actionType: "",
    state: "",
    time: [dayjs().subtract(1, "week").toDate(), new Date()]
  });
  const formRef = ref();
  const loading = ref(true);
  const carList = ref([]);
  let operationTypeList = ref([]);
  const stateList = ref([]);
  const TaskInstanceActionStateList = ref([]);
  const dataList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const TaskTemplateActionBlockingTypeDataList = ref({});
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });
  const columns: TableColumnList = [
    { type: "selection", align: "left", sortable: true },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.car"),
      label: "table.car",
      prop: "remark",
      sortable: true
    },
    {
      headerRenderer: () => t("table.actionType"),
      label: "table.actionType",
      prop: "actionType",
      sortable: true,
      formatter: ({ actionType }) => {
        // 字典中没有该类型，引用自定义数据（根据GetListToSelectByCarId请求回来的）
        let dictResult = actionTypeData.filter(x => x.code === actionType);
        if (locale.value == "zh") {
          return dictResult[0]?.name;
        } else {
          return dictResult[0]?.code;
        }
      }
    },
    {
      headerRenderer: () => t("table.blockageType"),
      label: "table.blockageType",
      prop: "blockingType",
      sortable: true,
      formatter: ({ blockingType }) => {
        if (locale.value == "zh") {
          if (blockingType == "NONE") {
            return "无限制";
          } else if (blockingType == "SOFT") {
            return "禁止行驶";
          } else if (blockingType == "HARD") {
            return "禁止行驶且禁止其它动作";
          } else {
            return blockingType;
          }
        } else {
          return blockingType;
        }
      }
    },
    {
      headerRenderer: () => t("table.state"),
      label: "table.state",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        // TaskInstanceActionStateList
        let dictResult = TaskInstanceActionStateList.value.filter(
          x => x.code === state
        );
        if (locale.value == "zh") {
          return dictResult[0]?.name;
        } else {
          return dictResult[0]?.code;
        }
        // if (locale.value == "zh") {
        //   if (state == "Created") {
        //     return "创建";
        //   } else if (state == "Released") {
        //     return "已发布";
        //   } else {
        //     return state;
        //   }
        // } else {
        //   return state;
        // }
      }
    },
    {
      headerRenderer: () => t("table.startTime"),
      label: "table.startTime",
      prop: "startTime",
      sortable: true,
      formatter: ({ startTime }) =>
        startTime ? dayjs(startTime).format("YYYY-MM-DD HH:mm:ss") : ""
    },
    {
      headerRenderer: () => t("table.endTime"),
      label: "table.endTime",
      prop: "endTime",
      sortable: true,
      formatter: ({ endTime }) =>
        endTime ? dayjs(endTime).format("YYYY-MM-DD HH:mm:ss") : ""
    },
    {
      headerRenderer: () => t("table.remark"),
      label: "table.remark",
      prop: "remark",
      sortable: true
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      sortable: true,
      formatter: ({ createAt }) => dayjs(createAt).format("YYYY-MM-DD HH:mm:ss")
    }
    // {
    //   headerRenderer: () => t("table.operation"),
    //   label: "table.operation",
    //   slot: "operation",
    //   minWidth: 120,
    //   sortable: true
    // }
  ];
  const shortcuts = [
    {
      text: "Last week",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
        return [start, end];
      }
    },
    {
      text: "Last month",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
        return [start, end];
      }
    },
    {
      text: "Last 3 months",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
        return [start, end];
      }
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
    const where = Object.keys(query)
      .filter(key => query[key])
      .flatMap(key => {
        if (key === "time" && Array.isArray(query.time)) {
          // 处理时间范围
          const [start, end] = query.time;
          return [
            {
              logic: "And",
              field: "createAt",
              operator: "GreaterEqual",
              value: dayjs(start).startOf("day").format("YYYY-MM-DD HH:mm:ss")
            },
            {
              logic: "And",
              field: "createAt",
              operator: "LessEqual",
              value: dayjs(end).endOf("day").format("YYYY-MM-DD HH:mm:ss")
            }
          ];
        } else {
          // 处理其他查询条件
          return {
            logic: "And",
            field: key,
            operator: "Contains",
            value: query[key]
          };
        }
      });

    return {
      pageParam: JSON.stringify({
        where,
        order: [{ field: "sortNumber", sequence: "asc" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }
  const queryCar = reactive({
    carTypeId: "",
    code: "",
    name: ""
  });
  function handkleCarPageParam() {
    return {
      pageParam: JSON.stringify({
        where: Object.keys(queryCar)
          .filter(e => queryCar[e])
          .map(e => ({
            logic: "And",
            field: e,
            operator: "Contains",
            value: queryCar[e]
          })),
        order: [{ field: "sortNumber", sequence: "asc" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }
  async function getCarList() {
    pageParam.value = handkleCarPageParam();
    const { data } = await GetCarListToSelect();
    carList.value = data;
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
    query.carId = "";
    handleSearch();
  }

  function handleRow(row = undefined) {
    return {
      carId: row?.carId ?? null,
      actionType: row?.actionType ?? null,
      actionDescription: row?.actionDescription ?? null,
      blockingType: row?.blockingType ?? null,
      remark: row?.remark ?? null,
      state: row?.state ?? null
      // code: row?.code ?? null,
      // name: row?.name ?? null,
      // sortNumber: row?.sortNumber ?? null,
      // isEnable: row?.isEnable !== undefined ? row.isEnable : true,
      // carId: row?.carId ?? null,
      // actionType: row?.actionType ?? null,
      // actionDescription: row?.actionDescription ?? null,
      // blockingType: row?.blockingType ?? null,
      // remark: row?.remark ?? null
    };
  }

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

  function handleAdd() {
    addDialog({
      title: "添加",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: {
        formInline: handleRow()
      },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline as FormItemProps;
        formData.state = "Created";
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await addOrUpdate("", formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
    });
  }

  function handleUpdate(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "修改",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow(row) },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline as FormItemProps;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await addOrUpdate({ keyValue: row?.id }, formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
    });
  }
  async function handleParameters(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "参数",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: row },
      contentRenderer: () => h(parametersFrom, { ref: formRef })
    });
  }

  async function handleForceDelete(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await ForceDelete(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
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
        await deletes(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleEnable(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await enable(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleDisable(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await disable(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleDeleteM3(rows = selection.value, cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteM3(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleDeleteM1(rows = selection.value, cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteM1(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleDeleteW1(rows = selection.value, cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteW1(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleDeleteD1(rows = selection.value, cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteD1(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleDeleteAll(rows = selection.value, cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteAll(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
  // Release
  async function handleRelease(rows = selection.value, cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await Release(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function TypeListToSelect() {
    // const { data } = await GetTypeListToSelect();
    // carTypeList.value = [...data];
  }
  async function getStateList() {
    let res = await GetDictItemInLocal("WorkState");
    stateList.value = res;
  }
  async function getTaskTemplateActionBlockingTypeDataList() {
    let res = await GetDictItemInLocal("TaskTemplateActionBlockingType");
    TaskTemplateActionBlockingTypeDataList.value = res;
  }

  async function getListToSelect(value: String) {
    let { data } = await GetListToSelectByCarId(value);
    operationTypeList.value = [...data];
  }
  async function getTaskInstanceTypeList() {
    let res = await GetDictItemInLocal("CarInstantActionState");
    TaskInstanceActionStateList.value = res;
  }

  onMounted(async () => {
    getTaskInstanceTypeList();
    handleSearch();
    TypeListToSelect();
    getStateList();
    getCarList();
    getTaskTemplateActionBlockingTypeDataList();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    carList,
    pagination,
    stateList,
    operationTypeList,
    TaskInstanceActionStateList,
    shortcuts,
    actionTypeData,
    TaskTemplateActionBlockingTypeDataList,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDetail,
    handleAdd,
    handleUpdate,
    handleDelete,
    handleEnable,
    handleDisable,
    getListToSelect,
    handleForceDelete,
    handleDeleteAll,
    handleDeleteD1,
    handleDeleteW1,
    handleDeleteM1,
    handleDeleteM3,
    handleRelease,
    handleParameters
  };
}
