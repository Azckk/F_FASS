import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { GetListToSelectByTaskTemplateId } from "@/api/flow/taskinstance";
import {
  getPage,
  DeleteAll,
  DeleteD1,
  DeleteM1,
  DeleteM3,
  DeleteW1,
  ExportExcel
} from "@/api/monitor/alarm";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef: Ref) {
  const { t } = useMyI18n();
  const switchLoadMap = ref({});
  const query = reactive({
    code: "",
    message: "",
    createAtStart: "",
    createAtEnd: "",
    createAt: "",
    // carId: "",
    time: [dayjs().subtract(1, "week").toDate(), new Date()]
  });
  const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const ListToSelectByTaskTemplateId = ref([]); //模板获取车辆
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
      type: "index",
      width: 60
    },
    {
      headerRenderer: () => t("table.ownVehicle"),
      label: "table.carName",
      prop: "remark"
    },
    {
      headerRenderer: () => t("table.level"),
      label: "table.level",
      prop: "level"
    },
    {
      headerRenderer: () => t("table.type"),
      label: "table.type",
      prop: "type"
    },
    {
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code"
    },
    {
      headerRenderer: () => t("table.message"),
      label: "table.message",
      prop: "message"
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      minWidth: 120,
      formatter: ({ createAt }) =>
        createAt ? dayjs(createAt).format("YYYY-MM-DD HH:mm:ss") : ""
    }
    // {
    //   headerRenderer: () => t("table.operation"),
    //   slot: "operation",
    //   minWidth: 120
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

  // function handlePageParam() {
  //   return {
  //     pageParam: JSON.stringify({
  //       where: Object.keys(query)
  //         .filter(e => query[e])
  //         .map(e => ({
  //           logic: "And",
  //           field: e,
  //           operator: "Contains",
  //           value: query[e]
  //         })),
  //       order: [{ field: "createAt", sequence: "DESC" }],
  //       number: pagination.currentPage,
  //       size: pagination.pageSize
  //     })
  //   };
  // }
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
    const { data } = await getPage(pageParam.value);
    dataList.value = data.rows;
    pagination.total = data.total;
    loading.value = false;
  }
  async function taskTemplateIdChange(value: any) {
    if (value !== null && value !== undefined) {
      tableRef.value.resetFields(["carId"]);
    }
    const { data } = await GetListToSelectByTaskTemplateId();
    ListToSelectByTaskTemplateId.value = [...data];
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
      name: row?.name ?? null,
      code: row?.code ?? null,
      sortNumber: row?.sortNumber ?? null,
      isEnable: row?.isEnable !== undefined ? row.isEnable : true
    };
  }
  async function handleDelete(rows = selection.value, cancel = undefined) {
    // if (rows.length === 0) {
    //   message("请至少选择一项数据再进行操作！", { type: "warning" });
    //   return;
    // }
    // ElMessageBox.confirm("是否确认操作？", "提示", {
    //   type: "warning",
    //   draggable: true
    // })
    //   .then(async () => {
    //     await deletes(rows.map(e => e.id));
    //     message("操作成功！", { type: "success" });
    //     handleSearch();
    //   })
    //   .catch(() => cancel?.());
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

  async function handleExport() {
    try {
      const response = await ExportExcel();
      // 处理文件下载
      const blob = new Blob([response], {
        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
      });
      console.log(blob);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", "报警监控.xlsx");
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error("导出失败:", error);
    }
  }

  onMounted(async () => {
    handleSearch();
    taskTemplateIdChange(null);
  });

  return {
    query,
    loading,
    columns,
    shortcuts,
    dataList,
    pagination,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDelete,
    handleDetail,
    handleDeleteM3,
    handleDeleteM1,
    handleDeleteW1,
    handleDeleteD1,
    handleDeleteAll,
    handleExport,
    ListToSelectByTaskTemplateId
  };
}
