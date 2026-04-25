<script setup lang="ts">
import * as echarts from "echarts";
import "./index.scss";
import { ref, onMounted, watch, onUnmounted, toRefs } from "vue";
defineOptions({
  name: ""
});

const props = defineProps<{
  title?: string;
  dataList: object;
  // 是否是空心
  isHollow?: boolean;
  color?: string[];
}>();
const main = ref<HTMLElement | null>(null);
let data = {
  name: [],
  value: []
};
let isHollows = ["0%", "70%"];
if (props.isHollow) {
  isHollows = ["40%", "60%"];
}
watch(
  () => props.dataList,
  () => {
    data = props.dataList as any;
    getData();
  }
);
data = props.dataList as any;
let myChart: echarts.ECharts | null = null;
function getData() {
  if (!main.value) return;
  myChart = echarts.init(main.value);
  var option;
  option = {
    title: {
      text: props.title,
      textStyle: {
        color: "#9da8d0"
      }
    },
    tooltip: {
      trigger: "item"
    },
    legend: {
      // orient: "vertical",
      bottom: 50,
      textStyle: {
        color: "#fff",
        fontSize: 12
      },
      itemWidth: 10, // 设置图例标记的宽度（圆的直径）
      itemHeight: 10, // 设置图例标记的高度（圆的直径）
      itemStyle: {
        borderWidth: 1 // 边框宽度
      }
    },
    series: [
      {
        name: "分类",
        type: "pie",
        radius: isHollows,
        center: ["50%", "40%"],
        avoidLabelOverlap: false,
        label: {
          show: true,
          position: "outside",
          formatter: "{c}:{b}",
          textStyle: {
            color: "#fff"
          }
        },
        labelLine: {
          show: true,
          showAbove: true
        },
        data: data,
        color: props.color
          ? props.color
          : ["#3E95D0", "#994FED", "#E36739", "#15DC7D", "#A5A5A5"]
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
  getData();
  // 监听全屏事件
  document.addEventListener("fullscreenchange", handleFullscreenChange);
});

onUnmounted(() => {
  // 移除全屏事件监听器
  document.removeEventListener("fullscreenchange", handleFullscreenChange);
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
  width: 100%;
  height: 100%;
}
</style>
