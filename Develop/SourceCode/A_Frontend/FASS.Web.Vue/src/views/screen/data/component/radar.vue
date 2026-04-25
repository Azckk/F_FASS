<script setup lang="ts">
import { onMounted, ref, onUnmounted, watch, watchEffect, toRef } from "vue";
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
  value: [],
  alarmLevel: []
};
// let isHorizontal = ["value", "category", "center"];
// if (props.isHorizontal) {
//   isHorizontal = ["category", "value", "right"];
// }
data = props.dataList as any;
watchEffect(() => {
  data = props.dataList.columnarChartData as any;
  getData();
});

// @ts-ignore
const transformedData = data?.data
  .map((value, index) => {
    const name = data.name[index];
    const alarmLevel = data.alarmLevel[index];
    return name ? { name, value, alarmLevel } : null; // 如果 name 为空则忽略
  })
  .filter(item => item !== null); // 过滤掉空的项
// 找出 value 最大的对象
const maxValueItem = transformedData?.reduce((maxItem, currentItem) => {
  return currentItem.value > (maxItem?.value || 0) ? currentItem : maxItem;
}, null);
// console.log("transformedData", maxValueItem, transformedData);
const categories = transformedData;
let myChart;
var color = ["#3EA5D0", "#EBDA20", "#F10E0E"];
categories?.map(function (e, i) {
  var cor = color[Number(e.alarmLevel)];
  return cor;
});

function getData() {
  if (!main.value) return;
  myChart = echarts.init(main.value);
  const option = {
    // backgroundColor: "#000928",
    color: ["#3D91F7", "#61BE67"],
    tooltip: {},
    legend: {
      show: true,
      icon: "circle",
      bottom: 30,
      center: 0,
      itemWidth: 14,
      itemHeight: 14,
      itemGap: 21,
      orient: "horizontal",
      data: ["a", "b"],
      textStyle: {
        fontSize: "70%",
        color: "#8C8C8C"
      }
    },

    radar: {
      // shape: 'circle',
      radius: "60%",
      triggerEvent: true,
      center: ["50%", "40%"],
      name: {
        textStyle: {
          // color: color[0],
          fontSize: "14",
          borderRadius: 3,
          padding: [3, 5]
        }
      },

      nameGap: "2",
      indicator: categories?.map(category => ({
        name: category.name,
        max: maxValueItem.value + 1, // 根据最大值动态设置
        color: color[Number(category.alarmLevel)],
        itemStyle: {
          color: color[Number(category.alarmLevel)]
        }
      })),
      splitArea: {
        areaStyle: {
          color: [
            "rgba(227, 227, 227, 0.1)",
            "rgba(227, 227, 227, 0.2)",
            "rgba(227, 227, 227, 0.3)",
            "rgba(227, 227, 227, 0.4)",
            "rgba(227, 227, 227, 0.5)"
          ].reverse()
        }
      },
      // axisLabel:{//展示刻度
      //     show: true
      // },
      angleAxis: {
        type: "category",
        boundaryGap: false,
        clockwise: false
      },
      axisLine: {
        //指向外圈文本的分隔线样式
        lineStyle: {
          color: "rgba(0,0,0,0)"
        }
      },
      splitLine: {
        lineStyle: {
          width: 2,
          color: [
            // 描边
            // "rgba(227, 227, 227, 0.1)",
            // "rgba(227, 227, 227, 0.2)",
            // "rgba(227, 227, 227, 0.3)",
            // "rgba(227, 227, 227, 0.4)",
            // "rgba(227, 227, 227, 0.5)"
          ].reverse()
        }
      }
    },
    // angleAxis: {
    //   type: "category",
    //   boundaryGap: false,
    //   clockwise: false
    // },
    series: [
      {
        name: "报警统计",
        type: "radar",
        symbol: "circle",
        tooltip: {
          trigger: "item"
        },
        symbolSize: 10,
        visualMap: {
          show: false,
          min: 0,
          max: 2, // 根据 alarmLevel 的值范围设置
          inRange: {
            color: ["#00FF00", "#FFFF00", "#FFA500"] // 对应不同 alarmLevel 的颜色
          }
        },
        itemStyle: {
          color: categories?.map(category => {
            let aaa = color[Number(category.alarmLevel)];
            return aaa.toString();
          })
          // let a = "";
          // console.log(categories);

          // for (let i = 0; i < categories.length; ++i) {
          //   let aaa = color[Number(categories[i].alarmLevel)];
          //   a = aaa;
          //   console.log(a);
          //   // return aaa;
          // }
          // return a;
          // color: "#F10E0E"
        },
        // itemStyle: {
        //   normal: {
        //     color: "#fff",
        //     borderColor: "#009afe",
        //     borderWidth: 2
        //   }
        // },
        // 数据图
        areaStyle: {
          normal: {
            color: "rgba(150, 41, 235, 0.5)"
          }
        },
        // 边框
        lineStyle: {
          normal: {
            color: "rgba(150, 41, 235, 0.8)",
            width: 1
          }
        },
        data: [
          {
            value: categories?.map(category => category.value),
            name: "车辆报警统计（今日top10）"
            // itemStyle: {
            //   color: params => {
            //     console.log(params);
            //     return color[Number(categories[params.dataIndex].alarmLevel)];
            //   }
            // }
          }
        ]
      }
      // {
      //   name: "",
      //   type: "scatter",
      //   coordinateSystem: "polar",
      //   symbolSize: 10,
      //   data: [
      //     [0, 0, 5],
      //     [0, 1, 1]
      //   ]
      // }
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

<template>
  <div ref="main" class="main" />
</template>

<style lang="scss" scoped>
.main {
  width: 100%;
  height: 100%;
}
</style>
