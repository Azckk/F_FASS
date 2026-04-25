<script setup lang="ts">
// import ScaleBox from "vue3-scale-box";
import Radar from "./component/radar.vue";
import Pie from "./component/pie.vue";
import chartOverview from "./component/chart-overview.vue";
import Table from "./component/table.vue";
import ChartLine from "./component/ChartLine.vue";
import { useHook } from "./utils/hook";
import { onMounted, reactive, ref, watch } from "vue";
const { dataList, getDataFn } = useHook();
// @ts-ignore
import { chartData } from "@/views/welcome/data.ts";
import "./component/index.scss";
type dataListType = {
  carState: any;
};
// const dataList = ref();
let carStatisticsData = {};
let carStatisticsData2 = {};
let carStatisticsData3 = {};
let carSummaryData = {};
let carSummaryData6 = ref({
  data: [],
  color: ["#3E95D0", "#994FED", "#15DC7D", "#E36739", "#A5A5A5"]
});
let storageData = ref({});
let chargeStateData = ref({});
let taskData = ref({});
let alarmData = ref({
  columnarChartData: {},
  totalAlarm: 0
});
let taskCount = ref({
  success: 1, // 已完成
  failure: 2, // 失败
  total: 33, // 总数
  totalAlarm: 3, // 报警总数
  cahrgeOnline: 4, // 在线充电桩
  carOnline: 5 // 在线车辆
});
let isCareatd = ref(false);
watch(
  dataList,
  (newVal, oldVal) => {
    carSummaryData6.value = dataList.value.carState;
    storageData.value = dataList.value.storage;
    chargeStateData.value = dataList.value.chargeState;
    taskData.value = dataList.value.rates;
    taskCount.value = {
      success: dataList.value.taskCount[0].success, // 已完成
      failure: dataList.value.taskCount[0].failure, // 失败
      total: dataList.value.taskCount[0].total, // 总数
      totalAlarm: dataList.value.alarm.totalAlarm, // 报警总数
      cahrgeOnline: dataList.value.cahrgeOnline.value, // 在线充电桩
      carOnline: dataList.value.carOnline.value // 在线车辆
    };
    alarmData.value =
      newVal.alarm.columnarChartData.name.length > 0
        ? newVal.alarm
        : {
            columnarChartData: {
              data: [1],
              name: ["暂无数据"],
              alarmLevel: [1]
            },
            totalAlarm: 0
          };
    // alarmData = {
    //   columnarChartData: {
    //     data: [7, 5, 8, 4, 6, 6, 6, 4, 4, 5],
    //     name: [
    //       "伸缩叉遇障",
    //       "左碰撞条触发停止机器人",
    //       "安全触边2报警",
    //       "称重传感器超重",
    //       "地标读取错误报警",
    //       "货物开关误触发",
    //       "货物探测-未检测到货物",
    //       "平移电机CAN异常",
    //       "前舵转向CAN异常",
    //       "上举未到位"
    //     ],
    //     alarmLevel: [0, 0, 0, 1, 1, 1, 2, 2, 2, 1]
    //   },
    //   totalAlarm: 18
    // };
    // tableData = dataList.value.data5;
    // isCareatd.value = false;
    isCareatd.value = true;
  },
  {
    deep: true
  }
);

defineOptions({
  name: ""
});

onMounted(async () => {
  // let res = await getDataFn();
  // dataList.value = res;
});
</script>
<template>
  <div v-if="isCareatd" class="main-content">
    <div class="left">
      <div class="left-top item">
        <div class="title">
          <div class="a" />
          机器人状态统计（今日）
        </div>
        <Pie :dataList="carSummaryData6" :isHollow="true" />
      </div>
      <div class="left-bottom item">
        <div class="title">
          <div class="a" />
          仓储库位监控(今日)
        </div>
        <Pie :dataList="storageData" :color="[]" :isHollow="true" />
      </div>
    </div>
    <div class="middle">
      <div class="middle-top item cw">
        <div class="title">
          <div class="a" />
          运行监控(今日)
        </div>
        <chartOverview title="" :dataList="taskCount" />
      </div>
      <div class="middle-bottom item">
        <!-- <Table title="车辆实时报警" :tableData="tableData" /> -->
        <div class="title">
          <div class="a" />
          任务达成率对比（近7天）
        </div>
        <ChartLine class="chartTable" :dataList="taskData" />
      </div>
    </div>
    <div class="right">
      <div class="right-top item">
        <!-- <Bar title="车辆状态统计" :dataList="carStatisticsData3" /> -->
        <div class="title">
          <div class="a" />
          充电桩状态统计（今日）
        </div>
        <Pie
          title=""
          :dataList="chargeStateData"
          :color="['#15DC7D', '#3E95D0', '#A5A5A5']"
          :isHollow="true"
        />
      </div>
      <div class="right-bottom item">
        <div class="title">
          <div class="a" />
          车辆报警统计（今日top10）
        </div>
        <Radar title="" :dataList="alarmData" />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.main-content {
  margin-top: 0px;
  padding-top: 90px !important;
  background: url("@/assets/screen/bg.png");
}
.main-content {
  display: grid;
  grid-template-columns: repeat(3, 1fr); /* 3 列，等宽 */
  grid-template-rows: repeat(2, auto); /* 2 行，高度 200px */
  gap: 20px; /* 网格间距 */
  padding: 20px;
  .left {
    display: grid;
    grid-template-columns: repeat(1, 1fr); /* 3 列，等宽 */
    grid-template-rows: repeat(2, 480px); /* 2 行 */
    // align-content: start;
  }
  .middle {
    display: grid;
    grid-template-columns: repeat(1, 1fr); /* 3 列，等宽 */
    grid-template-rows: repeat(2, 480px); /* 2 行*/
    overflow: hidden;
    .middle-bottom {
      // margin-top: 30px;
      overflow: hidden;
      overflow-y: visible;
      -webkit-overflow-scrolling: touch;
    }
    .middle-bottom::-webkit-scrollbar {
      display: none; /* Chrome、Safari 和 Opera */
    }
  }
  .right {
    display: grid;
    grid-template-columns: repeat(1, 1fr); /* 3 列，等宽 */
    grid-template-rows: repeat(2, 480px); /* 2 行 */
  }
}
.item {
  flex: 1;
  width: 100%;
  background: url(@/assets/screen/bgk.png);
  background-size: 100% 100%;
  margin-top: 20px;
}
.cw {
  background: url("");
}
</style>
