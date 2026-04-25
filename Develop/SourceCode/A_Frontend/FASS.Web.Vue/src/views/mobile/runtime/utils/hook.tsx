import { ref, onMounted, nextTick, onUnmounted } from "vue";
import { GetMap } from "@/api/monitor/runtime";
import leisure from "../../../../assets/monitor/叉车-空闲中.png";
import task from "../../../../assets/monitor/叉车-任务中.png";
import abnormal from "../../../../assets/monitor/叉车-异常.png";
import offline from "../../../../assets/monitor/叉车-离线.png";
import charge from "../../../../assets/monitor/叉车-充电.png";
import {
  connect,
  disconnect,
  setupSignalRHandlers,
  send
} from "@/api/useMessageHub";
import { ActivationCodeKey } from "@/utils/auth";
const activationCode = localStorage.getItem(ActivationCodeKey);
const translateX = ref(0);
const translateY = ref(0);
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
    离线: "叉车-离线",
    空闲: "叉车-空闲中",
    任务中: "叉车-任务中",
    "任务中[空载]": "叉车-任务中",
    "任务中[满载]": "叉车-任务中-满载",
    充电中: "叉车-充电",
    异常: "叉车-异常"
  };
  const CarConveyorIconMap = {
    离线: "辊筒-离线",
    空闲: "辊筒-空闲中",
    任务中: "辊筒-任务中不带载",
    "任务中[空载]": "辊筒-任务中不带载",
    "任务中[满载]": "辊筒-任务中带载",
    充电中: "辊筒-充电",
    异常: "辊筒-异常"
  };
  const CarTuggerIconMap = {
    离线: "牵引车-离线",
    空闲: "牵引车-空闲中",
    任务中: "牵引车--任务中不带载",
    "任务中[空载]": "牵引车--任务中不带载",
    "任务中[满载]": "牵引车--任务中带载",
    充电中: "牵引车-充电",
    异常: "牵引车-异常"
  };
  const CarCarrierIconMap = {
    离线: "装载-离线",
    空闲: "装载-空闲",
    任务中: "装载-任务中不带载",
    "任务中[空载]": "装载-任务中不带载",
    "任务中[满载]": "装载-任务中带载",
    充电中: "装载-充电",
    异常: "装载-异常"
  };

  // if (type && type === "CarForklift") {
  //   // 默认的离线图标
  //   const defaultIcon = CarForkliftIconMap["离线"];
  //   // 查找对应的车辆数据
  //   const car = carData.find(car => car.code === code);
  //   // 如果找到对应的车辆，返回相应的图标，否则返回离线图标
  //   return car ? CarForkliftIconMap[car.state] || defaultIcon : defaultIcon;
  // }
  function handleChange(val) {
    return new URL(`../../../../assets/monitor/${val}.png`, import.meta.url);
  }
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
  let isRowClickActive = false;
  let carCode = "";
  let dataBaseAlarm = ref([]);
  let dataBaseCarData = ref([]);
  let dataBaseCarStatus = ref([]);
  const dataList = ref();
  const visible = ref(false);
  const isMonitoring = ref(false);
  const map = ref();
  let carInfoArr = [];
  const draggableData = ref({
    drawer4Open: false,
    dialogTitle: "",
    info: {},
    mainTitle: ""
  });

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

  const getDatatoShow = async data => {
    if (
      data &&
      typeof data === "object" &&
      data?.map &&
      data?.map?.length &&
      isMonitoring?.value
    ) {
      changeStorageState(data.storage);
      // console.log("data is", "\n", data);
      carInfoArr = [];
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
          // "CarrierState": sensor[i].CarrierBasic.CarrierState===1?"上线":'离线',
          type: item.type,
          code: { text: item.code.text },
          image: { src: getCarIconSrc(data?.car, item.code.text, item.type) },
          base: {
            point: { x: 0, y: 0 },
            rotate: -item.base.rotate ?? 0,
            carEdges: [],
            carNodes: [],
            // size: item.base.size
            size: {
              w: item.base?.size?.w === 0 ? 455 : item.base?.size?.w,
              h: item.base?.size?.h === 0 ? 900 : item.base?.size?.h
            }
          }
        };
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
          carInfo.base.point.x = item.base.point.x;
          carInfo.base.point.y = item.base.point.y;
        } else {
          carInfo.base.point.x = item.base.point.x;
          carInfo.base.point.y = item.base.point.y;
          carInfo.base.rotate = item.base.rotate;
        }
        carInfoArr.push(carInfo);
      }
      // console.log(
      //   "newCarInfo is",
      //   // item.base.point.x,
      //   // item.base.point.y,
      //   "\n",
      //   carInfoArr
      // );
      // page.panel.addOrUpdateOrDeleteCars(carInfoArr);
      page.cars = carInfoArr;
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
        data?.alarm?.length
      ) {
        // 存储警报数据
        dataBaseAlarm.value = data.alarm;
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
    box: {},
    cars: [],
    prevTime: -1,
    deltaTime: 50,
    setPanelRenderAction: currTime => {
      if (page.prevTime === -1) page.prevTime = currTime;
      if (currTime - page.prevTime > page.deltaTime) {
        page.panel.addOrUpdateOrDeleteCars(page.cars);
        page.prevTime = currTime;
      }
    },
    init: async () => {
      page.box = document.getElementById("boxPanel");
      page.canvas = document.getElementById("canvas");
      page.panel = await new Panel(page.box, page.canvas, {
        map: { base: { size: { w: 13000, h: 9000 } } },
        mode: "Read",
        license: { activationCode }
      }).init();
      page.bind();
    },
    bind: () => {
      page.panel.panelRenderAction = (time: any) =>
        page.setPanelRenderAction(time);
      page.panel.panelActiveAction = event => {
        carCode = "";
      };
      page.panel.nodeRectActiveEnterAction = event => {
        carCode = "";
        page.readSiteInfo(event, "站点");
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
    readSiteInfo: (event, msg) => {
      // console.log("站点", event, msg);
      if (isRowClickActive) return false;
      // h(MessageBox, { modelValue: true, onClose: close, event });
      nextTick(() => {
        draggableData.value.drawer4Open = false;
        draggableData.value.drawer4Open = true;
        draggableData.value.dialogTitle = msg;
        draggableData.value.info = event;
        draggableData.value.mainTitle = event.code.text;
      });
    },
    startMonitor: async () => {
      isMonitoring.value = true;
      await connect();
      setupSignalRHandlers(getDatatoShow);
    },
    stopMonitor: async () => {
      isMonitoring.value = false;
      await disconnect(() => {
        page.panel.setClearShapes();
        page.panel.setScalingAdapt();
        // page.panel.addOrUpdateOrDeleteCars([]);
        page.cars = [];
      });
    }
  };
  const startMonitoring = async () => {
    await getMapFn(); // map
    // 替换base属性值的函数 地图路径颜色
    // 绘制地图
    page.panel.setData(map.value.fileContent);
    // console.log("绘制地图", map.value.fileContent);
    page.startMonitor();
    page.panel.setScalingAdapt();
  };
  const stopMonitoring = async () => {
    console.log("停止");
    isMonitoring.value = false;
    page.stopMonitor();
  };

  const handleRowDoubleClick = row => {
    // Perform logic based on the row data
    // console.log("page is", page, "\n", "row is", row);
    isRowClickActive = true;
    const predicate = e => e.kind === "Car" && e.code.text === row.code;
    page.panel.setActive(predicate);
    page.panel.setView(predicate);
    isRowClickActive = false;
  };

  const changeStorageState = storage => {
    if (storage && storage.length) {
      for (let index = 0; index < storage.length; index++) {
        const element = storage[index];
        // const node2 =
        page.panel.updateNode({
          type: "NodeRect",
          code: { text: element.nodeCode },
          base: { fillStyle: tagList[element.state].fillStyle }
        });
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
    await page.init(); //初始化Canvas
    startMonitoring(); //开始监控
  });

  onUnmounted(() => {
    page.stopMonitor();
  });
  return {
    dataBaseTask,
    dataBaseAlarm,
    dataBaseCarData,
    dataBaseCarStatus,
    handleButtonClick,
    isMonitoring,
    dataList,
    visible,
    page,
    draggableData,
    handleRowDoubleClick
  };
}
