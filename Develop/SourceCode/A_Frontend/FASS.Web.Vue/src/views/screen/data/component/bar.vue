<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
import * as echarts from "echarts";
import "./index.scss";

const props = defineProps<{
  title: string;
  dataList: object;
  // 是否是横向
  isHorizontal?: boolean;
}>();
const main = ref<HTMLElement | null>(null);
let data = {
  name: [],
  value: []
};
let isHorizontal = ["value", "category", "center"];
if (props.isHorizontal) {
  isHorizontal = ["category", "value", "right"];
}
watch(
  () => props.dataList,
  newVal => {
    data = newVal as any;
    getData();
  },
  {
    immediate: true,
    deep: true
  }
);
data = props.dataList as any;

function getData() {
  if (!main.value) return;
  const myChart = echarts.init(main.value);
  const option = {
    grid: {
      x: 60,
      y: 45,
      x2: 60,
      y2: 0,
      borderWidth: 1
    },
    title: {
      text: props.title ? props.title : "柱状图",
      textStyle: {
        color: "#9da8d0"
      }
    },
    tooltip: {
      trigger: "axis",
      axisPointer: {
        type: "shadow"
      }
    },
    xAxis: {
      type: isHorizontal[1],
      data: data.name,
      show: !isHorizontal
    },
    yAxis: {
      type: isHorizontal[0],
      data: data.name,
      splitLine: {
        show: !isHorizontal
      }
    },
    series: [
      {
        name: "Income",
        type: "bar",
        stack: "Total",
        label: {
          show: true,
          position: isHorizontal[3]
        },
        data: data.value
      }
    ]
  };

  option && myChart.setOption(option);
}

onMounted(() => {
  getData();
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
