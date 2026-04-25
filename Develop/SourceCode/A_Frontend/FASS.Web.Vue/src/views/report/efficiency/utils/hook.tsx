import { type Ref, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
// import { GetTypeListToSelect ,  getNodeList , addOrUpdate, deletes, enable, disable} from "@/api/data/car";
import { GetTaskReport } from "@/api/report/efficiency/index";
import { type FormItemProps } from "../utils/types";
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
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
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

  function handleMsg() {
    const [start, end] = query.time;
    return {
      Param: JSON.stringify({
        createAtStart: dayjs(start)
          .startOf("day")
          .format("YYYY-MM-DD HH:mm:ss"),
        createAtEnd: dayjs(end).endOf("day").format("YYYY-MM-DD HH:mm:ss")
      })
    };
  }
  let Param = ref();
  async function handleSearch() {
    loading.value = true;
    Param.value = handleMsg();
    console.log(Param.value);
    const { data } = await GetTaskReport(Param.value);
    // dataList.value = {
    //   taskDayList: [
    //     {
    //       day: "2024/06/29 00:00:00",
    //       success: 5,
    //       failure: 1
    //     },
    //     {
    //       day: "2024/06/30 00:00:00",
    //       success: 12,
    //       failure: 2
    //     },
    //     {
    //       day: "2024/07/01 00:00:00",
    //       success: 11,
    //       failure: 3
    //     },
    //     {
    //       day: "2024/07/02 00:00:00",
    //       success: 15,
    //       failure: 2
    //     },
    //     {
    //       day: "2024/07/03 00:00:00",
    //       success: 9,
    //       failure: 1
    //     },
    //     {
    //       day: "2024/07/04 00:00:00",
    //       success: 12,
    //       failure: 3
    //     },
    //     {
    //       day: "2024/07/05 00:00:00",
    //       success: 12,
    //       failure: 1
    //     },
    //     {
    //       day: "2024/07/06 00:00:00",
    //       success: 200,
    //       failure: 1
    //     }
    //   ],
    //   taskTotal: {
    //     success: 9999999,
    //     failure: 999999,
    //     total: 99999
    //   }
    // };
    dataList.value = data;
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
      controlType: row?.controlType ?? CarControlTypeList.value[0].id,
      avoidType: row?.avoidType ?? CarAvoidTypeList.value[0].id,
      minBattery: row?.minBattery ?? "30",
      maxBattery: row?.maxBattery ?? "80",
      isEnable: row?.isEnable !== undefined ? row.isEnable : true,
      remark: row?.remark ?? null,
      extend: row?.extend ?? null,
      level: row?.level ?? null,
      createAt: row?.createAt ?? null,
      message: row?.message ?? null
    };
  }

  function handleDetail(rows = selection.value) {
    // if (rows.length === 0) {
    //   message("请至少选择一项数据再进行操作！", { type: "warning" });
    //   return;
    // }
    // const row = rows[0];
    // addDialog({
    //   title: "查看",
    //   width: "60%",
    //   alignCenter: true,
    //   draggable: true,
    //   fullscreenIcon: true,
    //   hideFooter: true,
    //   props: { formInline: handleRow(row) },
    //   contentRenderer: () => editForm
    // });
  }

  async function handleEnable(rows = selection.value, cancel = undefined) {
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

  async function handleDisable(rows = selection.value, cancel = undefined) {
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
    // try {
    //   const data = await GetDictItemInLocal("CarControlType");
    //   CarControlTypeList.value = [...data];
    // } catch (error) {
    //   console.error("Error:", error);
    // }
  }

  async function GetCarAvoidTypeList() {
    // try {
    //   const data = await GetDictItemInLocal("CarAvoidType");
    //   CarAvoidTypeList.value = [...data];
    // } catch (error) {
    //   console.error("Error:", error);
    // }
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
  function handleSearch7Day(cancel = undefined) {}
  function handleSearch30Day(cancel = undefined) {}

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
    handleDeleteM3,
    handleSearch7Day,
    handleSearch30Day
  };
}
