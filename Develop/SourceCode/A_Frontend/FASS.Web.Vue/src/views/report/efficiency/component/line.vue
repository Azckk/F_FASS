<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
import * as echarts from "echarts";
import dayjs from "dayjs";
import { useMyI18n } from "@/plugins/i18n";
import { watchEffect } from "vue";
const { t, locale } = useMyI18n();
const props = defineProps<{
  title?: string;
  dataList: {
    taskDayList: object;
  };
  // 是否是横向
  isHorizontal?: boolean;
  query?: {
    time: string;
  };
}>();
const main = ref<HTMLElement | null>(null);
let subtext = ref();
subtext.value = setTimeFn(props.query.time);
function setTimeFn(newTime) {
  const [start, end] = newTime;
  let a = dayjs(start).startOf("day").format("YYYY/MM/DD");
  let b = dayjs(end).endOf("day").format("YYYY/MM/DD");
  return `${a} - ${b}`;
}
let lable = ref([t("table.completeTheTask"), t("table.exceptionTasks")]);
watch(
  () => props.query,
  newVal => {
    subtext.value = setTimeFn(newVal.time);
    init();
  },
  { deep: true }
);
let data = ref();
let xData = ref([]);
let y1Data = ref([]);
let y2Data = ref([]);

watch(
  () => props.dataList,
  newVal => {
    data.value = newVal.taskDayList;
    if (data.value) {
      xData.value = [];
      y1Data.value = [];
      y2Data.value = [];
      data.value.forEach(item => {
        xData.value.push(item.day);
        y1Data.value.push(item.success);
        y2Data.value.push(item.failure);
      });
      xData.value = xData.value.map(item => (item = item.split(" ")[0]));
    }
    init();
  }
);

watch(
  () => locale.value,
  newVal => {
    lable.value = [t("table.completeTheTask"), t("table.exceptionTasks")];
    init();
  },
  {
    immediate: true
  }
);

function init() {
  if (!main.value) return;
  const myChart = echarts.init(main.value);
  const option = {
    backgroundColor: "#fff",
    title: {
      left: "18px",
      top: "0",
      textStyle: {
        color: "#999",
        fontSize: 12,
        fontWeight: "400"
      }
    },
    color: ["#5B8FF9", "#FFA18E", "#5AD8A6"],
    tooltip: {
      trigger: "axis",
      axisPointer: {
        type: "cross",
        crossStyle: {
          color: "#999"
        },
        lineStyle: {
          type: "dashed"
        }
      }
    },
    grid: {
      left: "25",
      right: "25",
      bottom: "14",
      top: "45",
      containLabel: true
    },
    legend: {
      data: lable.value,
      left: "20px",
      top: 0,
      icon: "path://M0 2a2 2 0 0 1 2 -2h14a2 2 0 0 1 2 2v0a2 2 0 0 1 -2 2h-14a2 2 0 0 1 -2 -2z",
      textStyle: {
        color: "#333"
      },
      itemWidth: 10,
      itemHeight: 10
    },
    xAxis: {
      type: "category",
      data: xData.value,
      splitLine: {
        show: false
      },
      axisTick: {
        show: true,
        alignWithLabel: true
      },
      axisLine: {
        lineStyle: {
          color: "rgba(0,0,0,0.5)"
        }
      }
    },
    yAxis: {
      type: "value",
      // max: "100",
      // max: max_value>=100? max_value + 100: max_value+10,
      // max: max_value > 100 ? max_value * 2 : max_value + 10,
      // interval: 10,
      // nameLocation: "center",
      axisLabel: {
        color: "#999",
        textStyle: {
          fontSize: 12
        }
      },
      splitLine: {
        show: true,
        lineStyle: {
          color: "#F3F4F4"
        }
      },
      axisLine: {
        show: false
      }
    },
    series: [
      {
        name: lable.value[0],
        type: "line",
        smooth: true,
        data: y1Data.value
      },
      {
        name: lable.value[1],
        type: "line",
        smooth: true,
        data: y2Data.value
      }
    ]
  };

  option && myChart.setOption(option);
}

onMounted(() => {
  init();
  // setInterval(() => {
  //   props.dataList
  // }, 1000);
});
</script>

<template>
  <div ref="main" class="main" />
</template>

<style lang="scss" scoped>
.main {
  width: 100%;
  height: 100%;
}
</style>
