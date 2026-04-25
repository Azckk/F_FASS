import {
  ref,
  onMounted,
  watch,
  onUnmounted,
  reactive,
  watchEffect,
  unref
} from "vue";
import { GetMap, GetSideInfo } from "@/api/monitor/runtime";
// 叉车
// import leisure from "../../../../assets/monitor/叉车-空闲中.png";
// import task from "../../../../assets/monitor/叉车-任务中.png";
// import abnormal from "../../../../assets/monitor/叉车-异常.png";
// import offline from "../../../../assets/monitor/叉车-离线.png";
// import charge from "../../../../assets/monitor/叉车-充电.png";
// import taskFull from "../../../../assets/monitor/叉车-任务中-满载.png";
// 辊筒 CarConveyor
// import CarConveyorCharge from "../../../../assets/monitor/辊筒-充电.png";
// import CarConveyorLeisure from "../../../../assets/monitor/辊筒-空闲中.png";
// import CarConveyorOffline from "../../../../assets/monitor/辊筒-离线.png";
// import CarConveyorTask from "../../../../assets/monitor/辊筒-任务中不带载.png";
// import CarConveyorTaskFull from "../../../../assets/monitor/辊筒-任务中带载.png";
// import CarConveyorAbnormal from "../../../../assets/monitor/辊筒-异常.png";

import { message } from "@/utils/message";
import {
  connect,
  disconnect,
  setupSignalRHandlers,
  send
} from "@/api/useMessageHub";
import { ActivationCodeKey } from "@/utils/auth";
import { stringify, parse } from "flatted";
import { useMapStore } from "@/store/modules/map";
// import { text } from "stream/consumers";
import { useRoute, useRouter } from "vue-router";
import { handleAliveRoute } from "@/router/utils";

const activationCode = localStorage.getItem(ActivationCodeKey);
let lockNodeLength = ref(0);

const nodeEllipseForm = reactive({
  lock: {
    enable: false
  },
  base: {
    enable: true,
    visible: true,
    point: {
      x: 10,
      y: 10
    },
    size: {
      w: 40,
      h: 40
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0.8333333333333334
    },
    text: ""
  },
  name: {
    enable: true,
    visible: true,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  code: {
    enable: true,
    visible: true,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  data: {
    nodeDescription: "",
    released: false
  },
  fields: {
    type: "",
    x: "",
    y: "",
    id: ""
  },
  type: ""
});
const text = reactive({
  type: "TagRect",
  code: {
    text: "自动",
    font: "500px Arial"
  },
  base: {
    point: {
      x: 9142.978923437502 - 30,
      y: 606.7517124374999 - 900
    },
    fillStyle: "rgb(255, 255, 255)", // 填充颜色
    strokeStyle: "rgb(255, 255, 255)" // 边框颜色
  }
});
let cloneNodeEllipseForm = stringify(nodeEllipseForm);
// 控制赋值的时候不改变
let isWatchChange = ref(false);
const store = useMapStore();

function handleChange(val) {
  return new URL(`../../../../assets/monitor/${val}.png`, import.meta.url);
}
const getCarIconSrc = (carData, code, type) => {
  // 根据状态定义不同的图标路径
  // const iconMap = {
  //   离线: offline,
  //   空闲: leisure,
  //   任务中: task,
  //   "任务中[空载]": task,
  //   "任务中[满载]": taskFull,
  //   充电中: charge,
  //   异常: abnormal
  // };
  const CarForkliftIconMap = {
    离线: "forklift_offline",
    空闲: "forklift_idle",
    任务中: "forklift_working",
    "任务中[空载]": "forklift_working",
    "任务中[满载]": "forklift_working_full_load",
    充电中: "forklift_charging",
    异常: "forklift_error",
    告警: "forklift_warning",
    避障: "forklift_obstacle_avoidance",
    低电量: "forklift_low_battery"
  };
  const CarConveyorIconMap = {
    离线: "roller_offline",
    空闲: "roller_idle",
    任务中: "roller_working_no_load",
    "任务中[空载]": "roller_working_no_load",
    "任务中[满载]": "roller_working_with_load",
    充电中: "roller_charging",
    异常: "roller_error",
    告警: "roller_warning",
    避障: "roller_obstacle_avoidance",
    低电量: "roller_low_battery"
  };
  const CarTuggerIconMap = {
    离线: "tractor_offline",
    空闲: "tractor_idle",
    任务中: "tractor_working_no_load",
    "任务中[空载]": "tractor_working_no_load",
    "任务中[满载]": "tractor_working_with_load",
    充电中: "tractor_charging",
    异常: "tractor_error",
    告警: "tractor_warning",
    避障: "tractor_obstacle_avoidance",
    低电量: "tractor_low_battery"
  };
  const CarCarrierIconMap = {
    离线: "loader_offline",
    空闲: "loader_idle",
    任务中: "loader_working_no_load",
    "任务中[空载]": "loader_working_no_load",
    "任务中[满载]": "loader_working_with_load",
    充电中: "loader_charging",
    异常: "loader_error",
    告警: "loader_warning",
    避障: "loader_obstacle_avoidance",
    低电量: "loader_low_battery"
  };

  // if (type && type === "CarForklift") {
  //   // 默认的离线图标
  //   const defaultIcon = CarForkliftIconMap["离线"];
  //   // 查找对应的车辆数据
  //   const car = carData.find(car => car.code === code);
  //   // 如果找到对应的车辆，返回相应的图标，否则返回离线图标
  //   return car ? CarForkliftIconMap[car.state] || defaultIcon : defaultIcon;
  // }
  const car = carData.find(car => car.code === code);
  switch (type) {
    case "CarForklift": {
      // 叉车
      const url = handleChange(CarForkliftIconMap[car.state]);
      const defaultIcon = CarForkliftIconMap["离线"];
      return car ? url.pathname || defaultIcon : defaultIcon;
    }
    case "CarConveyor": {
      // CarConveyor 辊筒
      const url = handleChange(CarConveyorIconMap[car.state]);
      const defaultIcon = CarConveyorIconMap["离线"];
      return car ? url.pathname || defaultIcon : defaultIcon;
    }
    case "CarTugger": {
      // CarTugger  牵引
      const url = handleChange(CarTuggerIconMap[car.state]);
      const defaultIcon = CarTuggerIconMap["离线"];
      return car ? url.pathname || defaultIcon : defaultIcon;
    }
    case "CarCarrier": {
      // CarCarrier  装载
      const url = handleChange(CarCarrierIconMap[car.state]);
      const defaultIcon = CarCarrierIconMap["离线"];
      return car ? url.pathname || defaultIcon : defaultIcon;
    }
    default: {
      // 其他、默认；牵引车
      const url = handleChange(CarTuggerIconMap[car.state]);
      const defaultIcon = CarTuggerIconMap["离线"];
      return car ? url.pathname || defaultIcon : defaultIcon;
    }
  }
};

export function useHook() {
  let dataBaseTask = ref([]);
  let SearchCode = ref("");
  let isRowClickActive = false;
  let carCode = "";
  let dataBaseAlarm = ref([]);
  let dataBaseCarData = ref([]);
  let dataBaseCarStatus = ref([]);
  let dataBaseLowBattery = ref([]);
  const translateX = ref(0);
  const translateY = ref(0);
  const dataList = ref();
  const visible = ref(false);
  const isMonitoring = ref(false);
  //**是否展示车辆 */
  let isShowCar = ref(true);
  const map = ref();
  let carInfoArr = [];
  let textInfoArr = [];
  const draggableData = ref({
    drawer4Open: false,
    dialogTitle: "",
    info: {},
    mainTitle: ""
  });
  const route = useRoute();
  const router = useRouter();
  /** 刷新路由 */
  function onFresh() {
    const { fullPath, query } = unref(route);
    router.replace({
      path: "/redirect" + fullPath,
      query
    });
    handleAliveRoute(route as ToRouteType, "refresh");
  }
  function setShowCarFn() {
    isShowCar.value = !isShowCar.value;
  }

  async function getMapFn() {
    const { data } = await GetMap();
    map.value = data.map;
    map.value.fileContent = JSON.parse(map.value.fileContent);
    if (map.value?.fileContent?.map?.extends?.[0]?.value) {
      map.value.fileContent.map.extends[0].value = JSON.parse(
        map.value.fileContent.map.extends[0].value
      );
    }
  }
  const handleButtonClick = event => {
    let target = event.target;
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
        case "SearchMonitoring":
          SearchMonitoring();
          break;
        case "startMonitoring":
          startMonitoring();
          break;
        case "stopMonitoring":
          stopMonitoring();
          break;
        case "mapAdaptive":
          mapAdaptive();
          break;
        case "map100":
          map100();
          break;
        case "switchInformation":
          switchInformation();
          break;
        case "toggleTrajectory":
          toggleTrajectory();
          break;
        case "mapZoomIn":
          mapZoomIn();
          break;
        case "mapShrinks":
          mapShrinks();
          break;
        case "moveTop":
          moveTop();
          break;
        case "moveBottom":
          moveBottom();
          break;
        case "moveRight":
          moveRight();
          break;
        case "moveLeft":
          moveLeft();
          break;
        case "switchShowCar":
          setShowCarFn();
        default:
          console.log("Unknown action:", action);
      }
    }
  };
  function mapZoomIn() {
    page.panel.setScaleUp();
  }
  function mapShrinks() {
    page.panel.setScaleDown();
  }
  function mapAdaptive() {
    page.panel.setScalingAdapt();
  }
  function map100() {
    page.panel.setScalingNormal();
  }
  function switchInformation() {
    page.panel.setSwitchInfo();
  }
  function toggleTrajectory() {
    page.panel.setSwitchCarRoute();
  }
  function moveTop() {
    translateY.value = translateY.value - 200;
    page.panel.translate.y = translateY.value;
  }
  function moveBottom() {
    translateY.value = translateY.value + 200;
    page.panel.translate.y = translateY.value;
  }
  function moveLeft() {
    translateX.value = translateX.value - 200;
    page.panel.translate.x = translateX.value;
  }
  function moveRight() {
    translateX.value = translateX.value + 200;
    page.panel.translate.x = translateX.value;
  }
  // @ 接收传递过来的信息
  // 存在创建车辆之后会一直加载车辆图片的问题，影响效能

  // const worker = new Worker(new URL("./worker.ts", import.meta.url));

  // worker.onmessage = event => {
  //   const { type, ...data } = event.data;
  //   switch (type) {
  //     case "updateCarInfo":
  //       page.panel.addOrUpdateOrDeleteCars(data.carInfoArr);
  //       break;
  //     case "updateTasks":
  //       dataBaseTask.value = data.tasks;
  //       break;
  //     case "updateAlarms":
  //       dataBaseAlarm.value = data.alarms;
  //       break;
  //     case "updateCars":
  //       dataBaseCarData.value = data.cars;
  //       dataBaseCarStatus.value = data.status;
  //       break;
  //     default:
  //       break;
  //   }
  //   // console.log("11111111111111111111");
  // };
  // 向 Worker 发送数据
  // function processIncomingData(data) {
  //   worker.postMessage({ method: "processData", data });
  // }
  // const getDatatoShow = async data => {
  //   worker.postMessage({
  //     method: "processData",
  //     data,
  //     carCode,
  //     map: JSON.stringify(map.value)
  //   });
  //   await send("Update");
  // };
  // 处理s端主动禁用站点用
  const lockNode = ref([]);
  const storage = ref([]);
  let oldLockNode = []; // 用来存储之前的值

  watch(
    () => lockNode.value.length,
    async (newValue, oldValue) => {
      console.log(
        "---------------------------------当前被禁用的点个数",
        lockNode.value.length
      );
      // console.log("禁用点个数：", lockNode.value.length)
      // 找到 newValue 中增加的项（存在于 newValue 但不存在于 oldLockNode 中）
      const addedItems = lockNode.value.filter(
        newItem => !oldLockNode.some(oldItem => oldItem.id === newItem.id)
      );
      console.log("新增禁用点:", addedItems);
      if (addedItems.length > 0) {
        for (let i = 0; i < addedItems.length; i++) {
          if (addedItems[i].type == "NodeRect") {
            console.log(addedItems[i]);
            page.panel.updateNode({
              type: "NodeRect",
              code: { text: addedItems[i].code },
              base: { lineWidth: "100", strokeStyle: "#ff0000" }
            });
          }
        }
      }
      const removedItems = oldLockNode.filter(
        oldItem => !lockNode.value.some(newItem => newItem.id === oldItem.id)
      );
      console.log("删除禁用点:", removedItems);
      // 更新启用的样式
      if (removedItems.length > 0) {
        for (let i = 0; i < removedItems.length; i++) {
          // let { data } = await GetSideInfo(removedItems[i].code)
          changeNodesState2(removedItems[i].code, removedItems[i]);
          // if (removedItems[i].type == "NodeRect") {
          //   page.panel.updateNode({
          //     type: "NodeRect",
          //     code: { text: removedItems[i].code },
          //     base: {
          //       fillStyle: "#808080",
          //       lineWidth: "1",
          //       strokeStyle: "#ffffff"
          //     }
          //   });
          // } else {
          //   page.panel.updateNode({
          //     type: "NodeEllipse",
          //     code: { text: removedItems[i].code },
          //     base: { fillStyle: "#FFFFFF" }
          //   });
          // }
        }
      }
      // 更新 oldLockNode 为当前的 lockNode 以便下次比较
      oldLockNode = [...lockNode.value];
    }
  );
  watch(
    () => storage.value,
    (newVal, oldVal) => {
      // console.log("锁定库位")
      // console.log(storage.value)
      changeStorageState(storage.value);
      for (let i = 0; i < storage.value.length; i++) {
        const node = newVal[i];
        const code = storage.value[i].nodeCode;
        let baseConfig = {};

        if (node.isLock) {
          // 站点禁用、库位锁定
          baseConfig = { fillStyle: "#b75cc4", strokeStyle: "#ff0000" };

          // 统一更新节点
          page.panel.updateNode({
            type: "NodeRect",
            code: { text: code },
            base: baseConfig
          });
        }
      }
    }
  );
  function getMinEdge(w, h) {
    if (!w || !h || w == 0 || h == 0) {
      return "275px Arial";
    }
    let data;
    if (w > h) {
      data = h / 1.0;
    } else {
      data = w / 1.0;
    }
    data += "px Arial";
    return data;
  }
  function getMinPointOffset(w, h) {
    if (!w || !h || w == 0 || h == 0) {
      return 0;
    }
    let data;
    if (w > h) {
      data = h;
    } else {
      data = w;
    }
    return data / 10;
  }
  const getDatatoShow = async data => {
    lockNode.value = data.lockNode;
    storage.value = data.storage;
    // storage.value = data.storage
    //   .filter(item => item.isLock)
    //   .map(item => item);
    // console.log(lockNode.value)
    // console.log(data.storage)
    if (data && typeof data === "object") {
      changeNodesState(data.lockNode);
      changeStorageState(data.storage); // 站点样式
    }
    if (
      data &&
      typeof data === "object" &&
      data?.map &&
      data?.map?.length &&
      isMonitoring?.value
    ) {
      carInfoArr = [];
      textInfoArr = [];
      for (let i = 0; i < data.map.length; i++) {
        let item = data.map[i];
        if (item?.base?.carEdges && item?.base?.carEdges?.length) {
          item?.base?.carEdges.forEach(Edge => {
            if (Edge.type === "Prev") Edge.visible = false;
          });
        }
        if (item?.base?.carNodes && item?.base?.carNodes?.length) {
          item?.base?.carNodes.forEach(Node => {
            if (Node.type === "Prev") Node.visible = false;
          });
        }
        // carCode = event.code.text

        let carInfo = {
          type: item.type,
          code: {
            text: item.code.text,
            visible: false
          },
          image: {
            src: getCarIconSrc(data?.car, item.code.text, item.type)
          },
          base: {
            point: { x: 0, y: -500 },
            // @ts-expect-error
            rotate: -item.base.rotate - Math.PI / 2 ?? 0,
            carEdges: [],
            carNodes: [],
            // size: item.base.size
            size: {
              w: item.base?.size?.w === 0 ? 900 : item.base?.size?.w,
              // h: item.base?.size?.h === 0 ? 455 : item.base?.size?.h
              h: item.base?.size?.h === 0 ? 900 : item.base?.size?.w // 图片换为1:1尺寸，长宽需要保持一致
            }
          },
          name: {
            text: `${item.name.text} (${item.base.status.manualMode})`,
            visible: true,
            font: getMinEdge(item.base?.size?.w, item.base?.size?.h),
            // font: "12px Times New Roman",
            scaling: false // 自动缩放
          },
          status: item.base.status.manualMode
        };

        // console.log(data.map[0].base.status.manualMode); // 手动/自动模式状态
        if (!carCode || carCode === "") {
          carInfo.base.carEdges = (item?.base?.carEdges || []).filter(
            (value, index, self) =>
              index === self.findIndex(t => t.id === value.id)
          );
          carInfo.base.carNodes = (item?.base?.carNodes || []).filter(
            (value, index, self) =>
              index === self.findIndex(t => t.id === value.id)
          );
        } else {
          if (carCode === item.code.text) {
            carInfo.base.carEdges = (item?.base?.carEdges || []).filter(
              (value, index, self) =>
                index === self.findIndex(t => t.id === value.id)
            );
            carInfo.base.carNodes = (item?.base?.carNodes || []).filter(
              (value, index, self) =>
                index === self.findIndex(t => t.id === value.id)
            );
          }
        }
        if (map.value?.fileContent?.map?.extends?.[0]?.value) {
          // let value = map.value?.fileContent?.map?.extends?.[0]?.value;
          // let normalNode = value?.normNodeW ?? 0;
          // carInfo.base.point.x = item.base.point.x + value.AbsMinX + normalNode;
          // carInfo.base.point.y =
          //   item.base.point.y * -1 + value.AbsMinY + normalNode;
          // 偏转计算交由后端计算
          carInfo.base.point.x = item.base.point.x;
          carInfo.base.point.y = item.base.point.y;
        } else {
          carInfo.base.point.x = item.base.point.x;
          carInfo.base.point.y = item.base.point.y;
          carInfo.base.rotate = item.base.rotate;
        }

        if (isShowCar.value) {
          carInfoArr.push(carInfo);
        }
      }
      // console.log(
      //   "newCarInfo is",
      //   // item.base.point.x,
      //   // item.base.point.y,
      //   "\n",
      //   carInfoArr
      // );
      // page.panel.addOrUpdateOrDeleteCars(carInfoArr);
      // console.log("carInfoArr is", carInfoArr);
      page.cars = carInfoArr;
      // for (let i = 0; i < map.value.fileContent.tags.length; i++) {
      //   console.log(map.value.fileContent.tags[i]);
      //   let item = map.value.fileContent.tags[i];
      //   textInfoArr.push(item);
      // }
      // page.text = textInfoArr;
      // [
      //   {
      //     type: "TagRect",
      //     code: { text: 124 },
      //     name: {
      //       text: 1234,
      //       font: "500px Arial"
      //     },
      //     base: {
      //       point: {
      //         x: 100,
      //         y: 10
      //       },
      //       fillStyle: "rgb(255, 255, 255)", // 填充颜色
      //       strokeStyle: "rgb(255, 255, 255)" // 边框颜色
      //     }
      //   }
      // ];

      await send("Update");
    }
    try {
      if (
        data &&
        typeof data === "object" &&
        data?.task &&
        data?.task?.length
      ) {
        dataBaseTask.value = data.task;
        // console.log("dataBaseTask is", dataBaseTask.value);
      }

      if (
        data &&
        typeof data === "object" &&
        data?.alarm &&
        data?.alarm?.length >= 0
      ) {
        // 存储警报数据
        dataBaseAlarm.value = data.alarm;
      }

      if (
        data &&
        typeof data === "object" &&
        data?.lowBatteryCars &&
        data?.lowBatteryCars?.length
      ) {
        dataBaseLowBattery.value = data.lowBatteryCars;
        // console.log("dataBaseLowBattery is", dataBaseLowBattery.value);
      }

      if (data && typeof data === "object" && data?.car && data?.car?.length) {
        // 存储车辆数据
        dataBaseCarData.value = data.car;
        dataBaseCarStatus.value = data.status;
      }
    } catch (error) {
      // console.error("Error processing data update:", error);
    }
  };

  const page: any = {
    timerId: null,
    canvas: {},
    panel: {},
    // box: {},
    cars: [],
    text: [],
    prevTime: -1,
    deltaTime: 50,
    setPanelRenderAction: currTime => {
      if (page.prevTime === -1) page.prevTime = currTime;
      if (currTime - page.prevTime > page.deltaTime) {
        page.panel.addOrUpdateOrDeleteCars(page.cars);
        page.panel.addOrUpdateOrDeleteTags(page.text);
        page.prevTime = currTime;
      }
    },
    init: async () => {
      if (page.panel?.initialized) {
        page.destroyPanel();
      } // 如果 panel 已经初始化过，调用自定义销毁
      // page.box = document.getElementById("boxPanel");
      page.canvas = document.getElementById("canvas");
      // @ts-expect-error

      // page.panel = await new Panel(page.box, page.canvas, {
      //   map: { base: { size: { w: 13000, h: 9000 } } },
      //   mode: "Read",
      //   // part: false,
      //   license: { activationCode }
      // }).init();
      page.panel = await new Panel(page.canvas, {
        map: { base: { size: { w: 13000, h: 9000 } } },
        mode: "Read",
        // zoom: { minW: 0.3, minH: 0.3 },
        zoom: { enable: false },
        license: { activationCode: activationCode },
        scaling: {
          // code: {enable: true },
          // name: { enable: true}
        },
        view: {
          enable: false
        }
      }).init();
      page.panel.initialized = true; // 自定义标记，表示已经初始化过
      page.bind();
    },

    bind: () => {
      page.panel.panelRenderAction = (time: any) =>
        page.setPanelRenderAction(time);
      page.panel.panelActiveAction = _ => {
        carCode = "";
      };
      page.panel.nodeRectActiveEnterAction = event => {
        carCode = "";
        page.readSiteInfo(event, "站点");
      };
      page.panel.nodeActiveLeaveAction = _ => {
        // 站点激活离开时触发，隐藏弹窗
        draggableData.value.drawer4Open = false;
      };
      page.panel.carActiveLeaveAction = _ => {
        draggableData.value.drawer4Open = false;
      };
      page.panel.edgeActiveLeaveAction = _ => {
        draggableData.value.drawer4Open = false;
      };
      page.panel.edgeBeelineActiveLeaveAction = _ => {
        draggableData.value.drawer4Open = false;
      };
      page.panel.edgeQuadraticActiveLeaveAction = _ => {
        draggableData.value.drawer4Open = false;
      };
      page.panel.edgeBezierActiveLeaveAction = _ => {
        draggableData.value.drawer4Open = false;
      };
      page.panel.nodeEllipseActiveEnterAction = event => {
        carCode = "";
        page.readSiteInfo(event, "站点");
      };
      page.panel.edgeBeelineActiveEnterAction = event => {
        carCode = "";
        page.readSiteInfo(event, "路线");
      };
      page.panel.carActiveEnterAction = event => {
        carCode = event.code.text;
        //console.log(event);
        page.readSiteInfo(event, "车辆");
      };
      page.panel.edgeArcActiveEnterAction = event => {
        carCode = "";
        page.readSiteInfo(event, "路线");
      };
      page.panel.edgeQuadraticActiveEnterAction = event => {
        carCode = "";
        page.readSiteInfo(event, "路线");
      };
      page.panel.edgeBezierActiveEnterAction = event => {
        carCode = "";
        page.readSiteInfo(event, "路线");
      };
      // window.addEventListener("resize", event => page.panel.setResize());
    },
    destroyPanel: () => {
      //自定义销毁
      if (page.panel) {
        // 解除绑定的事件处理器
        page.panel.panelRenderAction = null;
        page.panel.panelActiveAction = null;
        page.panel.nodeRectActiveEnterAction = null;
        page.panel.nodeEllipseActiveEnterAction = null;
        page.panel.edgeBeelineActiveEnterAction = null;
        page.panel.carActiveEnterAction = null;
        page.panel.edgeArcActiveEnterAction = null;
        page.panel.edgeQuadraticActiveEnterAction = null;
        page.panel.edgeBezierActiveEnterAction = null;
        // 清空 panel 的引用
        page.panel = null;
      }
    },
    readSiteInfo: (event, msg) => {
      console.log("???", event, msg);
      if (isRowClickActive) return false;
      if (draggableData.value.drawer4Open) return;
      if (msg === "车辆") {
        console.log("车辆");
        localStorage.setItem("carCode", event.code.text);
      }
      if (msg === "站点") {
        nodeEllipseForm.base = event.base;
        nodeEllipseForm.code = event.code;
        nodeEllipseForm.name = event.name;
        nodeEllipseForm.data.nodeDescription = event.data.nodeDescription;
        nodeEllipseForm.lock.enable = event.lock.enable;
        nodeEllipseForm.data.released = event.data.released;
        nodeEllipseForm.type = event.type;
        if (event?.fields) {
          nodeEllipseForm.fields = event?.fields;
        }
        isWatchChange.value = false;
        cloneNodeEllipseForm = stringify(nodeEllipseForm);
        setTimeout(() => {
          isWatchChange.value = true;
        });
      }

      // h(MessageBox, { modelValue: true, onClose: close, event });
      // nextTick(() => {
      draggableData.value.drawer4Open = false;
      draggableData.value.drawer4Open = true;
      draggableData.value.dialogTitle = msg;
      draggableData.value.info = event;
      draggableData.value.mainTitle = event.code.text;
      // });
    },

    startMonitor: async () => {
      isMonitoring.value = true;
      await connect();
      setupSignalRHandlers(getDatatoShow);
    },
    stopMonitor: async () => {
      isMonitoring.value = false;
      await disconnect(() => {
        // page.panel.panelRenderAction = undefined; //画板渲染时触发
        // page.panel.setDeleteImage(); // 设置删除地图图片
        // page.panel.initData();
        // page.panel.setScalingAdapt(); // 地图自适应
        // page.panel.addOrUpdateOrDeleteCars([]);
        page.cars = [];
        // page.text = [];
      });
    }
  };
  const SearchMonitoring = () => {
    // const search = document.getElementById("textSearch").value;
    if (!SearchCode.value) {
      message("请输入编码/名称！", { type: "warning" });
      return;
    }
    const shape = page.panel.allShapes.find(
      e => e.code.text === SearchCode.value || e.name.text === SearchCode.value
    );
    console.log("SearchCode is", SearchCode.value, "\n", page.panel);
    if (!shape) {
      message("未搜索到对象！", { type: "warning" });
      return;
    }
    page.panel.setViewAndActive(shape);
  };
  const startMonitoring = async () => {
    lockNodeLength.value = -1;
    // stopMonitoring();
    await getMapFn(); // map
    // 替换base属性值的函数 地图路径颜色
    // 绘制地图
    console.log("绘制地图", map.value.fileContent);
    page.panel.setData(map.value.fileContent);
    // 解决标签异常消失问题
    for (let i = 0; i < map.value.fileContent.tags.length; i++) {
      console.log(map.value.fileContent.tags[i]);
      let item = map.value.fileContent.tags[i];
      textInfoArr.push(item);
    }
    page.text = textInfoArr;
    page.startMonitor();
    page.panel.setScalingAdapt();
  };
  const stopMonitoring = async () => {
    console.log("停止");
    lockNodeLength.value = 0;
    isMonitoring.value = false;
    page.stopMonitor();
    page.init(); //初始化Canvas
    setTimeout(() => {
      onFresh();
    }, 0);
  };

  const handleRowDoubleClick = row => {
    // Perform logic based on the row data
    isRowClickActive = true;
    const predicate = e => e.kind === "Car" && e.code.text === row.code;
    const shape = page.panel.allShapes.find(predicate);
    if (shape) {
      page.panel.setViewAndActive(shape);
    }
    console.log("Row double clicked:", row, "page is", page);
    // const predicate = e => e.kind === "Car" && e.code.text === row.code;
    // page.panel.setActive(predicate);
    // page.panel.setView(predicate);
    isRowClickActive = false;
  };

  // 自动站点禁用样式
  const changeNodesState = lockNode => {
    // if (lockNode.length == lockNodeLength.value) return;
    if (lockNode && lockNode.length) {
      for (let i = 0; i < lockNode.length; i++) {
        const element = lockNode[i];
        page.panel.updateNode({
          type: "NodeEllipse",
          code: { text: element.code },
          base: { fillStyle: "#eaeaea", lineWidth: 200, strokeStyle: "#ff0000" }
        });
      }
      lockNodeLength.value = lockNode.length;
    }
  };
  // 手动禁用样式
  const changeNodesState3 = (code, data) => {
    // page.panel.updateNode({
    //   type: data.type,
    //   code: { text: code },
    //   base: { fillStyle: "#A3A3A3" }
    // });
  };
  // 站点启用样式
  function changeNodesState2(code, data) {
    if (data.name?.text === "充电站") {
      page.panel.updateNode({
        type: data.type,
        code: { text: code },
        base: { fillStyle: "#77d642" }
      });
    } else {
      console.log("自动更新解锁站点");
      page.panel.updateNode({
        type: data.type,
        code: { text: code },
        base: { fillStyle: "#FFFFFF", lineWidth: 30, strokeStyle: "#7224d0" }
      });
    }
  }
  function findDifferences(newObj, oldObj, prefix = "", whitelist = []) {
    let changes = [];
    for (const key in newObj) {
      if (newObj.hasOwnProperty(key)) {
        const newPath = prefix ? `${prefix}.${key}` : key;

        // 检查路径是否包含白名单中的字符串
        const isWhitelisted = whitelist.some(whitelistedPath =>
          newPath.includes(whitelistedPath)
        );
        if (isWhitelisted) {
          continue; // 跳过白名单中的路径
        }

        if (typeof newObj[key] === "object" && newObj[key] !== null) {
          if (oldObj[key] === undefined || oldObj[key] === null) {
            // 如果旧对象中不存在这个键，说明是新增的属性
            changes.push({ path: newPath, value: newObj[key] });
          } else {
            changes = changes.concat(
              findDifferences(newObj[key], oldObj[key], newPath, whitelist)
            );
          }
        } else if (newObj[key] !== oldObj[key]) {
          changes.push({ path: newPath, value: newObj[key] });
        }
      }
    }
    return changes;
  }
  let count = store.GET_COUNT;
  watchEffect(() => {
    if (count == store.GET_COUNT) return;
    count = store.GET_COUNT;
    if (!draggableData.value.drawer4Open) return; // 防止弹窗关闭后继续执行
    if (!store.GET_ISENSBLE && nodeEllipseForm.code.text) {
      nodeEllipseForm.lock.enable = store.GET_ISENSBLE;
      changeNodesState2(nodeEllipseForm.code.text, nodeEllipseForm);
    } else if (store.GET_ISENSBLE && nodeEllipseForm.code.text) {
      nodeEllipseForm.lock.enable = store.GET_ISENSBLE;
      changeNodesState3(nodeEllipseForm.code.text, nodeEllipseForm);
    }
  });
  // watch(
  //   () => store.GET_COUNT,
  //   () => {
  //     console.log(store.GET_COUNT, store.GET_ISENSBLE);

  //     console.log(
  //       "站点属性调整",
  //       !store.GET_ISENSBLE,
  //       nodeEllipseForm.code.text
  //     );
  //     if (store.GET_ISENSBLE && nodeEllipseForm.code.text) {
  //       nodeEllipseForm.lock.enable = store.GET_ISENSBLE;
  //     }
  //     if (!store.GET_ISENSBLE && nodeEllipseForm.code.text) {
  //       changeNodesState2(nodeEllipseForm.code.text);
  //     }
  //   }
  // );

  // 监听站点
  watch(
    () => nodeEllipseForm,
    // () => nodeRectForm.shape.base,
    (newVal, _) => {
      // console.log("站点属性调整", newVal);
      if (!isWatchChange.value) return;
      let oldObj = parse(cloneNodeEllipseForm);
      const changes = findDifferences(newVal, oldObj);
      // console.log("旧对象", oldObj);
      changes.forEach(({ path, value }) => {
        page.panel.setActiveShapeParam(path, value, "NodeEllipse");
      });
      cloneNodeEllipseForm = stringify(newVal);
    },
    { deep: true } // 深度监听对象的属性变化
  );
  const changeStorageState = storage => {
    if (storage && storage.length) {
      for (let index = 0; index < storage.length; index++) {
        const element = storage[index];
        // console.log(element.isLock)
        if (!element.isLock) {
          page.panel.updateNode({
            type: "NodeRect",
            code: { text: element.nodeCode },
            base: { fillStyle: tagList[element.state].fillStyle }
          });
        }
      }
    }
  };
  const tagList = {
    FullContainer: { text: "满桶", fillStyle: "#42c456" },
    EmptyContainer: { text: "空桶", fillStyle: "#11aaea" },
    HasTag: { text: "只有盖子", fillStyle: "#b75cc4" },
    NoneContainer: { text: "空库位", fillStyle: "#cccccc" }
  };
  /// todo:
  // 1. 总开关 ，是否开启路线轨迹 isShowEdge Boolean
  // 2. 分开关 ，点击之后获取code，String

  onMounted(async () => {
    page.init(); //初始化Canvas
  });
  onUnmounted(() => {
    lockNodeLength.value = 0;
    // stopSendingUpdates();
    disconnect(() => {
      console.log("卸载WS！！.");
    });
    page.stopMonitor();
  });
  return {
    SearchCode,
    dataBaseTask,
    dataBaseAlarm,
    dataBaseCarData,
    dataBaseCarStatus,
    dataBaseLowBattery,
    handleButtonClick,
    isMonitoring,
    dataList,
    visible,
    page,
    draggableData,
    handleRowDoubleClick,
    startMonitoring
  };
}
