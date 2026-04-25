import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
// import { GetTypeListToSelect ,  getNodeList , addOrUpdate, deletes, enable, disable} from "@/api/data/car";
import { GetAlarmReport } from "@/api/report/fault/index";
import { type FormItemProps } from "./types";
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
    time: [dayjs().subtract(1, "week").toDate(), new Date()] // 最近一周的默认值
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
  const formattedQuery = reactive({
    createAtStart: "",
    createAtEnd: "",
    time: [dayjs().subtract(1, "week").toDate(), new Date()]
  });
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });
  const columns: TableColumnList = [{ type: "selection", align: "left" }];
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
  function handleSelection(val) {
    selection.value = val;
  }

  function handlePageSize() {
    handleSearch();
  }

  function handlePageCurrent() {
    handleSearch();
  }

  function organizeData(codes, backendData) {
    // 创建一个字典，用于存储后端返回的数据，以code为key，alarmCount为value
    const backendDict = backendData.reduce((acc, item) => {
      acc[item.code] = item.alarmCount;
      return acc;
    }, {});
    // 创建结果数组，根据预定义的codes顺序填充数据
    const result = codes.map(code => {
      return {
        key: code,
        value: backendDict[code] !== undefined ? backendDict[code] : 0
      };
    });

    return result;
  }

  function handleMsg() {
    const [start, end] = query.time;
    return {
      Param: JSON.stringify({
        carCode: "12",
        createAtStart: dayjs(start).startOf("day").format("YYYY-MM-DD"),
        createAtEnd: dayjs(end).endOf("day").add(1, "day").format("YYYY-MM-DD") // 只支持YYYY-MM-DD 不支持YYYY-MM-DD HH:mm:ss
      })
    };
  }
  let Param = ref();
  async function handleSearch() {
    // ["total", "1", "2", "3", "4", "5"];
    loading.value = true;
    Param.value = handleMsg();
    const { data } = await GetAlarmReport(Param.value);
    dataList.value = data;
    // 预定义的数组
    const codes = ["total", "1", "2", "3", "4", "5"];
    dataList.value.totalData = organizeData(codes, dataList.value.totalData);

    // loading.value = false;
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }

  // 后端返回的实例数据
  // const backendData = [
  //   {
  //     alarmCount: 0,
  //     name: "总故障",
  //     code: "total"
  //   }
  // ];

  // 调用方法并输出结果

  function handleRow(row = undefined) {
    return {
      createAtStart: formattedQuery.createAtStart,
      createAtEnd: formattedQuery.createAtEnd
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
  function handleSearch7Day(cancel = undefined) {}
  function handleSearch30Day(cancel = undefined) {}
  const handleDateChange = (value: [Date, Date]) => {
    formattedQuery.createAtStart = dayjs(value[0]).format("YYYY-MM-DD");
    formattedQuery.createAtEnd = dayjs(value[1]).format("YYYY-MM-DD");
    console.log(formattedQuery); // 这里你可以处理转换后的值
  };

  handleDateChange(query.time); // 初始化时转换默认值
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
