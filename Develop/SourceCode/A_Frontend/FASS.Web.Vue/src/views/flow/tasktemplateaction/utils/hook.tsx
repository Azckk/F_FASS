import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  GetListToSelectByCarTypeCode,
  putTaskTemplateAction,
  DeleteTaskTemplateAction,
  getTaskTemplateActionPage
} from "@/api/flow/tasktemplate";
import { type FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { useMyI18n } from "@/plugins/i18n";
import tasktemplateparameter from "../../tasktemplateparameter/index.vue";
export function useHook(
  tableRef?: Ref | undefined,
  keyValue?: Ref | undefined
) {
  const query = reactive({
    taskTemplateProcessCode: ""
  });
  const { t } = useMyI18n();
  const formRef = ref();
  const TemplateActionBlockingTypeList = ref([]);
  const dataList = ref([]);
  const loading = ref(true);
  const ListByCarTypeCode = ref([]);
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
      headerRenderer: () => t("buttons.subtasks"),
      label: "buttons.subtasks",
      prop: "taskTemplateProcessCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.actionType"),
      label: "table.actionType",
      prop: "actionType",
      sortable: true,
      formatter: ({ actionType }) => {
        let dictResult = ListByCarTypeCode.value.filter(
          x => x.code === actionType
        );
        if (dictResult && dictResult[0]) {
          return dictResult[0].name;
        }
        return "未知";
      }
    },
    /*{
      label: "路线", prop: "edgeCode", sortable: true,
       formatter: ({ edgeCode }) => { 
          if (edgeCode) {
              return  edgeCode;
          }
        return "-";
      }
    },*/
    {
      headerRenderer: () => t("table.blockageType"),
      label: "table.blockageType",
      prop: "blockingType",
      sortable: true,
      formatter: ({ blockingType }) => {
        let dictResult = TemplateActionBlockingTypeList.value.filter(
          x => x.code === blockingType
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
    const { data } = await getTaskTemplateActionPage(
      keyValue.value,
      pageParam.value
    );
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
    console.log(" keyValue.value ", keyValue.value);
    return {
      actionType: row?.actionType ?? null,
      actionDescription: row?.actionDescription ?? null,
      blockingType: row?.blockingType ?? null,
      sortNumber: row?.sortNumber ?? 0,
      taskTemplateProcessId: keyValue.value ?? null
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
            await putTaskTemplateAction("", formData);
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
            await putTaskTemplateAction(row?.id, formData);
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
        await DeleteTaskTemplateAction(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleBtnParameter(
    rows = selection.value,
    cancel = undefined
  ) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "参数",
      props: { actionId: row?.id ?? null },
      width: "80%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => tasktemplateparameter
    });
  }

  async function GetListByCarTypeCode() {
    // window.carTypeCode;
    const { data } = await GetListToSelectByCarTypeCode(window.carTypeCode);
    // Data/CarAction/GetListToSelectByCarTypeCode?carTypeCode=CarForklift&_=1714032697623
    ListByCarTypeCode.value = [...data];
  }

  async function GetTemplateActionBlockingTypeList() {
    try {
      const data = await GetDictItemInLocal("TaskTemplateActionBlockingType");
      TemplateActionBlockingTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  onMounted(async () => {
    // 需要在这里获取数据
    GetListByCarTypeCode();
    handleSearch();
    GetTemplateActionBlockingTypeList();
  });

  return {
    query,
    loading,
    columns,
    TemplateActionBlockingTypeList,
    ListByCarTypeCode,
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
    handleBtnParameter
  };
}
