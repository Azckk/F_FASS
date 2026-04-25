<script setup lang="ts">
import * as echarts from "echarts";
import { ref, onMounted, watch, watchEffect, nextTick } from "vue";
defineOptions({
  name: ""
});
type Data = {
  dataList: number[];
  name?: string[];
  alarmLevel?: number[];
  data?: number[];
};
const props = defineProps<{
  dataList: Data;
  name?: string[];
}>();
const dataList = ref(props.dataList);
const name = ref(props.name);

const main = ref<HTMLElement | null>(null);
// watch(
//   () => props.dataList,
//   newVal => {
//     console.log(newVal);
//     if (newVal.length == 0) {
//       dataList.value = [0];
//       name.value = [""];
//     }
//     getData();
//   },
//   { deep: true, immediate: true }
// );
watchEffect(() => {
  dataList.value.dataList = props.dataList.data;
  dataList.value.name = props.dataList.name;
  dataList.value.alarmLevel = props.dataList.alarmLevel;
  getData();
  // console.log("dataList", dataList.value);
});
// 模拟数据
// dataList.value.dataList = [100, 60, 80];
// dataList.value.alarmLevel = [0, 1, 2];
// dataList.value.name = [
//   "1#障碍物检测减速停止区报警1111111111111111111111111111111111111111111111111111111111111111111111111",
//   "地标读取错误报警",
//   "2#驱动器通讯异常"
// ];

function getData() {
  if (!main.value) return;
  const myChart = echarts.init(main.value);
  var option;
  option = {
    tooltip: {
      trigger: "axis",
      axisPointer: {
        type: "shadow"
      }
    },

    grid: {
      top: "5% ",
      left: "25px",
      right: "4%",
      containLabel: true
    },
    xAxis: {
      type: "value",
      boundaryGap: [0, 0.01],
      splitLine: { show: false },
      axisLine: {
        show: false // 隐藏Y轴线
      },
      axisTick: {
        show: false // 刻度
      },
      axisLabel: {
        show: false // 标签
      }
    },
    yAxis: {
      type: "category",
      data: dataList.value.name,
      splitLine: { show: false },
      axisLine: {
        show: false // 隐藏Y轴线
      },
      axisTick: {
        show: false // 刻度
      },
      axisLabel: {
        // show: false, // 标签
        // align: "left", // 标签左对齐
        width: 100, // 固定宽度
        overflow: "break"
        // padding: [0, 0, 0, -100] // 根据需要调整左边距
        // formatter: function (value, index) {
        //   if (index == 0) {
        //     return `${10 - index} ${value}`;
        //   } else {
        //     return `0${10 - index} ${value}`;
        //   }
        // }
      }
    },
    series: [
      {
        type: "bar",
        data: dataList.value.dataList,
        barWidth: 30, //柱图宽度
        itemStyle: {
          normal: {
            color: function (params: any) {
              if (dataList.value.alarmLevel) {
                const alarmValue = dataList.value.alarmLevel[params.dataIndex];
                console.log("alarmValue", alarmValue);
                // 根据 Alarm 的值设置不同的颜色
                if (alarmValue == 0) {
                  return "#3EA5D0";
                } else if (alarmValue == 1) {
                  return "#EBDA20";
                } else if (alarmValue == 2) {
                  return "#F10E0E";
                } else {
                  return "#994FED"; // 默认颜色
                }
              } else {
                return "#994FED"; // 默认颜色
              }
            },
            barBorderRadius: [0, 20, 20, 0]
          }
        }
      }
    ]
  };

  option && myChart.setOption(option);
}
onMounted(() => {
  getData();
});
</script>

<script lang="ts">
export default {
  name: ""
};
</script>

<template>
  <div id="main-pie" ref="main" />
</template>

<style lang="scss" scoped>
#main-pie {
  width: 90%;
  height: 100%;
}
</style>
