import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  getTaskInstanceActionPage,
  GetListToSelectByCarTypeCode,
  addOrUpdateTaskInstanceAction,
  TaskInstanceActionDelete
} from "@/api/flow/taskinstance";
import type { FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import taskinstanceparameter from "../../taskinstanceparameter/index.vue";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(
  tableRef?: Ref | undefined,
  keyValue?: Ref | undefined
) {
  const query = reactive({
    code: "",
    name: ""
  });
  const { t } = useMyI18n();
  const formRef = ref();
  const TemplateActionBlockingTypeList = ref([]);
  const StateList = ref([]);
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
      prop: "taskInstanceProcessCode",
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
      headerRenderer: () => t("table.state"),
      label: "table.state",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        let dictResult = StateList.value.filter(x => x.code === state);
        if (dictResult && dictResult[0]) {
          return dictResult[0].name;
        }
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      minWidth: 120,
      sortable: true,
      formatter: ({ createAt }) =>
        createAt ? dayjs(createAt).format("YYYY-MM-DD HH:mm:ss") : ""
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
    const { data } = await getTaskInstanceActionPage(
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
      taskInstanceProcessId: keyValue.value ?? null,
      state: row?.state ?? null
    };
  }

  /**
   * // {
//     "actionType": "StartPause",
//     "actionDescription": "111111111111",
//     "blockingType": "NONE",
//     "sortNumber": 4,
//     "taskInstanceProcessId": "3a123457-3f0b-4c6e-b307-d64ad2d8d124"
// }
   */

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
        formData.state = "Created";
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await addOrUpdateTaskInstanceAction("", formData);
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
            await addOrUpdateTaskInstanceAction(row?.id, formData);
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
        await TaskInstanceActionDelete(rows.map(e => e.id));
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
      contentRenderer: () => taskinstanceparameter
    });
  }

  async function GetListByCarTypeCode() {
    // window.carTypeCode;
    const { data } = await GetListToSelectByCarTypeCode(window.carTypeCode);
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

  async function GetStateList() {
    try {
      const data = await GetDictItemInLocal("TaskInstanceActionState");
      StateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  onMounted(async () => {
    // 需要在这里获取数据
    GetListByCarTypeCode();
    handleSearch();
    GetTemplateActionBlockingTypeList();
    GetStateList();
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
