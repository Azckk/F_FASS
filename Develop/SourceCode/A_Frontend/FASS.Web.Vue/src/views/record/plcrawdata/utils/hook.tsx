import { type Ref, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
// import { GetTypeListToSelect ,  getNodeList , addOrUpdate, deletes, enable, disable} from "@/api/data/car";
import {
  getPage,
  DeleteAll,
  DeleteD1,
  DeleteM1,
  DeleteM3,
  DeleteW1
} from "@/api/record/plcrawdata";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  const query = reactive({
    carTypeId: "",
    code: "",
    name: "",
    createAtStart: "",
    createAtEnd: "",
    createAt: "",
    time: [dayjs().subtract(1, "week").toDate(), new Date()]
  });
  // const formRef = ref();
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
  const shortcuts = [
    {
      text: "Last week",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
        return [start, end];
      }
    },
    {
      text: "Last month",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
        return [start, end];
      }
    },
    {
      text: "Last 3 months",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
        return [start, end];
      }
    }
  ];
  const columns: TableColumnList = [
    { type: "selection", align: "left" },
    // {
    //   headerRenderer: () => t("table.number"),
    //   label: "table.number",
    //   prop: "id"
    //   // hide: true
    // },
    {
      headerRenderer: () => t("table.message"),
      label: "table.message",
      prop: "message",
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
    const where = Object.keys(query)
      .filter(key => query[key])
      .flatMap(key => {
        if (key === "time" && Array.isArray(query.time)) {
          // 处理时间范围
          const [start, end] = query.time;
          return [
            {
              logic: "And",
              field: "createAt",
              operator: "GreaterEqual",
              value: dayjs(start).startOf("day").format("YYYY-MM-DD HH:mm:ss")
            },
            {
              logic: "And",
              field: "createAt",
              operator: "LessEqual",
              value: dayjs(end).endOf("day").format("YYYY-MM-DD HH:mm:ss")
            }
          ];
        } else {
          // 处理其他查询条件
          return {
            logic: "And",
            field: key,
            operator: "Contains",
            value: query[key]
          };
        }
      });
    return {
      pageParam: JSON.stringify({
        where,
        order: [{ field: "createAt", sequence: "DESC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    console.log(pageParam.value);
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
      carTypeId: row?.carTypeId ?? carTypeList.value[0]?.id ?? null,
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
      controlType: row?.controlType ?? CarControlTypeList.value[0].id,
      avoidType: row?.avoidType ?? CarAvoidTypeList.value[0].id,
      minBattery: row?.minBattery ?? "30",
      maxBattery: row?.maxBattery ?? "80",
      isEnable: row?.isEnable !== undefined ? row.isEnable : true,
      remark: row?.remark ?? null,
      extend: row?.extend ?? null,
      level: row?.level ?? null,
      createAt: row?.createAt ?? null,
      message: row?.message ?? null,
      id: row?.id ?? null
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

  async function handleEnable(/*rows = selection.value, cancel = undefined*/) {
    // if (rows.length === 0) {
    //   message("请至少选择一项数据再进行操作！", { type: "warning" });
    //   return;
    // }
    // ElMessageBox.confirm("是否确认操作？", "提示", { type: "warning", draggable: true })
    //   .then(async () => {
    //     await enable(rows.map(e => e.id));
    //     message("操作成功！", { type: "success" });
    //     handleSearch();
    //   })
    //   .catch(() => cancel?.());
  }

  async function handleDisable(/*rows = selection.value, cancel = undefined*/) {
    // if (rows.length === 0) {
    //   message("请至少选择一项数据再进行操作！", { type: "warning" });
    //   return;
    // }
    // ElMessageBox.confirm("是否确认操作？", "提示", { type: "warning", draggable: true })
    //   .then(async () => {
    //     await disable(rows.map(e => e.id));
    //     message("操作成功！", { type: "success" });
    //     handleSearch();
    //   })
    //   .catch(() => cancel?.());
  }

  async function GetCarControlTypeList() {
    try {
      const data = await GetDictItemInLocal("CarControlType");
      CarControlTypeList.value = [...data];
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

  async function handleDeleteAll(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteAll();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  function handleDeleteD1(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteD1();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  function handleDeleteW1(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteW1();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  function handleDeleteM1(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteM1();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }
  function handleDeleteM3(cancel = undefined) {
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await DeleteM3();
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  onMounted(async () => {
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
    shortcuts,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDetail,
    handleEnable,
    handleDisable,
    handleDeleteAll,
    handleDeleteD1,
    handleDeleteW1,
    handleDeleteM1,
    handleDeleteM3
  };
}
