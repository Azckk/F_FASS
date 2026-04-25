import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  GetListToSelectByTaskTemplateId,
  GetTypeListToSelect,
  getPage,
  getNodeList,
  addOrUpdate,
  deletes,
  Release,
  Pause,
  Resume,
  Cancel,
  DeleteM3,
  DeleteM1,
  DeleteW1,
  DeleteD1,
  DeleteAll,
  GetNodeListToSelect,
  GetTemplateListToSelect,
  Resend,
  ForceDelete
} from "@/api/task/taskrecord";
import type { FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
// import process from "../../taskinstanceprocess/index.vue";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef?: Ref | undefined) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: "",
    carName: "",
    taskStatus: "",
    carId: "",
    state: "",
    time: [dayjs().subtract(1, "week").toDate(), new Date()]
  });
  const formRef = ref();
  // TaskInstanceType
  const TaskInstanceTypeList = ref([]); // 任务实例类型列表
  const TaskRecordStateList = ref([]); // 任务实例类型列表
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
      headerRenderer: () => t("table.type"),
      label: "table.type",
      prop: "type",
      sortable: true,
      formatter: ({ type }) => {
        let dictResult = taskTypeList.value.filter(x => x.code === type);
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
      headerRenderer: () => t("table.taskTemplates"),
      label: "table.taskTemplates",
      prop: "taskTemplateId",
      sortable: true,
      formatter: ({ taskTemplateId }) => {
        if (isEnglish.test(t("table.taskTemplates"))) {
          return taskTemplateId;
        } else {
          let dictResult = TaskTemplateList.value.filter(
            x => x.id === taskTemplateId
          );
          if (dictResult && dictResult[0]) {
            return dictResult[0].name;
          }
          return taskTemplateId;
        }
      }
    },
    // {
    //   headerRenderer: () => t("menus.Flow-LogisticsRoute"),
    //   label: "menus.Flow-LogisticsRoute",
    //   prop: "name",
    //   sortable: true
    // },
    {
      headerRenderer: () => t("table.taskStartingPoint"),
      label: "table.taskStartingPoint",
      prop: "srcNodeCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.taskEndingPoint"),
      label: "table.taskEndingPoint",
      prop: "destNodeCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.priority"),
      label: "table.priority",
      prop: "priority",
      sortable: true
    },
    {
      headerRenderer: () => t("table.ownVehicle"),
      label: "table.ownVehicle",
      prop: "carId",
      sortable: true,
      formatter: ({ carId }) => {
        if (carId) {
          return getCarName(carId);
        }
        return "--无--";
      }
    },
    {
      headerRenderer: () => t("table.duplicate"),
      label: "table.duplicate",
      prop: "isLoop",
      sortable: true,
      formatter: ({ isLoop }) => {
        return isLoop ? t("table.yes") : t("table.no");
      }
    },
    {
      headerRenderer: () => t("table.taskStatus"),
      label: "table.taskStatus",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        let dictResult = TaskRecordStateList.value.filter(
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
      headerRenderer: () => t("table.startTime"),
      label: "table.startTime",
      prop: "startTime",
      sortable: true,
      minWidth: 120,
      formatter: ({ startTime }) =>
        startTime ? dayjs(startTime).format("YYYY-MM-DD HH:mm:ss") : ""
    },
    {
      headerRenderer: () => t("table.endTime"),
      label: "table.endTime",
      prop: "endTime",
      sortable: true,
      minWidth: 120,
      formatter: ({ endTime }) =>
        endTime ? dayjs(endTime).format("YYYY-MM-DD HH:mm:ss") : ""
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      sortable: true,
      minWidth: 120,
      formatter: ({ createAt }) => dayjs(createAt).format("YYYY-MM-DD HH:mm:ss")
    }
    // { label: "操作", slot: "operation", minWidth: 120 }
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

  // function handlePageParam() {
  //   return {
  //     pageParam: JSON.stringify({
  //       where: Object.keys(query)
  //         .filter(e => query[e])
  //         .map(e => ({
  //           logic: "And",
  //           field: e,
  //           operator: "Contains",
  //           value: query[e]
  //         })),
  //       order: [{ field: "createAt", sequence: "DESC" }],
  //       number: pagination.currentPage,
  //       size: pagination.pageSize
  //     })
  //   };
  // }
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
  async function taskTemplateIdChange(value: any) {
    if (value !== null && value !== undefined) {
      tableRef.value.resetFields(["carId"]);
      // tableRef.value.resetFields({ "carId":"" });
    }
    const { data } = await GetListToSelectByTaskTemplateId(value);
    ListToSelectByTaskTemplateId.value = [...data];
    // console.log("data is", data);
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
      code: row?.code ?? dayjs().format("YYYYMMDDHHmmssSSS"),
      name: row?.name ?? null,
      type: row?.type ?? "",
      carTypeId: row?.carTypeId ?? carTypeList.value[0].id,
      priority: row?.priority ?? 0,
      taskTemplateId: row?.taskTemplateId ?? "",
      carId: row?.carId ?? "",
      nodes: row?.nodes ?? [],
      edges: row?.edges ?? [],
      // id: "string",
      sortNumber: row?.sortNumber ?? 0,
      isEnable: true,
      isLoop: row?.isLoop ?? false,
      isDelete: false,
      extend: row?.extend ?? "",
      srcNodeId: row?.srcNodeId ?? "",
      destNodeId: row?.destNodeId ?? "",
      srcNodeCode: row?.srcNodeCode ?? "",
      destNodeCode: row?.destNodeCode ?? "",
      srcAreaId: row?.srcAreaId ?? "",
      destAreaId: row?.destAreaId ?? "",
      condition: row?.condition ?? ""
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
            if (formData.carId === "") formData.carId = null;
            if (formData.carTypeId === "") formData.carTypeId = null;
            await addOrUpdate({ keyValue: row?.id }, formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
    });
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
  // ForceDelete
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

  async function handleRelease(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        let { data } = await Release(rows.map(e => e.id));
        message(data, { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handlePause(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await Pause(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleResume(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await Resume(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
  function handleResend(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        let { data } = await Resend(rows.map(e => e.id));
        message(data, { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
  async function handleCancel(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await Cancel(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
  // Release, Pause,
  // Resume,Cancel

  async function TypeListToSelect() {
    const { data } = await GetTypeListToSelect();
    carTypeList.value = [...data];
  }
  // 模板选择
  async function GetTaskTemplateListToSelect() {
    // const { data } = await TaskTemplateListToSelect();
    // TaskTemplateList.value = [...data];
  }

  async function nodeListToSelect() {
    const { data } = await getNodeList();
    nodeList.value = [...data];
  }

  async function GetTaskInstanceTypeList() {
    try {
      const data = await GetDictItemInLocal("TaskInstanceType");
      TaskInstanceTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function GetTaskRecordStateList() {
    try {
      const data = await GetDictItemInLocal("TaskRecordState");
      TaskRecordStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function GetTaskInstanceStateList() {
    try {
      const data = await GetDictItemInLocal("TaskInstanceState");
      TaskInstanceStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }
  // 任务类型
  const taskTypeList = ref([]);
  async function GetTaskType() {
    try {
      let data = await GetDictItemInLocal("TaskType");
      // console.log("data is" , data);
      data = data.filter(item => item.code !== "LogisticsRoute");
      taskTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }
  const taskNodeList = ref([]);
  async function getListToSelect() {
    pageParam.value = handlePageParam();
    const { data } = await GetTemplateListToSelect();
    TaskTemplateList.value = data;
  }
  async function getTaskNodeSelect() {
    pageParam.value = handlePageParam();
    const { data } = await GetNodeListToSelect(pageParam.value);
    taskNodeList.value = data;
  }
  function srcNodeIdChange(value) {
    const selectedItem = taskNodeList.value.find(item => item.id === value);
    if (selectedItem) {
      // const label = `${selectedItem.name} (${selectedItem.code})`;
      return selectedItem.code;
    }
  }
  function destNodeIdChange(value) {
    const selectedItem = taskNodeList.value.find(item => item.id === value);
    if (selectedItem) {
      const label = `${selectedItem.name} (${selectedItem.code})`;
      return `${selectedItem.name} (${selectedItem.code})`;
    }
  }
  function getCarName(value) {
    const selectedItem = ListToSelectByTaskTemplateId.value.find(
      item => item.id === value
    );
    if (selectedItem) {
      return selectedItem.name;
    }
  }

  function handleAdd(rows = selection.value) {
    addDialog({
      title: "添加",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow() },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options }) => {
        const formData = options.props.formInline as FormItemProps;
        // 添加addNode[node]的值到formData
        formData.srcNodeCode = srcNodeIdChange(formData.srcNodeId);
        formData.destNodeCode = srcNodeIdChange(formData.destNodeId);
        // formData.code = new Date().getTime().toString();
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            if (formData.carId === "") formData.carId = null;
            if (formData.carTypeId === "") formData.carTypeId = null;
            await addOrUpdate({ keyValue: "" }, formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
    });
  }

  onMounted(async () => {
    // 需要在这里获取数据
    TypeListToSelect();
    GetTaskRecordStateList();
    nodeListToSelect();
    handleSearch();
    GetTaskInstanceTypeList();
    GetTaskTemplateListToSelect();
    taskTemplateIdChange(null);
    GetTaskInstanceStateList();
    // const { data } = await getList();
    getListToSelect();
    getTaskNodeSelect();
    GetTaskType();
  });

  return {
    query,
    loading,
    shortcuts,
    columns,
    carTypeList,
    taskNodeList,
    TaskTemplateList,
    taskTypeList,
    TaskInstanceTypeList,
    ListToSelectByTaskTemplateId,
    TaskRecordStateList,
    nodeList,
    dataList,
    pagination,
    handleRelease,
    handlePause,
    handleCancel,
    handleResume,
    taskTemplateIdChange,
    srcNodeIdChange,
    destNodeIdChange,
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
    handleResend,
    handleDeleteAll,
    handleDeleteD1,
    handleDeleteW1,
    handleDeleteM1,
    handleDeleteM3,
    handleForceDelete
  };
}
