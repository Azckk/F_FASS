import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  GetTypeListToSelect,
  getPage,
  getNodeList,
  addOrUpdate,
  deletes,
  enable,
  disable
} from "@/api/monitor/car";
import { type FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  const switchLoadMap = ref({});
  const query = reactive({
    carTypeId: "",
    code: "",
    name: ""
  });
  const formRef = ref();
  const CarControlTypeList = ref();
  const CarAvoidTypeList = ref(); //避让类型
  const nodeList = ref(); //站点
  const loading = ref(true);
  const carTypeList = ref([]);
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
      headerRenderer: () => t("table.startSite"),
      label: "table.startSite",
      prop: "startEdgeId"
    },
    {
      headerRenderer: () => t("table.endSite"),
      label: "table.endSite",
      prop: "endEdgeId"
    },

    {
      headerRenderer: () => t("table.frontRoute"),
      label: "table.frontRoute",
      prop: ""
    },
    {
      headerRenderer: () => t("table.rearRoute"),
      label: "table.rearRoute",
      prop: ""
    },
    {
      headerRenderer: () => t("table.currentRoute"),
      label: "table.currentRoute",
      prop: ""
    },
    {
      headerRenderer: () => t("table.startRoute"),
      label: "table.startRoute",
      prop: ""
    },
    {
      headerRenderer: () => t("table.endRoute"),
      label: "table.endRoute",
      prop: ""
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
      headerRenderer: () => t("table.manufacturer"),
      label: "table.manufacturer",
      prop: "manufacturer"
    },
    {
      headerRenderer: () => t("table.serialNumber"),
      label: "table.serialNumber",
      prop: "serialNumber"
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
      headerRenderer: () => t("table.controlType"),
      label: "table.controlType",
      prop: "controlType"
    },
    {
      headerRenderer: () => t("table.avoidType"),
      label: "table.avoidType",
      prop: "avoidType"
    },
    {
      headerRenderer: () => t("table.preState"),
      label: "table.preState",
      prop: "preState"
    },
    {
      headerRenderer: () => t("table.postState"),
      label: "table.postState",
      prop: "postState"
    },
    {
      headerRenderer: () => t("table.electricity"),
      label: "table.electricity",
      prop: "electricity"
    },
    {
      headerRenderer: () => t("table.minBattery"),
      label: "table.minBattery",
      prop: "minBattery"
    },
    {
      headerRenderer: () => t("table.maxBattery"),
      label: "table.maxBattery",
      prop: "maxBattery"
    },
    {
      headerRenderer: () => t("table.coordinatesX"),
      label: "table.coordinatesX",
      prop: "X"
    },
    {
      headerRenderer: () => t("table.coordinatesY"),
      label: "table.coordinatesY",
      prop: "Y"
    },
    {
      headerRenderer: () => t("table.angle"),
      label: "table.angle",
      prop: "angle"
    },
    {
      headerRenderer: () => t("table.speed"),
      label: "table.speed",
      prop: "speed"
    },
    {
      headerRenderer: () => t("table.isOnline"),
      label: "table.isOnline",
      prop: "isOnline",
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.isOnline ? "green" : "red"} class="no-inherit">
          {row.isOnline ? (
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
      headerRenderer: () => t("table.isNormal"),
      label: "table.isNormal",
      prop: "isNormal",
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.isNormal ? "green" : "red"} class="no-inherit">
          {row.isNormal ? (
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
      // controlType: row?.controlType ?? CarControlTypeList.value[0].id,
      // avoidType: row?.avoidType ?? CarAvoidTypeList.value[0].id,
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

  async function TypeListToSelect() {
    const { data } = await GetTypeListToSelect();
    carTypeList.value = [...data];
  }
  async function nodeListToSelect() {
    const { data } = await getNodeList();
    nodeList.value = [...data];
  }

  async function GetCarControlTypeList() {
    try {
      const data = await GetDictItemInLocal("CarControlType");
      CarControlTypeList.value = [...data];
      console.log("CarControlTypeList", data);
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function GetCarAvoidTypeList() {
    try {
      const data = await GetDictItemInLocal("CarAvoidType");
      CarAvoidTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
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

  onMounted(async () => {
    // 需要在这里获取数据
    // carTypeList
    TypeListToSelect();
    nodeListToSelect();
    GetCarControlTypeList();
    GetCarAvoidTypeList();
    handleSearch();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    carTypeList,
    nodeList,
    dataList,
    pagination,
    CarControlTypeList,
    CarAvoidTypeList,
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
