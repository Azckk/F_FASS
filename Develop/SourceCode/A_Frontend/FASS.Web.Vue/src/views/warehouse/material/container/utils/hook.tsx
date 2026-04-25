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
  getNodeList,
  getWarehouseAreaList,
  ContainerGetPage,
  ContainerDelete,
  ContainerAdd
} from "@/api/warehouse/material";
// import { getList } from "@/api/account/org";
import type { FormItemProps } from "../../utils/types";
// import { usePublicHooks } from "../../hooks";
import { GetDictItemInLocal } from "@/utils/auth";
import editForm from "../../../container/form.vue";
// import userSelect from "../../../container/select.vue";
// import userSelect from "../../../visualizationtask/container/index.vue";
import userSelect from "../../../visualizationtask/container/indexs.vue";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(
  tableRef?: Ref | undefined,
  keyValue?: Ref | undefined
) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const ContainerStateList = ref([]);
  const selectRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const ContainerTypeList = ref([]);
  const WarehouseAreaList = ref([]);
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
      headerRenderer: () => t("table.region"),
      label: "table.region",
      prop: "areaId",
      sortable: true,
      formatter: ({ areaId }) => {
        let dictResult = WarehouseAreaList.value.filter(x => x.id === areaId);
        if (dictResult && dictResult[0]) {
          return dictResult[0].name;
        }
        return "未知";
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
        let dictResult = ContainerTypeList.value.filter(x => x.code === type);
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
      headerRenderer: () => t("table.status"),
      label: "table.status",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        let dictResult = ContainerStateList.value.filter(x => x.code === state);
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
      headerRenderer: () => t("table.isLock"),
      label: "table.isLock",
      prop: "isLock",
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
      headerRenderer: () => t("table.barcode"),
      label: "table.barcode",
      prop: "barcode"
    },
    {
      headerRenderer: () => t("table.length"),
      label: "table.length",
      prop: "length",
      sortable: true
    },
    {
      headerRenderer: () => t("table.width"),
      label: "table.width",
      prop: "width",
      sortable: true
    },
    {
      headerRenderer: () => t("table.height"),
      label: "table.height",
      prop: "height",
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
        order: [{ field: "createAt", sequence: "DESC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await ContainerGetPage(keyValue.value, pageParam.value);
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
      areaId: row?.areaId ?? "",
      code: row?.code ?? "",
      name: row?.name ?? "",
      type: row?.type ?? "Default",
      state: row?.state ?? "",
      isLock: row?.isLock !== undefined ? row.isLock : true,
      barcode: row?.barcode ?? "",
      length: row?.length ?? "",
      width: row?.width ?? "",
      height: row?.height ?? ""
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
      width: "80%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow() },
      contentRenderer: () => h(userSelect, { ref: selectRef }),
      beforeSure: async (done, { options, index }) => {
        const rows = selectRef.value.tableRef.getTableRef().getSelectionRows();
        await ContainerAdd(keyValue.value, rows);
        message("操作成功！", { type: "success" });
        handleSearch();
        done();
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
        await ContainerDelete(keyValue.value, rows);
        // await deletes(rows.map(e => e.id));
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

  async function GetStorageTypeList() {
    try {
      const data = await GetDictItemInLocal("ContainerType");
      ContainerTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function GetStorageStateList() {
    try {
      const data = await GetDictItemInLocal("ContainerState");
      ContainerStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function GetWarehouseAreaToSelct() {
    try {
      const { data } = await getWarehouseAreaList();
      WarehouseAreaList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  onMounted(async () => {
    GetStorageTypeList();
    GetStorageStateList();
    handleSearch();
    // GetNodeListToSelct()
    GetWarehouseAreaToSelct();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    WarehouseAreaList,
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
