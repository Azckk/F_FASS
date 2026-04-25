<script setup lang="ts">
import * as echarts from "echarts";
import { ref, onMounted, watch, computed } from "vue";
defineOptions({
  name: ""
});

const props = defineProps<{
  dataList: { value: number; name: string }[];
  color?: string[];
}>();
const main = ref<HTMLElement | null>(null);
const dataList = ref(props.dataList as { value: number; name: string }[]);
watch(
  () => props.dataList,
  () => {
    dataList.value = props.dataList as { value: number; name: string }[];
    getData();
  }
);
function getData() {
  if (!main.value) return;
  const myChart = echarts.init(main.value);
  var option;
  option = {
    tooltip: {
      trigger: "item"
    },
    legend: {
      bottom: "15%",
      left: "center",
      borderRadius: 0
    },
    series: [
      {
        name: "",
        type: "pie",
        radius: ["30%", "50%"],
        center: ["50%", "35%"],
        avoidLabelOverlap: false,
        itemStyle: {},
        label: {
          formatter: "{b}: {c}"
        },
        emphasis: {
          label: {
            show: true,
            fontSize: 20,
            fontWeight: "bold",
            formatter: "{b}: {c}"
          },
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: "rgba(0, 0, 0, 0.5)"
          }
        },
        labelLine: {
          show: true
        },
        data: dataList.value,
        color: props.color
          ? props.color
          : ["#3E95D0", "#994FED", "#E36739", "#15DC7D", "#A5A5A5"]
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
  width: 100%;
  height: 100%;
}
</style>
