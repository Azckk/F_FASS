<script setup lang="ts">
import { onMounted, ref } from "vue";
import { Columnar, ChartLine, ChartRound } from "./components/charts";
import {
  chartData,
  picData1,
  picData2,
  columnarData,
  dataOverview,
  GetTaskReportFn
} from "./data";
import MoblieHeader from "@/views/mobile/home/components/header.vue";
import { usePermissionStoreHook } from "@/store/modules/permission";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";
import { storageLocal } from "@pureadmin/utils";
// 动态路由
import { getAsyncRoutes } from "@/api/routes";
import monitoringImg from "@/assets/welcome/运行监控.svg?component";
import map_editImg from "@/assets/Welcome/地图编辑.svg?component";
import ManagementImg from "@/assets/Welcome/任务管理.svg?component";
import VehicleImg from "@/assets/Welcome/车辆管理.svg?component";
import PermissionImg from "@/assets/Welcome/权限配置.svg?component";
import alarmImg from "@/assets/Welcome/报警监控.svg?component";
import visualizationImg from "@/assets/Welcome/库位可视化.svg?component";
// import SvgIcon from "./components/SvgIcon.vue";
// import monitoringImg from "@/assets/Welcome/monitoring.png";
// import map_editImg from "@/assets/Welcome/map_edit.png";
// import ManagementImg from "@/assets/Welcome/Management.png";
// import VehicleImg from "@/assets/Welcome/Vehicle.png";
// import PermissionImg from "@/assets/Welcome/Permission.png";
// import alarmImg from "@/assets/Welcome/alarm.png";
// import visualizationImg from "@/assets/Welcome/visualization.png";
const { wholeMenus } = storeToRefs(usePermissionStoreHook());
let isShow = ref(false);
if (wholeMenus.value.length != 0 && wholeMenus.value[1]?.name === "Mobile")
  isShow.value = true;
defineOptions({
  name: "Welcome"
});
const router = useRouter();
const dataList = ref({
  taskDayList: {}
});
const query = ref({
  time: ""
});

let routerList = ref();
async function getAsyncRoutesFn() {
  let { data } = await getAsyncRoutes();
  extractPaths(data);
  routerList.value = routerItems.filter(item =>
    availablePaths.includes(item.path)
  );
}
const routerItems = [
  {
    path: "/monitor/runtime/index",
    title: "运行监控",
    icon: monitoringImg
  },
  {
    path: "/task/taskrecord/index",
    title: "任务管理",
    icon: ManagementImg
  },
  {
    path: "/data/car/index",
    title: "车辆管理",
    icon: VehicleImg
  },
  {
    path: "/map/edit/index",
    title: "地图编辑",
    icon: map_editImg
  },
  {
    path: "/account/permission/index",
    title: "权限配置",
    icon: PermissionImg
  },
  {
    path: "/monitor/alarmmdcs/index",
    title: "报警监控",
    icon: alarmImg
  },
  {
    path: "/warehouse/visualization/index",
    title: "库位可视化",
    icon: visualizationImg
  }
];
let availablePaths = []; // 用于存储所有的路径
function extractPaths(routes) {
  routes.forEach(route => {
    availablePaths.push(route.path);
    if (route.children) {
      extractPaths(route.children);
    }
  });
}

// 路由跳转方法
function changeRouterFn(path) {
  router.push(path);
}
onMounted(() => {
  GetTaskReportFn();
  getAsyncRoutesFn();
});
</script>

<template>
  <div v-if="isShow" class="MoblieHeaderUI">
    <div class="header">
      <MoblieHeader />
    </div>
    <div style="padding-top: 6rem" />
  </div>
  <div class="welcome">
    <el-row :gutter="24" class="welcome-row">
      <el-col :span="16" :xs="24" :sm="24" :md="24" :lg="16" :xl="16"
        ><div class="grid-content ep-bg-purple" />
        <div class="chart">
          <span class="chartTitle">数据总览</span>
          <div class="cardList">
            <el-row :gutter="20" class="cardList-row">
              <el-col :span="6">
                <div class="item">
                  <div>
                    <p>今日执行任务总数</p>
                    <div class="item-data task">
                      <img src="@/assets/Welcome/Total Tasks.png" alt="" />
                      <div>
                        <span class="item-data-num">{{
                          dataOverview.total
                        }}</span
                        >个
                      </div>
                    </div>
                  </div>
                </div>
              </el-col>
              <el-col :span="6">
                <div class="item">
                  <div>
                    <p>任务已完成</p>
                    <div class="item-data overTask">
                      <img
                        src="@/assets/Welcome/Complete the task.png"
                        alt=""
                      />
                      <div>
                        <span class="item-data-num">{{
                          dataOverview.success
                        }}</span
                        >个
                      </div>
                    </div>
                  </div>
                </div>
              </el-col>
              <el-col :span="6">
                <div class="item">
                  <div>
                    <p>今日任务达成率</p>
                    <div class="item-data bTask">
                      <img
                        src="@/assets/Welcome/Task achievement rate.png"
                        alt=""
                      />
                      <div>
                        <span class="item-data-num">{{
                          dataOverview.rete
                        }}</span
                        >%
                      </div>
                    </div>
                  </div>
                </div>
              </el-col>
              <el-col :span="6">
                <div class="item">
                  <div>
                    <p>今日报警总数</p>
                    <div class="item-data errTask">
                      <img src="@/assets/Welcome/Alarms total.png" alt="" />
                      <div>
                        <span class="item-data-num">{{
                          dataOverview.totalAlarm
                        }}</span
                        >次
                      </div>
                    </div>
                  </div>
                </div>
              </el-col>
            </el-row>
          </div>
          <ChartLine class="chartTable" :dataList="chartData" />
        </div>
      </el-col>
      <el-col :span="8" :xs="24" :sm="24" :md="24" :lg="8" :xl="8">
        <div class="grid-content ep-bg-purple router">
          <span class="chartTitle">快捷入口</span>
          <div class="router-container">
            <div
              v-for="routerItem in routerList"
              :key="routerItem?.path"
              class="router-item"
              @click="changeRouterFn(routerItem.path)"
            >
              <div class="router-icon" style="">
                <!-- <img :src="routerItem.icon" alt="" /> -->
                <component
                  :is="routerItem.icon"
                  v-if="typeof routerItem.icon === 'object'"
                />
              </div>
              <div class="router-title">
                <span>{{ routerItem.title }}</span>
              </div>
            </div>
          </div>
        </div>
      </el-col>
    </el-row>
    <el-row :gutter="24">
      <el-col :span="16" :xs="24" :sm="24" :md="24" :lg="16" :xl="16">
        <div class="grid-content ep-bg-purple chartBottomDiv">
          <el-row :gutter="24">
            <el-col :span="16" :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
              <div class="chartBottom Robot">
                <span class="chartTitle">机器人工作状态</span>
                <ChartRound :dataList="picData1.data" :color="picData1.color" />
              </div>
            </el-col>
            <el-col :span="16" :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
              <div class="chartBottom change">
                <span class="chartTitle">充电站工作状态</span>
                <ChartRound :dataList="picData2.data" :color="picData2.color" />
              </div>
            </el-col>
          </el-row>
        </div>
      </el-col>
      <el-col :span="8" :xs="24" :sm="24" :md="24" :lg="8" :xl="8">
        <div class="grid-content ep-bg-purple chartBottomRight">
          <span class="chartTitle">今日报警类型Top10</span>
          <Columnar class="Columnar" :dataList="columnarData" />
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<style lang="scss" scoped>
.welcome {
  padding: 10px;
}
.header {
  height: 6rem;
  width: 100%;
  position: absolute;
  top: 0px;
  z-index: 999;
}
.chart {
  height: 50vh;
  // margin: 10px;
  background-color: #fff;
  border-radius: 5px;
  box-shadow: 2px 2px 12px 0 rgba(0, 0, 0, 0.1);
}

.chartTable {
  height: 30vh;
}

.chartTitle {
  display: inline-block;
  margin: 10px;
  font-size: 14px;
  font-weight: bold;
  color: #333;
}

.cardList {
  background-color: #fff;
  padding: 10px;
  padding-top: 0;
}
.item {
  height: 70px;
  width: 150px;
  border-radius: 4px;
  // border: 1px solid #e5e5e5;
  margin-top: 10px;
  display: flex;
  font-family: Microsoft YaHei;
  font-weight: 500;
  font-size: 14px;
  color: #333333;
  line-height: 27px;
  div > p {
    max-width: 150px;
    white-space: nowrap; /* 禁止换行 */
    overflow: hidden; /* 隐藏超出容器的内容 */
    text-overflow: ellipsis; /* 使用省略号表示溢出部分 */
    display: block;
  }
  img {
    height: 32px;
  }
}
.item-data {
  display: flex;
  align-items: center;
  font-size: 12px;
  span {
    margin-left: 8px;
  }
}
.item-data-num {
  font-size: 18px;
  font-weight: bold;
}
.task {
  color: #af4be7;
}
.overTask {
  color: #1bd135;
}
.bTask {
  color: #3ea5d0;
}
.errTask {
  color: #fc1b1b;
}

.router {
  height: 50vh;
  background-color: #fff;
  border-radius: 5px;
  box-shadow: 2px 2px 12px 0 rgba(0, 0, 0, 0.1);
  img {
    width: 122px;
    height: 96px;
  }
}
.router-container {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  grid-gap: 20px 0px;
  padding: 5px;
}
.router-item {
  text-align: center;
  font-size: 14px;
  font-weight: 500;
  color: #333333;
  transition: background-color 0.3s ease;
}
.router-item:hover {
  cursor: pointer;
  background-color: #f7f7f7;
  border-radius: 4px;
  transition: background-color 0.3s ease;
}
.router-title {
  margin-top: -20px;
}
// .chartBottomDiv {
//   display: grid;
//   grid-template-columns: repeat(2, 1fr);
//   grid-gap: 0 10px;
// }
.chartBottom {
  height: 33.5vh;
  margin-top: 10px;

  background-color: #fff;
  border-radius: 5px;
  box-shadow: 2px 2px 12px 0 rgba(0, 0, 0, 0.1);
}

.chartBottomRight {
  height: 33.5vh;
  // margin: 10px;
  margin-top: 10px;
  background-color: #fff;
  border-radius: 5px;
  box-shadow: 2px 2px 12px 0 rgba(0, 0, 0, 0.1);
}
@media screen and (max-width: 992px) {
  .welcome-row {
    display: grid;
    // grid-template-rows: repeat(2, 1fr);
  }
  .welcome-row > div:nth-child(1) {
    grid-row: 2; /* 第一个元素放在第二行 */
    margin-top: 10px;
    display: grid;
    grid-template-columns: repeat(1, 1fr);
  }

  .welcome-row > div:nth-child(2) {
    grid-row: 1; /* 第二个元素放在第一行 */
    margin-top: 10px;
  }
  .router {
    height: auto;
  }
  .router-container {
    grid-template-columns: repeat(4, 1fr);
  }
}
.router-icon {
  min-width: 70px;
  min-height: 100px;
  width: 70px;
  padding-bottom: 20px;
  margin: 0 auto;
  text-align: center;
}
</style>
