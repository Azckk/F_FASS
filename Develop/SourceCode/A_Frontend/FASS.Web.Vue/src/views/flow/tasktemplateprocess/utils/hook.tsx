import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  GetTypeListToSelect,
  getNodeList,
  TaskTemplateProcessAddOrUpdate,
  processDeletes,
  getProcessPage
} from "@/api/flow/tasktemplate";
import { type FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import taskTemplateAction from "../../tasktemplateaction/index.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(
  tableRef?: Ref | undefined,
  keyValue?: Ref | undefined
) {
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const { t } = useMyI18n();
  const TaskTemplateProcessTypeList = ref([]);
  const nodeList = ref(); //站点
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
      headerRenderer: () => t("table.taskTemplates"),
      label: "table.taskTemplates",
      prop: "taskTemplateCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.site"),
      label: "table.site",
      prop: "nodeCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.route"),
      label: "table.route",
      prop: "edgeCode",
      sortable: true,
      formatter: ({ edgeCode }) => {
        if (edgeCode) {
          return edgeCode;
        }
        return "-";
      }
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
      headerRenderer: () => t("table.type"),
      label: "table.type",
      prop: "type",
      sortable: true,
      formatter: ({ type }) => {
        let dictResult = TaskTemplateProcessTypeList.value.filter(
          x => x.code === type
        );
        if (dictResult && dictResult[0]) {
          return dictResult[0].name;
        }
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.sort"),
      label: "table.sort",
      prop: "sortNumber",
      sortable: true
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      minWidth: 120,
      sortable: true,
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
        order: [{ field: "sortNumber", sequence: "asc" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await getProcessPage(keyValue.value, pageParam.value);
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
      nodeId: row?.nodeId ?? null,
      code: row?.code ?? null,
      name: row?.name ?? null,
      type: row?.type ?? null,
      sortNumber: row?.sortNumber ?? 0,
      taskTemplateId: keyValue.value ?? null
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
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            if (formData.nodeId === "") formData.nodeId = null;
            await TaskTemplateProcessAddOrUpdate("", formData);
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
            if (formData.nodeId === "") formData.nodeId = null;
            await TaskTemplateProcessAddOrUpdate(row?.id, formData);
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
        await processDeletes(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleBtnAction(rows = selection.value, cancel = undefined) {
    // taskTemplateAction
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "动作",
      props: { actionId: row?.id ?? null },
      width: "80%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => taskTemplateAction
    });
  }

  async function nodeListToSelect() {
    const { data } = await getNodeList();
    nodeList.value = [...data];
  }

  async function GetTaskTemplateTypeList() {
    try {
      const data = await GetDictItemInLocal("TaskTemplateProcessType");
      TaskTemplateProcessTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  onMounted(async () => {
    // 需要在这里获取数据
    nodeListToSelect();
    handleSearch();
    GetTaskTemplateTypeList();
  });

  return {
    query,
    loading,
    columns,
    TaskTemplateProcessTypeList,
    nodeList,
    dataList,
    pagination,
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
    handleBtnAction
  };
}
