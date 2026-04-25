import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
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
  getChargeList,
  GetListToSelect
} from "@/api/monitor/car";
import {
  getNodeList as getNodeList2,
  ActionInit,
  ActionForceStop,
  // ActionRepair,
  ActionBlown,
  ActionCarGoToSomePlace,
  ActionReStart,
  GetCarInfo,
  ActionStopReceiveTask,
  ActionGoToStandby,
  ActionStopOrStart,
  ActionGoToCharge
} from "@/api/monitor/caraction";
import type { FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { useMyI18n } from "@/plugins/i18n";
// import { useMapStore } from "@/store/modules/map";

export function useHook(tableRef?: Ref | undefined) {
  const { t, locale } = useMyI18n();
  // const switchLoadMap = ref({});
  const query = reactive({
    carTypeId: "",
    code: "",
    name: "",
    CurrState: ""
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
  const TaskRecordStateList = ref([]);
  // const store = useMapStore();
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
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code"
    },
    {
      headerRenderer: () => t("table.alarm"),
      label: "table.alarm",
      prop: "isAlarm"
    },
    {
      headerRenderer: () => t("table.carStatus"),
      label: "table.carStatus",
      prop: "currState"
      //   formatter: ({ type }) => {
      //   let dictResult = TaskRecordStateList?.value?.filter(
      //     x => x.code === type
      //   );
      //   if (dictResult && dictResult[0]) {
      //     if (!isEnglish.test(t("table.carStatus"))) {
      //       return dictResult[0].name;
      //     }
      //   }
      //   return type;
      // }
    },
    {
      headerRenderer: () => t("table.trafficState"),
      label: "table.trafficState",
      prop: "trafficMessage"
    },
    {
      headerRenderer: () => t("table.electricity"),
      label: "table.electricity",
      prop: "battery"
    },
    {
      headerRenderer: () => t("table.isOnline"),
      label: "table.isOnline",
      prop: "isOnline",
      cellRenderer: ({ row }) => (
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
      headerRenderer: () => t("table.updateAt"),
      label: "table.updateAt",
      prop: "updateAt",
      minWidth: 120,
      formatter: ({ updateAt }) =>
        updateAt ? dayjs(updateAt).format("YYYY-MM-DD HH:mm:ss") : ""
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
    for (let i = 0; i < dataList.value.length; i++) {
      const { data } = await GetCarInfo(dataList.value[i].code);
      dataList.value[i].isAlarm = data.isAlarm ? t("table.yes") : t("table.no");
      dataList.value[i].speed = data.speed;
      dataList.value[i].trafficMessage = data.trafficMessage;
    }
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
      extend: row?.extend ?? null,
      isAlarm: row?.isAlarm ?? false,
      trafficMessage: row?.trafficMessage ?? null,
      CurrentTaskId: row?.CurrentTaskId ?? null,
      speed: row?.speed ?? null,
      battery: row?.battery ?? null,
      stopAccept: row?.stopAccept ?? false
    };
  }

  function handleDetail(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "详情",
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
      beforeSure: (done, { options }) => {
        const formData = options.props.formInline as FormItemProps;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await addOrUpdate({}, formData);
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
      beforeSure: (done, { options }) => {
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
      // console.log("CarControlTypeList", data);
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
  // x
  const title = ref();
  const handleClick = event => {
    let target = event.target;
    // console.log("target", event);
    while (
      target &&
      target !== event.currentTarget &&
      !target.getAttribute("data-action")
    ) {
      target = target.parentNode;
    }
    if (target && target.getAttribute("data-action")) {
      const action = target.getAttribute("data-action");
      switch (action) {
        case "goWhere":
          goWhere();
          break;
        case "goCharging":
          goCharging();
          break;
        case "goStartPoint":
          goStartPoint();
          break;
        case "initialization":
          initialization();
          break;
        case "Pause/Resume":
          Resume();
          break;
        case "endTask":
          endTask();
          break;
        case "completelyOffline":
          completelyOffline();
          break;
        case "stopTask":
          stopTask();
          break;
        // case "recoveryTask":
        //   recoveryTask();
        //   break;
        case "Restart":
          Restart();
          break;
        default:
          console.log("Unknown action:", action);
      }
    }
  };
  const sendData = ref({
    side: "",
    carId: "",
    targetNodeId: ""
  });
  const dialogVisible = ref(false);
  const carCode = ref();
  async function goWhere(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    carCode.value = row.code;
    sendData.value.targetNodeId = "";
    await getNodeListFn();
    locale.value == "zh" ? (title.value = "去某地") : (title.value = "goWhere");
    dialogVisible.value = true;
  }
  async function sendGoWhere(row) {
    // let data = JSON.stringify({
    //   id: sendData.value.targetNodeId,
    //   carCode: carCode.value
    // });
    // let res = await ActionCarGoToSomePlace({ Param: data });
    // dialogVisible.value = false;
    // sendData.value.targetNodeId = "";
    // carCode.value = "";
    let data = JSON.stringify({
      id: row.targetNodeId,
      carCode: carCode.value
    });
    await tableRef.value.validate(async valid => {
      if (valid) {
        let res = await ActionCarGoToSomePlace({ Param: data });
        if (res.code == 200 && res.data == "OK") {
          message("发送成功！", { type: "success" });
        }
        dialogVisible.value = false;
        carCode.value = "";
        row.targetNodeId = "";
      } else {
        // console.log("error submit!");
      }
    });
  }
  // async function goCharging(rows = selection.value) {
  //   if (rows.length === 0) {
  //     message("请至少选择一项数据再进行操作！", { type: "warning" });
  //     return;
  //   }
  //   const row = rows[0];
  //   carCode.value = row.code;
  //   sendData.value.targetNodeId = "";
  //   await getChargeListFn();
  //   locale.value == "zh"
  //     ? (title.value = "去充电")
  //     : (title.value = "goCharging");
  //   dialogVisible.value = true;
  // }

  function goCharging(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    carCode.value = row.code;
    ElMessageBox.confirm(t("buttons.goCharging"))
      .then(async () => {
        let res = await ActionGoToCharge({ carCode: carCode.value });
        if (res.code == 200 && res.data == "OK") {
          ElMessage({
            message: `${t("buttons.car")}${row.code}${t("buttons.goCharging")}！`,
            type: "success"
          });
        }
      })
      .catch(() => {
        // catch error
      });
  }

  function goStartPoint(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    ElMessageBox.confirm(t("buttons.goStartPoint"))
      .then(async () => {
        let res = await ActionGoToStandby({ carCode: row.code });
        if (res.code == 200 && res.data == "OK") {
          ElMessage({
            message: `${t("buttons.car")}${row.code}${t("buttons.goStartPoint")}！`,
            type: "success"
          });
        }
      })
      .catch(() => {
        // catch error
      });
  }
  function initialization(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    ElMessageBox.confirm(t("buttons.initialization"))
      .then(async () => {
        let res = await ActionInit({ carCode: row.code });
        if (res.code == 200 && res.data == "OK") {
          ElMessage({
            message: `${t("buttons.car")}${row.code}${t("buttons.initialization")}！`,
            type: "success"
          });
        }
      })
      .catch(() => {
        // catch error
      });
  }
  function Resume(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    ElMessageBox.confirm(t("buttons.Pause/Resume"))
      .then(async () => {
        let res = await ActionStopOrStart({ Param: row.code });
        if (res.code == 200 && res.data == "OK") {
          ElMessage({
            message: `${t("buttons.car")}${row.code}${t("buttons.Pause/Resume")}！`,
            type: "success"
          });
        }
      })
      .catch(() => {
        // catch error
      });
  }
  function endTask(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    ElMessageBox.confirm(t("buttons.cancelTask"))
      .then(async () => {
        let res = await ActionForceStop({ carCode: row.code });
        if (res.code == 200 && res.data == "OK") {
          ElMessage({
            message: `${t("buttons.car")}${row.code}${t("buttons.cancelTask")}！`,
            type: "success"
          });
        }
      })
      .catch(() => {
        // catch error
      });
  }
  function completelyOffline(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    ElMessageBox.confirm(t("buttons.completelyOffline"))
      .then(async () => {
        let res = await ActionBlown({ carCode: row.code });
        if (res.code == 200 && res.data == "OK") {
          ElMessage({
            message: `${t("buttons.car")}${row.code}${t("buttons.completelyOffline")}！`,
            type: "success"
          });
        }
      })
      .catch(() => {
        // catch error
      });
  }
  function stopTask(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    ElMessageBox.confirm(t("buttons.Stop/Recovery"))
      .then(async () => {
        let res = await ActionStopReceiveTask({
          carCode: row.code
        });
        if (res.code == 200 && res.data == "OK") {
          ElMessage({
            message: `${t("buttons.car")}${row.code}${t("buttons.Stop/Recovery")}！`,
            type: "success"
          });
          setTimeout(() => {
            handleSearch();
          }, 500);
        }
      })
      .catch(() => {
        // catch error
      });
  }

  // function recoveryTask(rows = selection.value) {
  //   if (rows.length === 0) {
  //     message("请至少选择一项数据再进行操作！", { type: "warning" });
  //     return;
  //   }
  //   const row = rows[0];
  //   if (row.stopAccept == true) {
  //     ElMessageBox.confirm(t("buttons.recoveryTask"))
  //       .then(async () => {
  //         let res = await ActionStopReceiveTask({
  //           carCode: row.code
  //         });
  //         if (res.code == 200 && res.data == "OK") {
  //           ElMessage({
  //             message: `${t("buttons.car")}${row.code}${t("buttons.recoveryTask")}！`,
  //             type: "success"
  //           });
  //           setTimeout(() => {
  //             handleSearch();
  //           }, 500);
  //         }
  //       })
  //       .catch(() => {
  //         // catch error
  //       });
  //   } else {
  //     message("该车辆正处于恢复任务状态，请选择停接任务！", {
  //       type: "warning"
  //     });
  //     return;
  //   }
  // }

  function Restart(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    ElMessageBox.confirm(t("buttons.restart")).then(async () => {
      try {
        let res = await ActionReStart({ carCode: row.code });
        if (res.code == 200 && res.data == "OK") {
          ElMessage({
            message: `${t("buttons.car")}${row.code}${t("buttons.restart")}！`,
            type: "success"
          });
        }
      } catch (error) {
        ElMessage({
          message: "1111",
          type: "error"
        });
      }
    });
  }
  async function GetTaskRecordStateList() {
    try {
      const data = await GetDictItemInLocal("CarState");
      console.log(data);

      TaskRecordStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  const nodeListData = ref();
  async function getNodeListFn() {
    let pageParam = handlePageParam();
    let { data } = await GetListToSelect(pageParam);
    nodeListData.value = data;
  }
  const chargeNodeListData = ref();

  async function getChargeListFn() {
    let pageParam = handlePageParam();
    let { data } = await getChargeList(pageParam);
    chargeNodeListData.value = data;
  }

  onMounted(async () => {
    // 需要在这里获取数据
    // carTypeList
    TypeListToSelect();
    nodeListToSelect();
    GetCarControlTypeList();
    GetCarAvoidTypeList();
    handleSearch();
    GetTaskRecordStateList();
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
    nodeListData,
    chargeNodeListData,
    dialogVisible,
    title,
    sendData,
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
    handleClick,
    initialization,
    sendGoWhere,
    TaskRecordStateList
  };
}
