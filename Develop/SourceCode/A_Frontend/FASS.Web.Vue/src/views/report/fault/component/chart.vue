<script setup lang="ts">
import * as echarts from "echarts";
import { ref, onMounted, watch } from "vue";
defineOptions({
  name: ""
});

const props = defineProps<{
  title: string;
  dataList: object;
  // 是否是空心
  isHollow?: boolean;
}>();
const main = ref<HTMLElement | null>(null);
let data = {
  name: [],
  value: []
};
let isHollow = ["0%", "80%"];
if (props.isHollow) {
  isHollow = ["50%", "80%"];
}
let formattedData = ref();
watch(
  () => props.dataList,
  () => {
    data = props.dataList;
    formattedData.value = data.map(data => {
      return { value: data.alarmCount, name: data.name ? data.name : "未命名" };
    });
    getData();
  }
);
data = props.dataList as any;
function getData() {
  if (!main.value) return;
  const myChart = echarts.init(main.value);
  var option;
  option = {
    title: {
      text: props.title,
      textStyle: {
        color: "#333"
      }
    },
    tooltip: {
      trigger: "item"
    },
    legend: {
      orient: "vertical",
      left: "right",
      textStyle: {
        color: "#333",
        fontSize: 12
      }
    },
    series: [
      {
        name: "分类",
        type: "pie",
        radius: isHollow,
        avoidLabelOverlap: false,
        label: {
          show: true,
          position: "outside"
        },
        labelLine: {
          show: true,
          showAbove: true
        },
        data: formattedData.value
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
  <div id="main-pie" ref="main"></div>
</template>

<style lang="scss" scoped>
#main-pie {
  width: 100%;
  height: 100%;
}
</style>
