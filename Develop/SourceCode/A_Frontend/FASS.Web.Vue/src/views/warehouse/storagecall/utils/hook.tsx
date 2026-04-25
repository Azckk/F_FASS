import { type Ref, ref, reactive, onMounted, h } from "vue";
import { ElMessageBox } from "element-plus";
import { message } from "@/utils/message";
import { GetDictItemInLocal } from "@/utils/auth";
import dayjs from "dayjs";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { useUserStore } from "@/store/modules/user";
import { router, resetRouter } from "@/router";
import {
  GetPage,
  deletes,
  DeleteAll,
  DeleteD1,
  DeleteM1,
  DeleteM3,
  DeleteW1
} from "@/api/mobile/pda";
import { useMyI18n } from "@/plugins/i18n";
import { addDialog } from "@/components/ReDialog";
import { GetListByTypeToSelect } from "@/api/warehouse/area";
import { GetListByAreaToSelect } from "@/api/warehouse/storage";

import { GetListToSelectByTaskTemplateId } from "@/api/task/taskrecord";

import {
  GetDictItemListToSelect,
  GetMaterialListToSelect,
  AddWork
} from "@/api/mobile/pda";
import editForm from "../form.vue";

const isEnglish = new RegExp("[A-Za-z]+");
export function useHook(tableRef?: Ref) {
  const { t } = useMyI18n();
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    state: "",
    areaId: "",
    storageId: "",
    callMode: "",
    carId: "",
    materialId: "",
    createAtStart: "",
    createAtEnd: "",
    createAt: "",
    time: [dayjs().subtract(1, "week").toDate(), new Date()]
  });
  const TaskInstanceStateList = ref([]); // 任务实例类型列表
  const loading = ref(true);
  const dataList = ref([]);
  const areaList = ref([]);
  const MaterialList = ref([]);
  const storageList = ref([]);
  const modelList = ref([]);
  const pageParam = ref({});
  const formRef = ref();
  const selection = ref([]);
  const ListToSelectByTaskTemplateId = ref([]); //模板获取车辆
  // const TaskInstanceActionStateList = ref([]);

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
      prop: "code",
      sortable: true
    },
    // {
    //   headerRenderer: () => t("table.taskId"),
    //   label: "table.taskId",
    //   prop: "taskId",
    //   sortable: true
    // },
    {
      headerRenderer: () => t("table.callmodel"),
      label: "table.callmodel",
      prop: "name",
      sortable: true,
      formatter: ({ name }) => {
        const parts = name.split("-")[1];
        if (parts) return parts;
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.callarea"),
      label: "table.callarea",
      prop: "areaName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.container"),
      label: "table.container",
      prop: "containerName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.barcode"),
      label: "table.barcode",
      prop: "barcode"
    },
    // {
    //   headerRenderer: () => t("table.ownVehicle"),
    //   label: "table.ownVehicle",
    //   prop: "carId",
    //   sortable: true,
    //   formatter: ({ carId }) => {
    //     if (carId) {
    //       return getCarName(carId);
    //     }
    //     return "--无--";
    //   }
    // },
    // 以下字段未验证
    {
      headerRenderer: () => t("table.taskStartingPoint"),
      label: "table.srcStorageName",
      prop: "srcStorageName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.taskEndingPoint"),
      label: "table.destStorageName",
      prop: "destStorageName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.carName"),
      label: "table.carName",
      prop: "carName",
      sortable: true
    },
    // {
    //   headerRenderer: () => t("table.vehicleID"),
    //   label: "table.vehicleID",
    //   prop: "",
    //   sortable: true
    // },
    {
      headerRenderer: () => t("table.state"),
      label: "table.state",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        let dictResult = TaskInstanceStateList.value.filter(
          x => x.code === state
        );
        if (dictResult && dictResult[0]) {
          if (!isEnglish.test(t("table.status"))) {
            return dictResult[0].name;
          } else {
            return dictResult[0].code;
          }
        }
        return "未知";
      }
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      sortable: true,
      formatter: ({ createAt }) => {
        if (createAt) {
          return dayjs(createAt).format("YYYY-MM-DD HH:mm:ss");
        } else {
          return "--";
        }
      }
    },
    {
      headerRenderer: () => t("table.updateAt"),
      label: "table.updateAt",
      prop: "updateAt",
      sortable: true,
      formatter: ({ updateAt }) => {
        if (updateAt) {
          return dayjs(updateAt).format("YYYY-MM-DD HH:mm:ss");
        } else {
          return "--";
        }
      }
    }
    // {
    //   headerRenderer: () => t("table.endTime"),
    //   label: "table.endTime",
    //   prop: "endTime",
    //   sortable: true,
    //   formatter: ({ endTime }) => {
    //     if (endTime) {
    //       return dayjs(endTime).format("YYYY-MM-DD HH:mm:ss");
    //     } else {
    //       return "--";
    //     }
    //   }
    // }
    // { label: "操作", slot: "operation", minWidth: 120 }
  ];
  const store = useUserStore();

  function handlePageSize() {
    handleSearch();
  }

  function handlePageCurrent() {
    handleSearch();
  }
  function handleSelection(val) {
    selection.value = val;
  }

  function getCarName(value) {
    const selectedItem = ListToSelectByTaskTemplateId.value.find(
      item => item.id === value
    );
    if (selectedItem) {
      return selectedItem.name;
    }
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

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await GetPage(pageParam.value);
    dataList.value = data.rows;
    pagination.total = data.total;
    loading.value = false;
  }
  async function taskTemplateIdChange(value: any) {
    if (value !== null && value !== undefined) {
      tableRef.value.resetFields(["carId"]);
      // tableRef.value.resetFields({ "carId":"" });
    }
    const { data } = await GetListToSelectByTaskTemplateId(value);
    ListToSelectByTaskTemplateId.value = [...data];
    // console.log("data is", data);
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }

  async function handleLogout(cancel = undefined) {
    ElMessageBox.confirm("是否确认退出？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        // await Logout();
        store.logOut();
        message("成功退出！", { type: "success" });
      })
      .catch(() => cancel?.());
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
        // console.log(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
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

  function handleGoMenu() {
    // router.push("/mobile/menu/index");
    router.go(0);
  }
  function handleRow(row = undefined) {
    return {
      areaId: row?.areaId ?? ""
    };
  }
  function handleAdd() {
    addDialog({
      title: "缺料呼叫",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow() },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options }) => {
        const formData = options.props.formInline;
        formRef.value.getRef().validate(async (valid, fields) => {
          if (valid) {
            let res = await AddWork(formData);
            if (res.code == 200) {
              message("操作成功！", { type: "success" });
              handleReset(formRef);
            } else {
              message("操作失败！", { type: "warning" });
            }
            done();
          } else {
            console.log("error submit!", fields);
          }
        });
      }
    });
  }

  async function FunctionGetListByTypeToSelect() {
    let data = await GetListByTypeToSelect({ type: "PdaCall" });
    areaList.value = data.data;
    let res = await GetMaterialListToSelect();
    MaterialList.value = res.data;
  }
  async function AreaChange(value: any, callback) {
    callback();
    let row = areaList.value.filter(area => area.id === value)[0];
    if (row) {
      let res = await GetListByAreaToSelect({ areaId: row.id });
      storageList.value = res.data;
      let data = await GetDictItemListToSelect({
        dictCode: "CallMode",
        param: row.code
      });
      modelList.value = data.data;
      console.log("data is", data);
    }
  }
  async function handleConfirm(formRef) {
    loading.value = true;
    await formRef.validate(async (valid, fields) => {
      if (valid) {
        let res = await AddWork(query);
        if (res.code == 200) {
          message("操作成功！", { type: "success" });
          handleReset(formRef);
        } else {
          message("操作失败！", { type: "warning" });
        }
      } else {
        console.log("error submit!", fields);
      }
    });
    loading.value = false;
  }
  async function GetTaskInstanceStateList() {
    try {
      const data = await GetDictItemInLocal("WorkState");
      TaskInstanceStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }
  onMounted(async () => {
    // handleSearch();
    GetTaskInstanceStateList();
    taskTemplateIdChange(null);
  });

  return {
    query,
    loading,
    columns,
    dataList,
    pagination,
    storageList,
    MaterialList,
    modelList,
    areaList,
    deviceDetection,
    handlePageSize,
    handlePageCurrent,
    taskTemplateIdChange,
    handleSearch,
    handleReset,
    handleLogout,
    handleGoMenu,
    handleAdd,
    AreaChange,
    handleConfirm,
    FunctionGetListByTypeToSelect,
    shortcuts,
    ListToSelectByTaskTemplateId,
    TaskInstanceStateList,
    handleDelete,
    handleSelection,
    handleDeleteM3,
    handleDeleteM1,
    handleDeleteW1,
    handleDeleteD1,
    handleDeleteAll
  };
}
