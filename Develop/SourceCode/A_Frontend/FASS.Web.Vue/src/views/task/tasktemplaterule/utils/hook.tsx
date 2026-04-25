import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  addOrUpdate,
  getPage,
  deletes,
  GetTypeListToSelect
} from "@/api/task/tasktemplaterule";
import { usePublicHooks } from "../../hooks";
import { type FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";

import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef?: Ref | undefined, keyValue?: string) {
  const { switchStyle } = usePublicHooks();
  const switchLoadMap = ref({});

  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const TaskTemplateTypeList = ref([]);
  const nodeList = ref(); //站点
  const loading = ref(true);
  const carTypeList = ref([]);
  const dataList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const switchLoad = ref({});
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
      type: "index",
      width: 60
    },
    {
      headerRenderer: () => t("table.commonRules"),
      label: "table.commonRules",
      prop: "code", // name没有匹配字段
      sortable: true
    },
    {
      headerRenderer: () => t("table.value"),
      label: "table.value",
      prop: "value",
      sortable: true
    },
    {
      headerRenderer: () => t("table.description"),
      label: "table.description",
      prop: "remark",
      sortable: true
    },
    {
      headerRenderer: () => t("table.isEnable"),
      label: "table.isEnable",
      prop: "isEnable",
      sortable: true,
      cellRenderer: scope => (
        <el-switch
          size={scope.props.size === "small" ? "small" : "default"}
          loading={switchLoadMap.value[scope.index]?.loading}
          v-model={scope.row.isEnable}
          active-value={true}
          inactive-value={false}
          active-text="是"
          inactive-text="否"
          inline-prompt
          style={switchStyle.value}
          onChange={async () => {
            await addOrUpdate({ keyValue: scope.row?.id }, scope.row);
          }}
        />
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
  let taskTemplateId = "";
  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    let templateId;
    if (keyValue) {
      templateId = keyValue["value"];
    } else {
      templateId = "";
    }
    const { data } = await getPage(templateId, pageParam.value);
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
      code: row?.code ?? null,
      isEnable: row?.isEnable !== undefined ? row.isEnable : true,
      isDelete: row?.isDelete ?? false,

      sortNumber: row?.sortNumber ?? 0,
      remark: row?.remark ?? null,
      extend: row?.extend ?? null,
      taskTemplateId: row?.taskTemplateId ?? null, //????
      description: row?.description ?? null,
      value: row?.value ?? null,

      carTypeId: row?.carTypeId ?? null,
      name: row?.name ?? null,
      type: row?.type ?? null,
      priority: row?.priority ?? 0,
      carTypeCode: row?.carTypeCode ?? null,
      carTypeName: row?.carTypeName ?? null
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

  function handleAdd(id) {
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
        let formData = options.props.formInline as FormItemProps;
        formData.taskTemplateId = id;
        // formData.code = id;
        console.log(" formData ", formData);
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await addOrUpdate({}, formData);
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
    console.log(" row ", row);
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
            console.log(" row ", row.id);
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

  async function TypeListToSelect() {
    const { data } = await GetTypeListToSelect();
    carTypeList.value = [...data];
  }

  async function GetTaskTemplateTypeList() {
    try {
      const data = await GetDictItemInLocal("TaskTemplateType");
      TaskTemplateTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  function handleEnable(rows = selection.value, cancel = undefined) {
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

  function handleDisable(rows = selection.value, cancel = undefined) {
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

  onMounted(async () => {
    // 需要在这里获取数据
    TypeListToSelect();
    handleSearch();
    GetTaskTemplateTypeList();
  });

  return {
    query,
    loading,
    columns,
    carTypeList,
    TaskTemplateTypeList,
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
    handleDelete
  };
}
