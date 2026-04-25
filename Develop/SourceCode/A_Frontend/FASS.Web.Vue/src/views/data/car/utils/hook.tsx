import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  GetTypeListToSelect,
  getPage,
  getNodeList,
  addOrUpdate,
  deletes,
  enable,
  disable,
  getSimpleCars
} from "@/api/data/car";
import type { FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import zoneForm from "../zone.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { useMyI18n } from "@/plugins/i18n";
const isEnglish = new RegExp("[A-Za-z]+");
export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  const query = reactive({
    carTypeId: "",
    code: "",
    name: ""
  });
  const formRef = ref();
  const CarControlTypeList = ref();
  const CarProtocolTypeList = ref();
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
      headerRenderer: () => t("table.agreementType"),
      label: "table.agreementType",
      prop: "type",
      formatter: ({ type }) => {
        let dictResult = CarProtocolTypeList?.value?.filter(
          x => x.code === type
        );
        if (dictResult && dictResult[0]) {
          if (!isEnglish.test(t("table.agreementType"))) {
            return dictResult[0].name;
          }
        }
        return type;
      }
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
    },
    { label: "所属区域", slot: "operation", minWidth: 120 }
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
      controlType: row?.controlType ?? CarControlTypeList.value[0]?.code,
      avoidType: row?.avoidType ?? CarAvoidTypeList.value[0]?.code,
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

  function handleZonePage(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "所属区域",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: row },
      contentRenderer: () => zoneForm
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

  async function handleImport() {
    await getSimpleCars();
    message("操作成功！", { type: "success" });
    handleSearch();

    // handleSearch();
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
      // console.log("CarControlTypeList", data);
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function GetCarProtocolType() {
    try {
      const data = await GetDictItemInLocal("CarProtocolType");
      CarProtocolTypeList.value = [...data];
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
    GetCarProtocolType();
    TypeListToSelect();
    GetCarControlTypeList();
    // nodeListToSelect();
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
    CarProtocolTypeList,
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
    handleDisable,
    handleZonePage,
    handleImport
  };
}
