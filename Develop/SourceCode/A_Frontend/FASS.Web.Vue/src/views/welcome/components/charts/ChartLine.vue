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
    time: string[];
    data: number | string[];
  };
  // 是否是横向
  isHorizontal?: boolean;
  query?: {
    time: string;
  };
}>();
const main = ref<HTMLElement | null>(null);

let lable = ref([t("table.completeTheTask"), t("table.exceptionTasks")]);

let data = ref();
let xData = ref();
let y1Data = ref();
watch(
  () => props.dataList,
  newVal => {
    xData.value = newVal.time;
    y1Data.value = newVal.data;
    init();
  }
);
watch(
  () => locale.value,
  newVal => {
    lable.value = ["任务达成率对比（近7天）"];
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
      // boundaryGap: false,
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
        // smooth: true, // 圆滑曲线
        data: y1Data.value,
        areaStyle: {
          opacity: 0.3
        }
      }
    ]
  };

  option && myChart.setOption(option);
}

onMounted(() => {
  init();
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
