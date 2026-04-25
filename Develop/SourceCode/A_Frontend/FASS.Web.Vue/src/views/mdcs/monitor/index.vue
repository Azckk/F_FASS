<script setup>
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import CaretRight from "@iconify-icons/ep/caret-right";
import FullScreen from "@iconify-icons/ep/full-screen";
import Fold from "@iconify-icons/ep/fold";
import Rank from "@iconify-icons/ep/rank";
import Plus from "@iconify-icons/ep/plus";
import Minus from "@iconify-icons/ep/minus";
import Operation from "@iconify-icons/ep/operation";
import { onMounted } from "vue";
import { reactive, ref, toRefs, watch, computed } from "vue";
import { ElLoading } from "element-plus";
import { IconifyIconOffline } from "@/components/ReIcon";
import { useHook } from "./utils/hook.tsx"
import { Save } from "@/api/map/edit";
defineOptions({
  name: ""
});
const tabCurrent = reactive({
  data: {
    id: "tabPanel",
    title: "：地图"
  }
});
const tableData = [
  {
    date: "001",
    name: "Tom",
    address: "12"
  },
  {
    date: "002",
    name: "Tom",
    address: "100"
  },
  {
    date: "003",
    name: "Tom",
    address: "32"
  },
  {
    date: "004",
    name: "Tom",
    address: "89"
  }
];



onMounted(() => {
  getMap();
  page.init(); //初始化Canvas
  // OpenMap();
});
</script>

<template>
  <div>
    <el-row :gutter="10">
      <el-col :span="4">
        <el-card class="box-card">
          <template #header>
            <div class="card-header">
              <span>{{ $t("title.status") }}</span>
            </div>
          </template>
          <el-scrollbar id="boxToolbar">
            <el-table :data="tableData" style="width: 100%; height: 600px">
              <el-table-column
                prop="date"
                :label="$t('table.car')"
                width="60"
              />
              <el-table-column
                prop="name"
                :label="$t('table.site')"
                width="60"
              />
              <el-table-column
                prop="address"
                :label="$t('table.electricity')"
                width="60"
              />
              <el-table-column prop="address" :label="$t('table.status')" />
            </el-table>
          </el-scrollbar>
        </el-card>
      </el-col>
      <el-col :span="16">
        <el-card class="box-card">
          <template #header>
            <div class="card-header">
              <div>
                <span>{{ $t("title.map") }}</span>
              </div>
              <div>
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(CaretRight)"
                >
                  {{ $t("buttons.startMonitoring") }}</el-button
                >
                <el-button text bg size="small" :icon="useRenderIcon(Plus)">
                  {{ $t("buttons.mapZoomIn") }}</el-button
                >
                <el-button text bg size="small" :icon="useRenderIcon(Minus)">
                  {{ $t("buttons.mapShrinks") }}</el-button
                >
                <el-button text bg size="small" :icon="useRenderIcon(Rank)">
                  {{ $t("buttons.mapAdaptive") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(FullScreen)"
                >
                  {{ $t("buttons.map100") }}</el-button
                >
                <el-button text bg size="small" :icon="useRenderIcon(Fold)">
                  {{ $t("buttons.switchInformation") }}</el-button
                >
                <el-button
                  text
                  bg
                  size="small"
                  :icon="useRenderIcon(Operation)"
                >
                  {{ $t("buttons.toggleTrajectory") }}</el-button
                >
              </div>
            </div>
          </template>
          <el-scrollbar id="boxPanel">
            <canvas
              style="width: 740px; height: 600px; border: 1px solid #ccc"
              id="canvas"
              tabindex="0"
              >浏览器不支持canvas</canvas
            >
          </el-scrollbar>
        </el-card>
      </el-col>
      <el-col :span="4">
        <el-card class="box-card">
          <template #header>
            <div class="card-header">
              <span>{{ $t("title.alarm") }}</span>
            </div>
          </template>
          <el-scrollbar id="boxToolbar">
            <el-table :data="tableData" style="width: 100%; height: 600px">
              <el-table-column
                prop="date"
                :label="$t('table.level')"
                width="60"
              />
              <el-table-column
                prop="name"
                :label="$t('table.type')"
                width="60"
              />
              <el-table-column prop="address" :label="$t('table.message')" />
            </el-table>
          </el-scrollbar>
        </el-card>
      </el-col>
    </el-row>
    <!-- <NodePosition ref="nodePositionRef" />
    <ActionIndex ref="actionIndexRef" />
    <ExtendedField ref="extendedFieldRef" />
    <TrackRoute ref="trackRouteRef" /> -->
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
</style>