<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
import * as echarts from "echarts";
import dayjs from "dayjs";
import { useMyI18n } from "@/plugins/i18n";
const { t, locale } = useMyI18n();
interface Query {
  time: string; // 或者根据具体情况定义其他属性类型
}

const props = defineProps<{
  title?: string;
  dataList: object;
  // 是否是横向
  isHorizontal?: boolean;
  query?: Query;
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
let lable = ref([
  t("table.chargeElectricalEnergy"),
  t("table.dischargeElectricalEnergy")
]);
watch(
  () => locale.value,
  newVal => {
    lable.value = [
      t("table.chargeElectricalEnergy"),
      t("table.dischargeElectricalEnergy")
    ];
    init();
  },
  {
    immediate: true
  }
);
watch(
  () => props.query,
  newVal => {
    subtext.value = setTimeFn(newVal.time);
    init();
  },
  { deep: true }
);
let data = ref();

watch(
  () => props.dataList,
  newVal => {
    data.value = newVal;
    init();
  }
);

function init() {
  if (!main.value) return;
  const myChart = echarts.init(main.value);
  const option = {
    backgroundColor: "#fff",
    title: {
      text: props.title ? props.title : "",
      // subtext: subtext.value ? subtext.value : "",
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
      data: data.value
        ? data.value.chargeList.map(item => item.chargeTime)
        : null,
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
        name: lable.value[1],
        type: "line",
        smooth: true,
        data: data.value ? data.value.disChargeList.map(item => item.dn) : null
      },
      {
        name: lable.value[0],
        type: "line",
        smooth: true,
        data: data.value ? data.value.chargeList.map(item => item.dn) : null
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
