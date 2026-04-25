import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  TaskTemplateListToSelect,
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
  ForceDelete
} from "@/api/flow/taskinstance";
import { GetListToSelect } from "@/api/base/edge";
import type { FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import process from "../../taskinstanceprocess/index.vue";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef?: Ref | undefined) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: "",
    state: "",
    carId: "",
    time: [dayjs().subtract(1, "week").toDate(), new Date()]
  });
  const formRef = ref();
  // TaskInstanceType
  const TaskInstanceTypeList = ref([]); // 任务实例类型列表
  const TaskInstanceStateList = ref([]); // 任务实例类型列表
  const CarInstantActionStateList = ref([]);
  const nodeList = ref(); //站点
  const edgeList = ref(); //站点
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
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "name",
      sortable: true
    },
    {
      headerRenderer: () => t("table.taskTemplates"),
      label: "table.taskTemplates",
      prop: "taskTemplateCode",
      sortable: true,
      formatter: ({ taskTemplateCode }) => {
        // if (taskTemplateCode) {
        //   return taskTemplateCode;
        // }
        // return "--未指定--";
        let dictResult = TaskTemplateList.value.filter(
          x => x.code === taskTemplateCode
        );
        if (dictResult && dictResult[0]) {
          if (!isEnglish.test(t("table.type"))) {
            return dictResult[0].name;
          } else {
            return dictResult[0].code;
          }
        }
        return "--未指定--";
      }
    },
    {
      headerRenderer: () => t("table.ownVehicle"),
      label: "table.ownVehicle",
      prop: "remark",
      sortable: true,
      formatter: ({ remark }) => {
        // const parts = name.split(",")[0];
        if (remark) return remark;
        return "--无--";
      }
    },
    {
      headerRenderer: () => t("table.carType"),
      label: "table.carType",
      prop: "carTypeName",
      sortable: true,
      formatter: ({ carTypeCode, carTypeName }) => {
        if (!isEnglish.test(t("table.carType"))) {
          return carTypeName;
        } else {
          return carTypeCode;
        }
      }
    },

    {
      headerRenderer: () => t("table.type"),
      label: "table.type",
      prop: "type",
      sortable: true,
      formatter: ({ type }) => {
        let dictResult = TaskInstanceTypeList.value.filter(
          x => x.code === type
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
      headerRenderer: () => t("table.priority"),
      label: "table.priority",
      prop: "priority",
      sortable: true
    },
    {
      headerRenderer: () => t("table.status"),
      label: "table.status",
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
        order: [{ field: "createAt", sequence: "DESC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
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

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await getPage(pageParam.value);
    dataList.value = data.rows;
    pagination.total = data.total;
    loading.value = false;
  }
  async function taskTemplateIdChange(value: any) {
    const { data } = await GetListToSelectByTaskTemplateId();
    ListToSelectByTaskTemplateId.value = [...data];
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
      /*{"taskTemplateId": "Double",
    "carId": "3a11d989-6151-a9bc-b7c6-2f30e31cec9a",
    "carTypeId": "3a11d2ed-cd4f-d3a8-a09a-9c21e8de5851",
    "code": "1",
    "name": "001",
    "type": "Normal",
    "priority": 10,
    "nodes": ["06a372e0-7ff6-4f32-b5a7-b0e60cbc332f","#",],
    "edges": ["#","-"]}*/
      code: row?.code ?? null,
      name: row?.name ?? null,
      type: row?.type ?? TaskInstanceTypeList.value[0].code,
      carTypeId: row?.carTypeId ?? carTypeList.value[0].id,
      priority: row?.priority ?? 0,
      taskTemplateId: row?.taskTemplateId ?? "",
      carId: row?.carId ?? null,
      // isLoop: row?.isLoop !== undefined ? row.isLoop : false,
      nodes: row?.nodes ?? [],
      edges: row?.edges ?? [],
      state: row?.state ?? null
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
      props: { formInline: handleRow() },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline as FormItemProps;
        if (formData.carId === "") formData.carId = null;
        if (formData.carTypeId === "") formData.carTypeId = null;
        formData.state = "Created";
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await addOrUpdate({ keyValue: "" }, formData);
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
        await Release(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
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

  async function handleBtnProcess(rows = selection.value, cancel = undefined) {
    // function handleDetail(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "子任务",
      props: { roleId: row?.id ?? null },
      width: "80%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => {
        window.carTypeCode = row.carTypeCode;
        return process;
      }
    });
  }

  async function TypeListToSelect() {
    const { data } = await GetTypeListToSelect();
    carTypeList.value = [...data];
  }
  // 模板选择
  async function GetTaskTemplateListToSelect() {
    const { data } = await TaskTemplateListToSelect();
    TaskTemplateList.value = [...data];
    // console.log("   TaskTemplateList.value is" ,  TaskTemplateList.value   )
  }

  async function nodeListToSelect() {
    const { data } = await getNodeList();
    nodeList.value = [...data];
  }
  // edgeList

  async function edgeListToSelect() {
    const { data } = await GetListToSelect();
    edgeList.value = [...data];
  }

  async function GetTaskInstanceTypeList() {
    try {
      const data = await GetDictItemInLocal("TaskInstanceType");
      TaskInstanceTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function GetCarInstantActionState() {
    try {
      const data = await GetDictItemInLocal("CarInstantActionState");
      CarInstantActionStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  // CarInstantActionState

  async function GetTaskInstanceStateList() {
    try {
      const data = await GetDictItemInLocal("TaskInstanceState");
      TaskInstanceStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  onMounted(async () => {
    GetCarInstantActionState();
    // 需要在这里获取数据
    TypeListToSelect();
    nodeListToSelect();
    edgeListToSelect();
    handleSearch();
    GetTaskInstanceTypeList();
    GetTaskTemplateListToSelect();
    taskTemplateIdChange(null);
    GetTaskInstanceStateList();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    carTypeList,
    TaskTemplateList,
    TaskInstanceTypeList,
    ListToSelectByTaskTemplateId,
    CarInstantActionStateList,
    nodeList,
    edgeList,
    dataList,
    pagination,
    TaskInstanceStateList,
    handleRelease,
    handlePause,
    handleCancel,
    handleResume,
    taskTemplateIdChange,
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
    handleBtnProcess,
    handleDeleteAll,
    handleDeleteD1,
    handleDeleteW1,
    handleDeleteM1,
    handleDeleteM3,
    handleForceDelete,
    shortcuts
  };
}
