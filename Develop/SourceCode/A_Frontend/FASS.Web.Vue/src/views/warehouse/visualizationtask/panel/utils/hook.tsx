import { ref, onMounted, reactive, watch, unref } from "vue";
// import { ref, unref } from 'vue';
import { useRoute, useRouter } from "vue-router";
import { handleAliveRoute, getTopMenu } from "@/router/utils";

import {
  StorageGetPage,
  SaveApi,
  ResetAll
} from "@/api/warehouse/visualizationTask";
import { GetAreaList } from "@/api/warehouse/visualization";
import konva from "konva";
import { ElMessage, ElMessageBox } from "element-plus";
export function useHook() {
  const tagData = ref([]);
  const selectedItemId = ref();
  let cardStates = ref();
  const areaList = ref([]);

  const newFormInline = ref();
  interface SketchpadLocation {
    defaultx: number;
    defaulty: number;
  }

  interface SketchpadConfig {
    sketchpadScale: number;
    sketchpadLocation: SketchpadLocation;
  }

  interface Sketchpad {
    [key: string]: SketchpadConfig; // 使用索引签名定义任意键                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
  }

  let stage; // 画板
  let rectangles = []; // 存储矩形元素
  let sketchpadScale; // 舞台缩放
  let dataList = ref([]);
  let draggable = ref(true);
  let isTrue = ref(false); // 编辑模式
  let isKw = ref(false); // 是否是库位
  let defaultx = ref(0); // 画板默认偏移
  let defaulty = ref(0);
  let sketchpad: Sketchpad = {};
  let newPos = null;
  let initPos = null;
  let offsetCoordinate = ref([0, 0]); // 库位偏移
  let coefficient = ref(0.2); // 系数（间距）
  let textX = ref(10); // 文本偏移系数
  let textY = ref(40);
  let staging = ref(0); // 控制放弃编辑时候缩放倍数
  let width = 1920; // 画板宽高
  let height = 1080;
  let menuItem = "all";
  let clickTimeout;
  const color = {
    FullContainer: "#42c456",
    EmptyContainer: "#11aaea",
    HasTag: "#b75cc4",
    NoneContainer: "#cccccc"
  };
  const activeAreaId = ref("all");
  const pageParam = ref({});
  const query = reactive({
    code: "",
    name: ""
  });
  const pagination = reactive({
    total: 0,
    pageSize: 99999,
    currentPage: 1,
    background: true
  });
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
        order: [{ field: "code", sequence: "ASC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }
  let isFirstExecution = ref(true); // 标志变量
  watch(
    () => activeAreaId.value,
    () => {
      isFirstExecution.value = true;
    }
  );
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

  async function handleGetList(row) {
    menuItem = row;
    activeAreaId.value = row.index ? row.index : row;
    if (activeAreaId.value === "all") {
      activeAreaId.value = "";
    }
    pageParam.value = handlePageParam();
    const { data } = await StorageGetPage(activeAreaId.value, pageParam.value);
    dataList.value = data.rows;
    coefficient.value = dataList.value[0]?.coefficient
      ? dataList.value[0]?.coefficient
      : 0.2; // 系数（间距）
    offsetCoordinate.value = dataList.value[0]?.offsetCoordinate ?? [0, 0]; // 库位偏移
    textX.value = dataList.value[0]?.textCoordinate
      ? dataList.value[0]?.textCoordinate[0]
      : 0; // 文本偏移系数
    textY.value = dataList.value[0]?.textCoordinate
      ? dataList.value[0]?.textCoordinate[1]
      : 0;
    let a = dataList.value[0] ? (dataList.value[0].extend ? JSON.parse(dataList.value[0].extend) : null) : null;
    let b = a ? (a.sketchpad ? JSON.parse(a.sketchpad) : null) : null;
    if (menuItem == "") menuItem = "all";
    sketchpad = b ? b : null; // 如果后端没有返回该字段，证明是默认，给一个null值
    // console.log("上次保存到后端的数据", b);
    if (isFirstExecution.value && dataList.value[0]) {
      let topLeftCoordinate = findTopLeftCoordinate(dataList.value);
      // 舞台默认偏移，按照左上角的坐标进行偏移  ==》 如果后端有返回当前的舞台偏移值，则使用后端的偏移值，否则默认
      if (sketchpad && sketchpad[menuItem]) {
        defaultx.value =
          sketchpad == null
            ? (-(topLeftCoordinate.coordinate[0] + offsetCoordinate.value[0]) /
              10) *
            coefficient.value +
            20
            : sketchpad[menuItem].sketchpadLocation.defaultx;
        defaulty.value =
          sketchpad == null
            ? (-(topLeftCoordinate.coordinate[1] + offsetCoordinate.value[1]) /
              10) *
            coefficient.value +
            20
            : sketchpad[menuItem].sketchpadLocation.defaulty;
        sketchpadScale = sketchpad[menuItem].sketchpadScale
          ? sketchpad[menuItem].sketchpadScale
          : 1;
        newPos = {
          x: defaultx.value,
          y: defaulty.value
        };
        // console.log(sketchpad[menuItem]);
      } else {
        defaultx.value =
          (-(topLeftCoordinate.coordinate[0] + offsetCoordinate.value[0]) /
            10) *
          coefficient.value +
          20;
        defaulty.value =
          (-(topLeftCoordinate.coordinate[1] + offsetCoordinate.value[1]) /
            10) *
          coefficient.value +
          20;
      }

      newPos = {
        x: defaultx.value,
        y: defaulty.value
      };
      isFirstExecution.value = false;
    }
    canvasInit();
    initPos = newPos;
  }
  function findTopLeftCoordinate(arr) {
    return arr.reduce((topLeft, item) => {
      const [x, y] = item.coordinate;
      const [minX, minY] = topLeft.coordinate;
      // 如果 x 坐标更小，或者 x 相同时 y 坐标更小，就更新 topLeft
      if (x < minX || (x === minX && y < minY)) {
        return item;
      }
      return topLeft;
    }, arr[0]);
  }
  async function GetAreaListFn() {
    const { data } = await GetAreaList();
    areaList.value = data;
    if (areaList.value.length) {
      await handleGetList("all");
    }
  }
  function handleSetStates(e) {
    cardStates.value = e.target.attrs.dataCode;
  }
  function handleResetStates() {
    cardStates.value = 1;
  }
  function handleEdit() {
    isTrue.value = !isTrue.value;
    if (isTrue.value) {
      staging.value = coefficient.value;
    } else {
      coefficient.value = staging.value;
      handleGetList(activeAreaId.value);
    }
    canvasInit();
  }
  function setSketchpad() {
    if (menuItem == "") menuItem = "all";
    if (!sketchpad) {
      sketchpad = {};
    }
    // console.log("第二次", Object.keys(sketchpad));
    sketchpad[menuItem] = {
      sketchpadScale,
      sketchpadLocation: {
        defaultx: defaultx.value,
        defaulty: defaulty.value
      }
    };
  }
  async function handleSave() {
    isTrue.value = !isTrue.value;
    sketchpadScale = stage.scaleX();
    dataList.value.forEach(item => {
      item.coefficient = coefficient.value;
      item.textCoordinate = [textX.value, textY.value];
      item.sketchpad = JSON.stringify(sketchpad);
    });
    for (let i = 0; i < locationArrx.length; i++) {
      if (locationArrx[i] === undefined) locationArrx[i] = 0;
      // console.log(dataList.value[i].offsetCoordinate);
      dataList.value[i].offsetCoordinate[0] = Number(
        dataList.value[i].offsetCoordinate[0] + locationArrx[i]
      );
    }
    for (let i = 0; i < locationArry.length; i++) {
      if (locationArry[i] === undefined) locationArry[i] = 0;
      dataList.value[i].offsetCoordinate[1] = Number(
        dataList.value[i].offsetCoordinate[1] + locationArry[i]
      );
    }
    await SaveApi(dataList.value);
    // console.log("保存成功的数据源", sketchpad);
    // console.log("保存成功的坐标", defaultx.value, defaulty.value);
    await handleGetList(activeAreaId.value);
    canvasInit();
    onFresh();
  }

  async function handleResetAll() {
    ElMessageBox.confirm("确定初始化坐标吗?", {
      confirmButtonText: "确定",
      cancelButtonText: "取消",
      type: "warning"
    })
      .then(async () => {
        isTrue.value = false;
        sketchpadScale = 1;
        canvasInit();
        await ResetAll();
        await handleGetList(activeAreaId.value);
        onFresh();
        ElMessage({
          type: "success",
          message: "初始化成功"
        });
      })
      .catch(() => { });
  }
  function handleZoomUp() {
    let zoomAmount = 0.05; // 缩放速度
    let oldScale = stage.scaleX(); // 获取缩放
    let pointer = stage.getPointerPosition();
    if (!pointer) pointer = { x: 110, y: 0 };
    let newScale = oldScale + zoomAmount;
    // 限制缩放范围
    if (newScale > 0.1 && newScale < 10) {
      if (isTrue.value) {
        sketchpadScale = newScale;
        setSketchpad();
      }
      stage.scale({ x: newScale, y: newScale }); // 针对整个 stage 设置新的缩放比例
      // 根据缩放比例调整舞台位置，使其保持在鼠标指针位置不变
      newPos = {
        x: pointer.x - ((pointer.x - stage.x()) * newScale) / oldScale,
        y: pointer.y - ((pointer.y - stage.y()) * newScale) / oldScale
      };
      defaultx.value = newPos.x;
      defaulty.value = newPos.y;
      // stage.position(newPos); // 更新缩放位置
      stage.batchDraw(); // 渲染更新后的缩放和位置
    }
  }
  function handleZoomDown() {
    let zoomAmount = -0.05; // 缩放速度
    let oldScale = stage.scaleX(); // 获取缩放
    let pointer = stage.getPointerPosition();
    // console.log(pointer);
    if (!pointer) pointer = { x: 110, y: 0 };
    if (oldScale <= 0.3 && oldScale > 0) {
      zoomAmount = -0.02;
    }
    let newScale = oldScale + zoomAmount;

    // 限制缩放范围
    if (newScale > 0.1 && newScale < 10) {
      if (isTrue.value) {
        sketchpadScale = newScale;
        setSketchpad();
      }
      stage.scale({ x: newScale, y: newScale }); // 针对整个 stage 设置新的缩放比例

      // 根据缩放比例调整舞台位置，使其保持在鼠标指针位置不变
      newPos = {
        x: pointer.x - ((pointer.x - stage.x()) * newScale) / oldScale,
        y: pointer.y - ((pointer.y - stage.y()) * newScale) / oldScale
      };
      defaultx.value = newPos.x;
      defaulty.value = newPos.y;
      // stage.position(newPos); // 更新缩放位置
      stage.batchDraw(); // 渲染更新后的缩放和位置
    }
  }
  let layer;
  let locationArrx = [];
  let locationArry = [];
  function canvasInit() {
    stage = new konva.Stage({
      container: "main", //dom id
      width: width,
      height: height,
      x: defaultx.value,
      y: defaulty.value,
      draggable: draggable.value
    });
    layer = new konva.Layer();
    rectangles = []; // 重置矩形元素数组
    for (let i = 0; i < dataList.value.length; i++) {
      const defColor = "#eee";
      let group = new konva.Group({
        x: 0,
        y: 0,
        width: 100,
        height: 100,
        draggable: isTrue.value
      });

      let x = dataList.value[i].coordinate
        ? (dataList.value[i].coordinate[0] +
          (dataList.value[i].offsetCoordinate
            ? dataList.value[i].offsetCoordinate[0]
            : 0)) *
        coefficient.value
        : null;

      let y = dataList.value[i].coordinate
        ? (dataList.value[i].coordinate[1] +
          (dataList.value[i].offsetCoordinate
            ? dataList.value[i].offsetCoordinate[1]
            : 0)) *
        coefficient.value
        : null;
      let t = setText(dataList, i);

      // 判断 x 和 y 是否为 null
      if (x === null || y === null) {
        continue;
      }
      let divColor = dataList.value[i].state
        ? color[dataList.value[i].state] || defColor
        : defColor;
      if (dataList.value[i].isLock) {
        divColor = color.HasTag;
      }
      let rect = new konva.Rect({
        x: x / 10,
        y: y / 10,
        dataCode: dataList.value[i],
        width: 120,
        height: 100,
        fill: divColor,
        shadowBlur: 5,
        cornerRadius: 4
      });

      let simpleText = new konva.Text({
        x: x / 10 + textX.value,
        y: y / 10 + textY.value,
        dataCode: dataList.value[i],
        text: t || "未命名",
        fontSize: 20,
        fontFamily: "Calibri",
        fill: "#000000"
      });
      rectangles.push({ rect, simpleText }); // 存储元素
      layer.add(rect); // 添加到图层
      group.add(rect, simpleText);
      group.on("mousedown touchstart", function (e) {
        // e.cancelBubble = true;
        // var pos = stage.getPointerPosition();
        // console.log("当前鼠标或触摸点的页面坐标：", pos);
        // console.log(e.target.attrs.dataCode);

        if (e.target == rect) {
          draggable.value = false;
        }
        if (isTrue.value) return; // 编辑模式，直接返回
        clickTimeout = setTimeout(() => {
          handleSetStates(e); // 只有在没有拖动的情况下才处理点击
        }, 200);
        // console.log("触发点击事件");
        // handleSetStates(e);
      });

      group.on("dragstart", function (_) {
        // console.log("group开始拖拽");
        isKw.value = true;
      });

      group.on("dragend", function (e) {
        // 更新 dataList 中的坐标，初始坐标加上偏移量
        // console.log("group拖拽结束", e);
        if (!dataList.value[i].offsetCoordinate) {
          dataList.value[i].offsetCoordinate = [0, 0];
          console.log(dataList.value[i]);
        }
        // dataList.value[i].offsetCoordinate[0] =
        //   dataList.value[i].offsetCoordinate[0] + group.x() * 10;
        // dataList.value[i].offsetCoordinate[1] =
        //   dataList.value[i].offsetCoordinate[1] + group.y() * 10;
        locationArrx[i] =
          Number((group.x() * 10).toFixed(5)) / coefficient.value;
        locationArry[i] =
          Number((group.y() * 10).toFixed(5)) / coefficient.value;
        // dataList.value[i].offsetCoordinate[0] = (group.x() * 10).toFixed(5);
        // dataList.value[i].offsetCoordinate[1] = (group.y() * 10).toFixed(5);
        stage.draw();

        isKw.value = false;
        // console.log(
        //   "拖拽后的偏移量",
        //   dataList.value[i].offsetCoordinate[0],
        //   dataList.value[i].offsetCoordinate[1]
        // );

        // canvasInit();
      });
      layer.add(group);
    }
    stage.on("mousedown touchstart", function (e) {
      draggable.value = true;
    });
    stage.on("dragstart", function (e) {
      clearTimeout(clickTimeout); //发现是拖拽就清空点击事件
    });
    let touchStartX, touchStartY, touchEndX, touchEndY, moveX, moveY;
    let isTouchstart = false;
    // 监听触摸开始事件
    stage.on("touchstart", function (e) {
      isTouchstart = true;
      // 获取触摸点的位置
      const touchPoint = e.evt.touches[0]; // 获取第一个触摸点
      touchStartX = touchPoint.clientX;
      touchStartY = touchPoint.clientY;
    });
    // 监听触摸结束事件
    stage.on("touchend", function (e) {
      if (isKw.value || !isTrue.value) return;
      // 获取触摸点的位置
      const touchPoint = e.evt.changedTouches[0]; // 获取离开的触摸点
      touchEndX = touchPoint.clientX;
      touchEndY = touchPoint.clientY;
      moveX = touchEndX - touchStartX;
      moveY = touchEndY - touchStartY;
      defaultx.value += moveX;
      defaulty.value += moveY;
      setSketchpad();
    });
    stage.on("dragend", function (e) {
      if (isTouchstart) {
        isTouchstart = false;
        return;
      }
      if (e.target === stage && isTrue.value && !isKw.value) {
        defaultx.value = e.target.attrs.x;
        defaulty.value = e.target.attrs.y;
        setSketchpad();
      }
    });

    stage.on("wheel", function (e) {
      let zoomAmount = e.evt.deltaY * -0.0005; // 缩放速度
      e.evt.preventDefault();
      let oldScale = stage.scaleX(); // 获取缩放
      let pointer = stage.getPointerPosition();
      let newScale = oldScale + zoomAmount;

      // 限制缩放范围
      if (newScale > 0.1 && newScale < 10) {
        if (isTrue.value) {
          sketchpadScale = newScale;
          setSketchpad();
        }
        stage.scale({ x: newScale, y: newScale }); // 针对整个 stage 设置新的缩放比例

        // 根据缩放比例调整舞台位置，使其保持在鼠标指针位置不变
        newPos = {
          x: pointer.x - ((pointer.x - stage.x()) * newScale) / oldScale,
          y: pointer.y - ((pointer.y - stage.y()) * newScale) / oldScale
        };

        // stage.position(newPos); // 更新缩放位置
        stage.batchDraw(); // 渲染更新后的缩放和位置
      }
    });

    if (sketchpad && sketchpad[menuItem]) {
      let scale = sketchpad[menuItem].sketchpadScale;
      // console.log("sketchpadScale", scale);
      if (scale) {
        stage.scale({ x: Number(scale), y: Number(scale) });
      }
      // if (!scale) {
      //   scale = 0.2;
      // }

      // newPos.x = newPos.x * Number(defaultx.value);
      // newPos.y = newPos.y * Number(defaulty.value);

      // stage.position(newPos);
      stage.batchDraw();
    }
    stage.add(layer);
  }

  function setText(dataList, i) {
    let containersT = "";
    let materialsT = "";
    let t = "";
    if (dataList.value[i].containers && dataList.value[i].containers.length) {
      containersT = dataList.value[i].containers[0].name;
      if (dataList.value[i].containers.length >= 6) {
        containersT = dataList.value[i].containers[0].name
          .replace(/(.{6})/g, "$1\n")
          .trim();
      }
    }
    if (dataList.value[i].materials && dataList.value[i].materials.length) {
      materialsT = dataList.value[i].materials[0].name;
      if (dataList.value[i].materials.length >= 6) {
        materialsT = dataList.value[i].materials[0].name
          .replace(/(.{6})/g, "$1\n")
          .trim();
      }
    }
    if (dataList.value[i].name.length >= 6) {
      // t = dataList.value[i].name.slice(0, 6) + "...";
      t =
        dataList.value[i].name.replace(/(.{6})/g, "$1\n").trim() +
        "\n" +
        containersT +
        "\n" +
        materialsT;
    } else {
      t = dataList.value[i].name + "\n" + containersT + "\n" + materialsT;
    }
    return t;
  }
  async function updateColor() {
    newPos = initPos;
    defaultx.value = newPos.x;
    defaulty.value = newPos.y;
    if (activeAreaId.value === "all") {
      activeAreaId.value = "";
      pageParam.value = handlePageParam();
    }
    setTimeout(async () => {
      await handleGetList(activeAreaId.value);
    }, 0);
    // const { data } = await StorageGetPage(activeAreaId.value, pageParam.value);
    // dataList.value = data.rows;
    // // 更新已存在的元素颜色
    // rectangles.forEach((item, i) => {
    //   if (dataList.value[i]) {
    //     let newColor = color[dataList.value[i].state] || "#eee"; // 获取新颜色
    //     if (dataList.value[i].isLock) {
    //       newColor = color.HasTag;
    //     }
    //     item.rect.fill(newColor); // 更新颜色
    //     let t = setText(dataList, i);
    //     item.simpleText.text(t); // 更新文本内容
    //     // item.simpleText.text(dataList.value[index].name); // 更新文本内容
    //   }
    // });
    // layer.batchDraw(); // 只更新图层
  }

  onMounted(async () => {
    // setInterval(async () => {
    //   updateColor();
    // }, 5000);
  });

  return {
    tagData,
    dataList,
    activeAreaId,
    selectedItemId,
    newFormInline,
    isTrue,
    coefficient,
    textX,
    textY,
    cardStates,
    areaList,
    handleEdit,
    handleSave,
    canvasInit,
    handleResetStates,
    handleGetList,
    GetAreaListFn,
    handleResetAll,
    handleZoomUp,
    handleZoomDown,
    updateColor
    // handleContainer
  };
}
