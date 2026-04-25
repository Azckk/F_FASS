<script setup lang="ts">
import { onMounted, ref, watch, onUnmounted } from "vue";
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

let lable = ref([]);

let data = ref();
let xData = ref(props.dataList.time);
let y1Data = ref(props.dataList.data);
watch(
  () => props.dataList,
  newVal => {
    xData.value = newVal.time;
    y1Data.value = newVal.data;
    init();
  }
);
let myChart;
function init() {
  if (!main.value) return;
  myChart = echarts.init(main.value);
  const option = {
    backgroundColor: "rgba(255,255,255,0.01)",
    // backgroundColor: "#fff",
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
      }
      // axisLine: {
      //   lineStyle: {
      //     color: "#fff"
      //   }
      // }
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
          color: "#F3F4F4",
          type: "dashed"
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
function handleFullscreenChange() {
  if (myChart && main.value) {
    // 重新调整图表大小
  }
  const viewElem = document.body;
  const resizeObserver = new ResizeObserver(() => {
    // 此处放 当窗口大小发生变化时，想要让宽高自适应的图表的.resize()，除此处外其余都是固定写法，举例如下
    myChart.resize();
  });
  resizeObserver.observe(viewElem);
}
onMounted(() => {
  init();
  // 监听全屏事件
  document.addEventListener("fullscreenchange", handleFullscreenChange);
});

onUnmounted(() => {
  // 移除全屏事件监听器
  document.removeEventListener("fullscreenchange", handleFullscreenChange);
});
</script>

<template>
  <div ref="main" class="main" />
</template>

<style lang="scss" scoped>
.main {
  width: 100%;
  height: calc(100% - 40px);
}
</style>
