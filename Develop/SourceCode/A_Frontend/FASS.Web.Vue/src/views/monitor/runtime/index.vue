<script setup lang="ts">
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import CaretRight from "@iconify-icons/ep/caret-right";
import Pause from "@iconify-icons/ri/pause-fill";
import CaretLeft from "@iconify-icons/ep/caret-left";
import FullScreen from "@iconify-icons/ep/full-screen";
import Fold from "@iconify-icons/ep/fold";
import search from "@iconify-icons/ep/search";
import Rank from "@iconify-icons/ep/rank";
import Plus from "@iconify-icons/ep/plus";
import Minus from "@iconify-icons/ep/minus";
import Operation from "@iconify-icons/ep/operation";
import { useHook as useMapHook } from "./utils/hook";
import { reactive, ref, toRefs, watch, computed, onMounted } from "vue";
// import TestDialog from "./components/test.vue";
import TaskDialog from "./components/task.vue";
import TlarmDialog from "./components/alarm.vue";
import CarDialog from "./components/car.vue";
import LowBattery from "./components/lowBattery.vue";

import monitor_lowBattery from "../../../assets/monitor/monitor_lowBattery.svg?component";

import mapInfoDialog from "./mapUtils/dialog/index.vue";
import Side from "./components/side.vue";
import SlideDrawer from "./DraggableDrawer.vue";
import { watchEffect } from "vue";

defineOptions({
  name: "Monitor-Runtime"
});
let mapCanvas = ref(null);
const {
  SearchCode,
  handleButtonClick,
  isMonitoring,
  draggableData,
  dataBaseTask,
  dataBaseAlarm,
  dataBaseCarData,
  dataBaseCarStatus,
  dataBaseLowBattery,
  handleRowDoubleClick
} = useMapHook();
onMounted(() => {});
// const dataBaseLowBattery = ref([
//   { carName: "车辆1", vehicleID: "123", electricity: 25 },
//   { carName: "车辆2", vehicleID: "124", electricity: 26 },
//   { carName: "车辆3", vehicleID: "126", electricity: 16 }
// ]);

// 弹窗功能
const drawer1Open = ref(false);
const drawer2Open = ref(false);
const drawer3Open = ref(false);
const drawer4Open = ref(false);
const drawer5Open = ref(false);

const activeDrawer = ref(null);
const baseZIndex = 1000;

const zIndex1 = ref(baseZIndex);
const zIndex2 = ref(baseZIndex);
const zIndex3 = ref(baseZIndex);
const zIndex4 = ref(baseZIndex);
const zIndex5 = ref(baseZIndex);

const toggleDrawer = (drawerNumber: number) => {
  // Reset all zIndex values to baseZIndex
  zIndex1.value = baseZIndex;
  zIndex2.value = baseZIndex;
  zIndex3.value = baseZIndex;
  zIndex4.value = baseZIndex;
  zIndex5.value = baseZIndex;
  // Toggle the drawer state
  if (drawerNumber === 1) {
    if (!drawer1Open.value) {
      bringToFront(1);
    }
    drawer1Open.value = !drawer1Open.value;
  }
  if (drawerNumber === 2) {
    if (!drawer2Open.value) {
      bringToFront(2);
    }
    drawer2Open.value = !drawer2Open.value;
  }
  if (drawerNumber === 3) {
    if (!drawer3Open.value) {
      bringToFront(3);
    }
    drawer3Open.value = !drawer3Open.value;
  }
  if (drawerNumber === 4) {
    if (!draggableData.value.drawer4Open) {
      bringToFront(4);
    }
  }
  if (drawerNumber === 5) {
    if (!drawer5Open.value) {
      bringToFront(5);
    }
    drawer5Open.value = !drawer5Open.value;
  }
};

const bringToFront = (drawerNumber: number) => {
  zIndex1.value = baseZIndex;
  zIndex2.value = baseZIndex;
  zIndex3.value = baseZIndex;
  zIndex4.value = baseZIndex;
  zIndex5.value = baseZIndex;

  if (drawerNumber === 1) zIndex1.value = baseZIndex + 10;
  if (drawerNumber === 2) zIndex2.value = baseZIndex + 10;
  if (drawerNumber === 3) zIndex3.value = baseZIndex + 10;
  if (drawerNumber === 4) zIndex4.value = baseZIndex + 10;
  if (drawerNumber === 5) zIndex5.value = baseZIndex + 10;
};

const setActiveDrawer = (drawerNumber: number) => {
  activeDrawer.value = drawerNumber;
};
const isShowDrawer = ref(false);
watchEffect(() => {
  // 解决弹窗动画问题
  if (draggableData.value.drawer4Open) {
    isShowDrawer.value = draggableData.value.drawer4Open;
  } else {
    setTimeout(() => {
      isShowDrawer.value = draggableData.value.drawer4Open;
    }, 1000);
  }
});
</script>

<template>
  <div>
    <el-row :gutter="10">
      <el-col :span="24">
        <el-card class="box-card">
          <template #header>
            <div class="card-header">
              <div>
                <span>{{ $t("title.map") }}</span>
              </div>
              <div class="flex" @click="handleButtonClick">
                <el-input
                  v-model="SearchCode"
                  class="w-200px"
                  size="small"
                  clearable
                  placeholder="编码"
                />
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(search)"
                  data-action="SearchMonitoring"
                >
                  {{ $t("buttons.search") }}</el-button
                >
                <el-button
                  v-if="!isMonitoring"
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(CaretRight)"
                  data-action="startMonitoring"
                >
                  {{ $t("buttons.startMonitoring") }}</el-button
                >
                <el-button
                  v-else
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(Pause)"
                  data-action="stopMonitoring"
                >
                  {{ $t("buttons.stopMonitoring") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(Plus)"
                  data-action="mapZoomIn"
                >
                  {{ $t("buttons.mapZoomIn") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(Minus)"
                  data-action="mapShrinks"
                >
                  {{ $t("buttons.mapShrinks") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(Rank)"
                  data-action="mapAdaptive"
                >
                  {{ $t("buttons.mapAdaptive") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(FullScreen)"
                  data-action="map100"
                >
                  {{ $t("buttons.map100") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(Fold)"
                  data-action="switchInformation"
                >
                  {{ $t("buttons.switchInformation") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(Operation)"
                  data-action="toggleTrajectory"
                >
                  {{ $t("buttons.toggleTrajectory") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(Operation)"
                  data-action="switchShowCar"
                >
                  {{ $t("buttons.switchShowCar") }}</el-button
                >
              </div>
            </div>
          </template>
          <div id="boxPanel" class="box-card mapCanvasDiv">
            <!-- <el-scrollbar id="boxPanel" style="height: 100%"> -->
            <canvas
              id="canvas"
              ref="mapCanvas"
              style="
                width: 1120px;
                height: calc(100vh - 250px);
                border: 1px solid #ccc;
                background: #cfd6e0;
              "
              tabindex="0"
              >浏览器不支持canvas</canvas
            >
            <!-- </el-scrollbar> -->
          </div>
        </el-card>
      </el-col>
    </el-row>
    <div class="monitor-box">
      <div class="box-item" @click="toggleDrawer(1)">
        <el-button link :icon="useRenderIcon(CaretLeft)" />
        <div class="item">
          <img
            src="../../../assets/monitor/monitor_task.png"
            class="h-12"
            alt=""
          />
          <div>{{ $t("table.taskMonitoring") }}</div>
        </div>
      </div>
      <div class="box-item" @click="toggleDrawer(2)">
        <el-button link :icon="useRenderIcon(CaretLeft)" />
        <div class="item">
          <img
            src="../../../assets/monitor/monitor_alarm.png"
            class="h-12"
            alt=""
          />
          <div>{{ $t("table.alarmMonitoring") }}</div>
        </div>
      </div>
      <div class="box-item" @click="toggleDrawer(3)">
        <el-button link :icon="useRenderIcon(CaretLeft)" />
        <div class="item">
          <img
            src="../../../assets/monitor/monitor_car.png"
            class="h-12"
            alt=""
          />
          <div>{{ $t("table.fleetMonitoring") }}</div>
        </div>
      </div>
      <div class="box-item" @click="toggleDrawer(5)">
        <el-button link :icon="useRenderIcon(CaretLeft)" />
        <div class="item">
          <!-- <img
            src="../../../assets/monitor/monitor_car.png"
            class="h-12"
            alt=""
          /> -->
          <div class="h-12 svgCnt">
            <component :is="monitor_lowBattery" />
          </div>

          <div>{{ $t("table.lowOnPower") }}</div>
        </div>
      </div>
    </div>
    <div>
      <SlideDrawer
        id="4"
        v-model="drawer1Open"
        title="任务监控"
        :z-index="zIndex1"
        :is-active="activeDrawer === 1"
        @activate="setActiveDrawer(1)"
        @mouseenter="bringToFront(1)"
      >
        <template #default>
          <TaskDialog :tableData="dataBaseTask" />
        </template>
      </SlideDrawer>
      <SlideDrawer
        id="1"
        v-model="drawer2Open"
        title="报警监控"
        :z-index="zIndex2"
        :is-active="activeDrawer === 2"
        @activate="setActiveDrawer(2)"
        @mouseenter="bringToFront(2)"
      >
        <template #default>
          <TlarmDialog :tableData="dataBaseAlarm" />
        </template>
      </SlideDrawer>
      <SlideDrawer
        id="2"
        v-model="drawer3Open"
        title="车队监控"
        :z-index="zIndex3"
        :is-active="activeDrawer === 3"
        @activate="setActiveDrawer(3)"
        @mouseenter="bringToFront(3)"
      >
        <template #default>
          <CarDialog
            :tableData="dataBaseCarData"
            :carStateList="dataBaseCarStatus"
            @row-double-click="handleRowDoubleClick"
          />
        </template>
      </SlideDrawer>
      <SlideDrawer
        id="3"
        v-model="draggableData.drawer4Open"
        :title="draggableData.dialogTitle"
        :mainTitle="draggableData.mainTitle"
        :z-index="zIndex4"
        :is-active="activeDrawer === 4"
        @activate="setActiveDrawer(4)"
        @mouseenter="bringToFront(4)"
      >
        <template #default>
          <mapInfoDialog
            v-if="isShowDrawer"
            :data="draggableData.info"
            :msg="draggableData.dialogTitle"
          />
        </template>
      </SlideDrawer>

      <SlideDrawer
        id="5"
        v-model="drawer5Open"
        title="低电量车辆"
        :z-index="zIndex5"
        :is-active="activeDrawer === 5"
        @activate="setActiveDrawer(5)"
        @mouseenter="bringToFront(5)"
      >
        <template #default>
          <LowBattery :tableData="dataBaseLowBattery" />
        </template>
      </SlideDrawer>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.card-header {
  display: flex;
  justify-content: space-between;
}

.el-scrollbar {
  height: 79vh;
}

::v-deep(.el-menu-item) {
  height: 28px !important;
}

.monitor-box {
  border-radius: 4px;
  background-color: #eee;
  position: absolute;
  right: 15px;
  top: calc(50% - 200px);

  .item {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-bottom: 10px;
  }
}

.box-item {
  padding: 10px;
  display: flex;
}

.box-item:hover {
  background-color: rgba(90, 156, 248, 0.1);
}

.close {
  z-index: 9999;
}
#canvas,
.main-content,
.el-row,
.el-card__body,
.box-card {
  height: 100%;
}
.rtlBox {
  width: 30vw;
  height: auto;
  // background-color: #fff;
  position: absolute;
  right: 0px;
  top: 2px;
  z-index: 9999;
  box-shadow: #ccc 0 0 15px;
}
::v-deep(.el-scrollbar__wrap) {
  height: calc(100% - 60px);
}
::v-deep(.el-card__body) {
  // height: calc(100% - 120px);
}
</style>
<style>
.dialog-fade-enter-active,
.dialog-fade-leave-active {
  transition: transform 0.3s ease-out;
}

.dialog-fade-enter,
.dialog-fade-leave-to {
  transform: translateX(100%);
}
.mapCanvasDiv {
  width: 100%;
  height: 100%;
  /* max-width: 800px; */
  /* min-height: 800px; */
  /* border: 1px solid red; */
  /* overflow: hidden; */
}
.svgCnt {
  width: 48px;
  margin-bottom: -10px;
}
</style>
