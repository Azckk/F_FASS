import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { GetTypeListToSelect } from "@/api/data/car";
import { addDialog } from "@/components/ReDialog";
import {
  getChargeCarPage,
  addOrUpdate,
  ChargeCarDelete,
  ChargeAdd,
  getNodeList
} from "@/api/data/charge";
// import { getList } from "@/api/account/org";
import type { FormItemProps } from "../utils/types";
// import { usePublicHooks } from "../../hooks";
import { GetDictItemInLocal } from "@/utils/auth";
import editForm from "../../car/form.vue";
import selectCar from "../../car/avoid.vue";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(
  tableRef?: Ref | undefined,
  keyValue?: Ref | undefined
) {
  const { t } = useMyI18n(); // const { switchStyle } = usePublicHooks();
  const query = reactive({
    carTypeId: "",
    code: "",
    name: ""
  });
  const formRef = ref();
  const selectRef = ref();
  const carTypeList = ref([]);
  const loading = ref(true);
  const nodeList = ref([]);
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
    { type: "selection", align: "left" },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.carType"),
      label: "table.carType",
      prop: "carTypeName"
    },
    {
      headerRenderer: () => t("table.frontSite"),
      label: "table.frontSite",
      prop: "prevNodeCode"
    },
    {
      headerRenderer: () => t("table.currentSite"),
      label: "table.currentSite",
      prop: "currNodeCode"
    },
    {
      headerRenderer: () => t("table.rearSite"),
      label: "table.rearSite",
      prop: "nextNodeCode"
    },
    {
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code"
    },
    {
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "name"
    },
    {
      headerRenderer: () => t("table.IPAddress"),
      label: "table.IPAddress",
      prop: "ipAddress"
    },
    {
      headerRenderer: () => t("table.port"),
      label: "table.port",
      prop: "port"
    },
    {
      headerRenderer: () => t("table.type"),
      label: "table.type",
      prop: "type"
    },
    {
      headerRenderer: () => t("table.length"),
      label: "table.length",
      prop: "length"
    },
    {
      headerRenderer: () => t("table.width"),
      label: "table.width",
      prop: "width"
    },
    {
      headerRenderer: () => t("table.height"),
      label: "table.height",
      prop: "height"
    },
    {
      headerRenderer: () => t("table.isEnable"),
      label: "table.isEnable",
      prop: "isEnable",
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
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
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
    const { data } = await getChargeCarPage(keyValue.value, pageParam.value);
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
      carTypeId: row?.carTypeId ?? carTypeList.value[0].id,
      prevNodeId: row?.prevNodeId ?? null,
      currNodeId: row?.currNodeId ?? null,
      nextNodeId: row?.nextNodeId ?? null,
      code: row?.code ?? null,
      name: row?.name ?? null,
      ipAddress: row?.ipAddress ?? null,
      port: row?.port ?? null,
      type: row?.type ?? null,
      manufacturer: row?.manufacturer ?? null,
      serialNumber: row?.serialNumber ?? null,
      length: row?.length ?? 0,
      width: row?.width ?? 0,
      height: row?.height ?? 0,
      controlType: row?.controlType ?? "",
      avoidType: row?.avoidType ?? "",
      minBattery: row?.minBattery ?? "30",
      maxBattery: row?.maxBattery ?? "80",
      isEnable: row?.isEnable !== undefined ? row.isEnable : true,
      remark: row?.remark ?? null,
      extend: row?.extend ?? null
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
      contentRenderer: () => h(selectCar, { ref: selectRef }),
      beforeSure: async (done, { options, index }) => {
        const rows = selectRef.value.tableRef.getTableRef().getSelectionRows();
        let idList = rows.map(item => item.id);
        await ChargeAdd(keyValue.value, idList);
        message("操作成功！", { type: "success" });
        handleSearch();
        done();
      }
    });
  }
  async function TypeListToSelect() {
    const { data } = await GetTypeListToSelect();
    carTypeList.value = [...data];
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
        let idList = rows.map(item => item.id);
        await ChargeCarDelete(keyValue.value, idList);
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function getNodeListToSelect() {
    const { data } = await getNodeList();
    nodeList.value = [...data];
  }

  onMounted(async () => {
    TypeListToSelect();
    handleSearch();
    getNodeListToSelect();
  });

  return {
    query,
    loading,
    columns,
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
    // handleAvoidCar,
    carTypeList
  };
}
