<script setup lang="ts">
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import CaretRight from "@iconify-icons/ep/caret-right";
import Pause from "@iconify-icons/ri/pause-fill";
import CaretLeft from "@iconify-icons/ep/caret-left";
import CaretTop from "@iconify-icons/ep/caret-top";
import CaretBottom from "@iconify-icons/ep/caret-bottom";
import FullScreen from "@iconify-icons/ep/full-screen";
import Fold from "@iconify-icons/ep/fold";
import Rank from "@iconify-icons/ep/rank";
import Plus from "@iconify-icons/ep/plus";
import Minus from "@iconify-icons/ep/minus";
import Operation from "@iconify-icons/ep/operation";
import ArrowLeftBold from "@iconify-icons/ep/arrow-left-bold";
// import { useHook as useMapHook } from "./utils/hook";
import { useHook as useMapHook } from "@/views/monitor/runtime/utils/hook";
import { reactive, ref, toRefs, watch, computed, onMounted } from "vue";
// import TestDialog from "./components/test.vue";
import TaskDialog from "./components/task.vue";
import TlarmDialog from "./components/alarm.vue";
import CarDialog from "./components/car.vue";
// import mapInfoDialog from "./mapUtils/dialog/index.vue";
import LowBattery from "@/views/monitor/runtime/components/lowBattery.vue";

import Side from "./components/side.vue";
import SlideDrawer from "./DraggableDrawer.vue";
import Header from "../home/components/header.vue";
import { watchEffect } from "vue";
// import DialogMobile from "./dialog/index.vue";
import { ElMessageBox } from "element-plus";
import type { DrawerProps } from "element-plus";
defineOptions({
  name: "Monitor-Runtime"
});
let mapCanvas = ref(null);
const {
  handleButtonClick,
  isMonitoring,
  draggableData,
  dataBaseTask,
  dataBaseAlarm,
  dataBaseCarData,
  dataBaseCarStatus,
  dataBaseLowBattery,
  handleRowDoubleClick,
  startMonitoring
} = useMapHook();
onMounted(() => {
  startMonitoring();
});
const drawerTitle = ref();
const drawer = ref(false);
const activeDrawer = ref(-1);
const toggleDrawer = (drawerNumber: number, title: string) => {
  drawerTitle.value = title;
  activeDrawer.value = drawerNumber;
  drawer.value = true;
};
</script>

<template>
  <div style="height: 5rem"><Header class="header" /></div>
  <div class="monitor-btn-box" @click="handleButtonClick">
    <div class="monitor-btn">
      <el-button
        text
        bg
        size="large"
        :icon="useRenderIcon(Plus)"
        data-action="mapZoomIn"
      />
      <el-button
        text
        bg
        size="large"
        :icon="useRenderIcon(Minus)"
        data-action="mapShrinks"
      />
      <el-button
        text
        bg
        size="large"
        :icon="useRenderIcon(Rank)"
        data-action="mapAdaptive"
      />
    </div>
    <div class="btnfx">
      <el-button
        text
        bg
        size="large"
        :icon="useRenderIcon(CaretTop)"
        data-action="moveTop"
      />
      <div class="btn-noup">
        <el-button
          text
          bg
          size="large"
          :icon="useRenderIcon(CaretLeft)"
          data-action="moveLeft"
        />
        <el-button
          text
          bg
          size="large"
          :icon="useRenderIcon(CaretBottom)"
          data-action="moveBottom"
        />
        <el-button
          text
          bg
          size="large"
          :icon="useRenderIcon(CaretRight)"
          data-action="moveRight"
        />
      </div>
    </div>
  </div>
  <div>
    <el-row :gutter="10">
      <el-card class="box-card">
        <div id="boxPanel" class="box-card">
          <!-- <el-scrollbar id="boxPanel" style="height: 100%"> -->
          <canvas
            id="canvas"
            ref="mapCanvas"
            style="
              width: 1120px;
              height: 100%;
              border: 1px solid #ccc;
              background: #cfd6e0;
            "
            tabindex="0"
            >浏览器不支持canvas</canvas
          >
          <!-- </el-scrollbar> -->
        </div>
      </el-card>
    </el-row>
    <div class="monitor-box">
      <!-- <button @click="showDialogFn">test drawer</button> -->
      <div
        class="box-item"
        @click="toggleDrawer(1, $t('table.taskMonitoring'))"
      >
        <div class="item">
          <img
            src="../../../assets/monitor/monitor_task.png"
            class="h-12"
            alt=""
          />
          <div>{{ $t("table.taskMonitoring") }}</div>
        </div>
      </div>
      <div
        class="box-item"
        @click="toggleDrawer(2, $t('table.alarmMonitoring'))"
      >
        <div class="item">
          <img
            src="../../../assets/monitor/monitor_alarm.png"
            class="h-12"
            alt=""
          />
          <div>{{ $t("table.alarmMonitoring") }}</div>
        </div>
      </div>
      <div
        class="box-item"
        @click="toggleDrawer(3, $t('table.fleetMonitoring'))"
      >
        <!-- <el-button link :icon="useRenderIcon(CaretLeft)" /> -->
        <div class="item">
          <img
            src="../../../assets/monitor/monitor_car.png"
            class="h-12"
            alt=""
          />
          <div>{{ $t("table.fleetMonitoring") }}</div>
        </div>
      </div>
      <div class="box-item" @click="toggleDrawer(4, $t('table.lowOnPower'))">
        <div class="item">
          <img
            src="../../../assets/monitor/electricity.png"
            class="h-12"
            style="width: 40px; height: 30px"
            alt=""
          />
          <div>低电量车</div>
        </div>
      </div>
    </div>
    <el-drawer v-model="drawer" direction="rtl" :show-close="false" size="100%">
      <template #header="{ close, titleId, titleClass }">
        <el-button
          class="icon"
          text
          size="large"
          :icon="useRenderIcon(ArrowLeftBold)"
          @click="close"
        />
        <h4
          :id="titleId"
          style="text-align: center; transform: translateX(-15px)"
          :class="titleClass"
        >
          {{ drawerTitle }}
        </h4>
      </template>
      <CarDialog
        v-if="activeDrawer == 3"
        :tableData="dataBaseCarData"
        :carStateList="dataBaseCarStatus"
        @row-double-click="handleRowDoubleClick"
      />
      <TlarmDialog v-else-if="activeDrawer == 2" :tableData="dataBaseAlarm" />
      <TaskDialog v-else-if="activeDrawer == 1" :tableData="dataBaseTask" />
      <LowBattery
        v-else-if="activeDrawer == 4"
        :tableData="dataBaseLowBattery"
      />
    </el-drawer>
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
  width: 70px;
  font-size: 12px;
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
  height: calc(100% - 120px);
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
.monitor-btn-box {
  display: flex;
  width: 40px;
  position: absolute;
  left: 25px;
  bottom: 50px;
  font-size: 50;
  z-index: 999;
}
.monitor-btn {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  width: 40px;
  margin-right: 20px;
}
.btnfx {
  display: flex;
  flex-direction: column;
  justify-content: space-around;
  align-items: center;
  margin: 0;
}
.btn-noup {
  display: flex;
}
.icon {
  justify-content: left;
  width: 30px;
  flex: 0 !important;
}
</style>
