import { type Ref, h, ref, reactive, onMounted } from "vue";
//@ts-ignore
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  GetTypeListToSelect,
  getPage,
  addOrUpdate,
  deletes,
  getAreaList
} from "@/api/task/logisticsroute";
import { GetMap, GetSimpleMap } from "@/api/monitor/runtime";
// import SimpleMap  from "./0606test.json"
import { type FormItemProps } from "../utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
import process from "../../tasktemplateprocess/index.vue";
import { useMyI18n } from "@/plugins/i18n";
import { mapTransform } from "./transform.js";
export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const TaskTemplateTypeList = ref([]);
  const nodeList = ref(); //站点
  const loading = ref(true);
  const carTypeList = ref([]);
  const dataList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const areaCode = ref();
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });
  const columns: TableColumnList = [
    { type: "selection", align: "left" },
    { label: "编号", prop: "id", hide: true },
    { headerRenderer: () => t("table.index"), type: "index", width: 60 },
    { headerRenderer: () => t("table.code"), prop: "code", sortable: true },
    { headerRenderer: () => t("table.lineName"), prop: "name", sortable: true },
    {
      headerRenderer: () => t("table.startingPointReservoirArea"),
      prop: "srcName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.terminalReservoirArea"),
      prop: "destName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.operation"),
      slot: "operation",
      minWidth: 120
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
    // 遍历路由数组  修改起点/终点库区的id为name
    dataList.value.forEach(route => {
      const srcArea = areaCode.value.find(
        area => area.code === route.srcAreaCode
      );
      if (srcArea) {
        route.srcName = srcArea.name;
      }
      const destArea = areaCode.value.find(
        area => area.code === route.destAreaCode
      );
      if (destArea) {
        route.destName = destArea.name;
      }
    });
    loading.value = false;
  }

  function handleRow(row = undefined) {
    console.log(row);
    return {
      code: row?.code ?? `dx_${dayjs().format("YYMMDDHHmmss")}`,
      name: row?.name ?? "",
      // destAreaCode: row?.destAreaCode ?? "",
      // srcAreaCode: row?.srcAreaCode ?? "",
      // sortNumber: row?.sortNumber ?? 0,
      destAreaId: row?.destAreaId ?? "",
      srcAreaId: row?.srcAreaId ?? ""
      // isEnable: row?.isEnable ?? true,
      // isDelete: row?.isDelete ?? false,
      // "createBy": row?.createBy ??"",
      // "updateBy": row?.updateBy ??"",
      // "remark": row?.remark ??"",
      // "extend": row?.extend ??"",
    };
  }

  const page: any = {
    canvas: {},
    panel: {},
    init: () => {
      page.box = document.getElementById("boxPanel");
      page.canvas = document.getElementById("canvas");
      page.panel = new Panel(page.box, page.canvas, {
        map: { size: { w: 1800, h: 1500 } },
        isEdit: false,
        isInfo: false
      }).init();
      page.bind();
      // page.time();
    },
    bind: () => {
      page.panel.panelActiveAction = panel => page.setPanelForm(panel);
      page.panel.nodeRectActiveAction = (shape, shapes) =>
        page.setNodeRectForm(shape);
      page.panel.nodeEllipseActiveAction = (shape, shapes) =>
        page.setNodeEllipseForm(shape, shapes);
      page.panel.edgeBeelineActiveAction = (shape, shapes) =>
        page.setEdgeBeelineForm(shape);
      page.panel.edgeArcActiveAction = (shape, shapes) =>
        page.setEdgeArcForm(shape);
      page.panel.edgeQuadraticActiveAction = (shape, shapes) =>
        page.setEdgeQuadraticForm(shape);
      page.panel.edgeBezierActiveAction = (shape, shapes) =>
        page.setEdgeBezierForm(shape);
      page.panel.tagRectActiveAction = (shape, shapes) =>
        page.setTagRectForm(shape);
      page.panel.tagEllipseActiveAction = (shape, shapes) =>
        page.setTagEllipseForm(shape);
      page.panel.zoneRectActiveAction = (shape, shapes) =>
        page.setZoneRectForm(shape);
      page.panel.zoneEllipseActiveAction = (shape, shapes) =>
        page.setZoneEllipseForm(shape);
      page.panel.carForkliftActiveAction = (shape, shapes) =>
        page.setCarForm(shape);
      page.panel.carConveyorActiveAction = (shape, shapes) =>
        page.setCarForm(shape);
      page.panel.carTuggerActiveAction = (shape, shapes) =>
        page.setCarForm(shape);
      page.panel.carCarrierActiveAction = (shape, shapes) =>
        page.setCarForm(shape);
      window.addEventListener("resize", event => page.panel.setResize());
    },
    setPanelForm: panel => {
      page.setTabActive("map", "：站点(矩形)");
      // console.log("地图")
    },
    setNodeRectForm: shape => {
      page.setTabActive("tabNodeRect", "：站点(矩形)");
      // showInfo.node = reactive(shape)
    },
    setNodeEllipseForm: shape => {
      page.setTabActive("tabNodeRect", "：站点(圆形)");
      // showInfo.node = reactive(shape)
    },
    setEdgeBeelineForm: shape => {
      page.setTabActive("tabEdge", "：路线(直线)");
      // edgeBeelineForm.shape = toRefs(reactive(shape));
      // showInfo.line  = reactive(shape)
      // console.log("showInfo.line",showInfo.line)
    },
    setEdgeArcForm: shape => {
      page.setTabActive("tabEdge", "：路线(圆弧)");
      // showInfo.line  = reactive(shape)
    },
    setEdgeQuadraticForm: shape => {
      page.setTabActive("tabEdge", "：路线(贝塞尔二次)");
      // showInfo.line  = reactive(shape)
    },
    setEdgeBezierForm: shape => {
      page.setTabActive("tabEdge", "：路线(贝塞尔三次)");
      // showInfo.line  = reactive(shape)
    },
    setTagRectForm: shape => {
      page.setTabActive("tabTagRect", "：标签(矩形)");
      // tagRectForm.shape = toRefs(reactive(shape));
      // console.log( "shape" , shape   )
    },
    setTagEllipseForm: shape => {
      page.setTabActive("tabTagEllipse", "：标签(圆形)");
      // tagEllipseForm.shape = toRefs(reactive(shape));
    },
    setZoneRectForm: shape => {
      page.setTabActive("tabZone", "：区域(矩形)");
      // showInfo.zone = reactive(shape)
      // zoneRectForm.shape = toRefs(reactive(shape));
      // console.log( "shape" , shape   )
    },
    setZoneEllipseForm: shape => {
      // showInfo.zone = reactive(shape)
      page.setTabActive("tabZone", "：区域(圆形)");
      // zoneEllipseForm.shape = toRefs(reactive(shape));
    },
    setCarForm: shape => {
      page.setTabActive("tabCar", "车辆");
      // showInfo.car = reactive(shape);
      // showInfo.car.point = JSON.parse(JSON.stringify(showInfo.car.point))
      // console.log(" showInfo.car" , showInfo.car)
    },
    setTabActive: (tabId, tabTitle) => {},
    saveData: data => {},
    setNodePosition: shapeType => {},
    setTrajectory: shapeType => {},
    setActions: shapeType => {},
    setExtends: shapeType => {},
    test: () => {}
  };

  //获取地图数据，并导入地图
  const getMap = async () => {
    try {
      let data = await GetSimpleMap();
      // let data  = await GetMap();
      // let data = {
      //   code: 200,
      //   data: {}
      // };
      // data.data = SimpleMap;
      console.log("data is", data);
      if (data.code === 200) {
        console.log(data, "我进来了");
        if (!Object.keys(data.data).length) {
          message("请打开Simple地图再进行操作！", { type: "warning" });
          return;
        }
        // let MapInfo = JSON.parse(data.data)
        let newMap = mapTransform.init(data.data);
        console.log("newMap", "\n", newMap);
        // ZoneInfoArr.length = 0;
        // ZoneInfoArr = [];
        page.panel.setData(newMap);
        // page.panel.setData(JSON.parse(data.data.map.fileContent))
        // setTimeout(() => {
        //   for (let index = 0; index < MapInfo.zones.length; index++) {
        //   // const element = array[index];
        //   ZoneInfoArr.push(MapInfo.zones[index].code);
        //   StartZoneTransparency = MapInfo.zones[index].globalAlpha
        //   ZoneTransparency = MapInfo.zones[index].globalAlpha
        //   const element = MapInfo.zones[index]
        //   // console.log("element",MapInfo.zones.code)
        //   page.panel.setToBottom(MapInfo.zones[index])
        // }
        // }, 1000);
      }
    } catch {}
  };

  async function TypeListToSelect() {
    const { data } = await GetTypeListToSelect();
    carTypeList.value = [...data];
  }

  async function GetTaskTemplateTypeList() {
    try {
      const data = await GetDictItemInLocal("TaskTemplateType");
      TaskTemplateTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function getZoneDataListFn() {
    pageParam.value = handlePageParam();
    const { data } = await getAreaList(pageParam.value);
    areaCode.value = data.rows;
  }

  async function handleDelete(rows = selection.value, cancel = undefined) {
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

  onMounted(async () => {
    // 需要在这里获取数据
  });

  return {
    page,
    getMap,
    loading,
    columns,
    dataList,
    pagination,
    areaCode,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleDelete
  };
}
