import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  getPage,
  addOrUpdate,
  deletes,
  enable,
  disable,
  GetCarListToSelect
} from "@/api/data/cartraffic";
// import { getList } from "@/api/account/org";
import { type FormItemProps } from "../utils/types";
// import { usePublicHooks } from "../../hooks";
import editForm from "../form.vue";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  const switchLoadMap = ref({});
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    fromCarId: "",
    createAt: "",
    endAt: "",
    toCarId: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const carTypeList = ref([]);
  const carList = ref([]);
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
    { type: "selection", align: "left", sortable: true },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.controlOfVehicles"),
      label: "table.controlOfVehicles",
      prop: "remark",
      sortable: true,
      formatter: ({ remark }) => {
        const parts = remark.split(",")[0];
        if (parts) return parts;
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.regulatedvehicle"),
      label: "table.regulatedvehicle",
      prop: "remark",
      sortable: true,
      formatter: ({ remark }) => {
        const parts = remark.split(",")[1];
        if (parts) return parts;
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.regulatoryStatistics"),
      label: "table.regulatoryStatistics",
      prop: "count",
      sortable: true
    },
    {
      headerRenderer: () => t("table.bidirectional"),
      label: "table.bidirectional",
      prop: "isMutual",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.isMutual ? "green" : "red"} class="no-inherit">
          {row.isMutual ? (
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
    // {
    //   label: "是否双向", prop: "isFinish",sortable: true,
    //   cellRenderer: ({ row, props }) => (
    //     <el-icon color={row.isFinish ? "green" : "red"} class="no-inherit">
    //       {row.isFinish ?    <div style="display: flex; align-items: center">
    //       <iconify-icon-online icon="ep:check" />
    //       <span style="margin-left: 10px">{row.date}</span>
    //     </div>
    //       : <div style="display: flex; align-items: center">
    //       <iconify-icon-online icon="ep:close" />
    //       <span style="margin-left: 10px">{row.date}</span>
    //     </div>}
    //     </el-icon>
    //   )
    // },
    // IsFinish
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      minWidth: 120,
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
    handleSearch();
  }

  function handleRow(row = undefined) {
    return {
      fromCarId: row?.fromCarId ?? "",
      toCarId: row?.toCarId ?? "",
      count: row?.count ?? null,
      isMutual: row?.isMutual !== undefined ? row.isMutual : true,
      IsFinish: row?.IsFinish !== undefined ? row.IsFinish : true
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

  async function TypeListToSelect() {
    const { data } = await GetCarListToSelect();
    carList.value = [...data];
  }

  onMounted(async () => {
    handleSearch();
    TypeListToSelect();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    carList,
    dataList,
    carTypeList,
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
    handleDisable
  };
}
