import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { getPage, addOrUpdate, deletes } from "@/api/data/trafficrules";
import { type FormItemProps } from "../utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
import editForm from "../form.vue";
//@ts-ignore
import { usePublicHooks } from "../../hooks";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const switchLoadMap = ref({});
  const loading = ref(true);
  const nodeList = ref([]);
  const carList = ref([]);
  const dataList = ref([]);
  const AvoidStateList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
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
      headerRenderer: () => t("table.index"),
      label: "table.index",
      type: "index",
      minWidth: 40
    },
    {
      headerRenderer: () => t("table.ruleConfiguration"),
      label: "table.ruleConfiguration",
      prop: "name",
      sortable: true
    },
    {
      headerRenderer: () => t("table.ruleDescription"),
      label: "table.ruleDescription",
      prop: "description",
      sortable: true
    },
    {
      headerRenderer: () => t("table.ruleParameters"),
      label: "table.ruleParameters",
      prop: "value",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <span style={{ color: "#409EFF" }}>{row.value}</span>
      )
    },
    {
      headerRenderer: () => t("table.isEnable"),
      label: "table.isEnable",
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
    // {
    //   headerRenderer: () => t("table.operation"),
    //   label: "table.operation",
    //   slot: "operation",
    //   minWidth: 120,
    //   sortable: true
    // }
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
          .map(e => {
            if (e === "endAt") {
              return {
                logic: "And",
                field: "createAt",
                operator: "LessEqual",
                value: query[e]
              };
            } else if (e === "createAt") {
              return {
                logic: "And",
                field: "createAt",
                operator: "GreaterEqual",
                value: query[e]
              };
            } else {
              return {
                logic: "And",
                field: e,
                operator: "Contains",
                value: query[e]
              };
            }
          }),
        order: [{ field: "createAt", sequence: "desc" }],
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
    query.name = "";
    handleSearch();
  }

  function handleRow(row = undefined) {
    return {
      name: row?.name ?? "",
      description: row?.description ?? "",
      value: row?.value ?? "",
      isEnable: row?.isEnable ?? true
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
            // await addOrUpdate({keyValue:row?.id}, formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
    });
  }

  async function GetAvoidStateList() {
    try {
      const data = await GetDictItemInLocal("AvoidState");
      AvoidStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  function handleAdd(rows = selection.value) {
    const row = rows[0];
    addDialog({
      title: "添加",
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
        await deletes(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
  function handleDisable() {}
  function handleEdit(rows = selection.value) {
    if (rows.length === 0) {
      message("请选择要操作的数据！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "添加",
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
            console.log(row?.id, formData);
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
  onMounted(async () => {
    handleSearch();
    GetAvoidStateList();
  });

  return {
    query,
    loading,
    columns,
    AvoidStateList,
    carList,
    dataList,
    nodeList,
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
    handleEnable,
    handleDisable,
    handleEdit
  };
}
