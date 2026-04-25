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
const keys = ref([]);
const values = ref([]);
watch(
  () => props.dataList,
  () => {
    data = props.dataList as any;
    keys.value = [];
    values.value = [];
    values.value = data.map(data => data.alarmCount);
    keys.value = data.map(data => (data.name ? data.name : "未命名"));
    getData();
  }
);

function getData() {
  if (!main.value) return;
  const myChart = echarts.init(main.value);
  var option;
  option = {
    title: {
      text: props.title
    },
    tooltip: {
      trigger: "axis",
      axisPointer: {
        type: "shadow"
      }
    },
    legend: {},
    grid: {
      left: "3%",
      right: "4%",
      bottom: "3%",
      containLabel: true
    },
    xAxis: {
      type: "value",
      boundaryGap: [0, 0.01]
    },
    yAxis: {
      type: "category",
      data: keys.value ? keys.value : "暂无数据"
    },
    series: [
      {
        type: "bar",
        data: values.value ? values.value : "暂无数据"
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
