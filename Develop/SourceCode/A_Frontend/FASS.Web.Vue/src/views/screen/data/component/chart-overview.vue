<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
import "./index.scss";

defineOptions({
  name: ""
});
const props = defineProps<{
  title?: string;
  dataList: any;
}>();
const localDataList = ref(props.dataList);
// 响应式变量，用于存储解析后的图片路径
const imagePath = ref([""]);
// 异步获取图片路径的函数 处理vite编译后动态导入图片问题
const getImageSrc = async name => {
  const url = new URL(
    `../../../../assets/screen/icon/${name}.png`,
    import.meta.url
  );
  imagePath.value.push(url.pathname);
  return imagePath.value;
};

function setImageSrc() {
  imagePath.value = [];
  for (let i = 0; i < data.length; i++) {
    getImageSrc(data[i].name);
  }
}
let data = [
  {
    name: "今日任务总数",
    icon: "@/assets/screen/icon/今日任务总数.png",
    num: props.dataList.total
  },
  {
    name: "任务已完成",
    icon: "/src/assets/screen/icon/任务已完成.png",
    num: props.dataList.success
  },
  {
    name: "异常任务",
    icon: "/src/assets/screen/icon/异常任务.png",
    num: props.dataList.failure
  },
  {
    name: "在线车辆",
    icon: "/src/assets/screen/icon/在线车辆.png",
    num: props.dataList.carOnline
  },
  {
    name: "在线充电桩",
    icon: "/src/assets/screen/icon/在线充电桩.png",
    num: props.dataList.cahrgeOnline
  },
  {
    name: "今日报警总数",
    icon: "/src/assets/screen/icon/今日报警总数.png",
    num: props.dataList.totalAlarm
  }
];

// 使用 watch 监听 dataList 的变化，并且深度监听
watch(
  () => props.dataList,
  newDataList => {
    console.log("数据更新：", newDataList);
    data = [
      {
        name: "今日任务总数",
        icon: "@/assets/screen/icon/今日任务总数.png",
        num: newDataList.total
      },
      {
        name: "任务已完成",
        icon: "/src/assets/screen/icon/任务已完成.png",
        num: newDataList.success
      },
      {
        name: "异常任务",
        icon: "/src/assets/screen/icon/异常任务.png",
        num: newDataList.failure
      },
      {
        name: "在线车辆",
        icon: "/src/assets/screen/icon/在线车辆.png",
        num: newDataList.carOnline
      },
      {
        name: "在线充电桩",
        icon: "/src/assets/screen/icon/在线充电桩.png",
        num: newDataList.cahrgeOnline
      },
      {
        name: "今日报警总数",
        icon: "/src/assets/screen/icon/今日报警总数.png",
        num: newDataList.totalAlarm
      }
    ];
    setImageSrc();
  },
  { deep: true } // 深度监听对象
);

function getItemId(index) {
  if (index === 2) {
    return "third-item";
  } else if (index === 5) {
    return "sixth-item";
  } else if (index === 1) {
    return "tow-item";
  }
  return "";
}

onMounted(() => {
  setImageSrc();
});
</script>

<template>
  <!-- <div class="title">
    <div class="a" />
    {{ props.title }}
  </div> -->
  <div class="container">
    <div v-for="(item, index) in data" :key="index" class="item">
      <div class="icon">
        <img :src="imagePath[index]" alt="icon" />
        <!-- <img :src="item.icon" alt="icon" /> -->
      </div>
      <div class="text">
        <p>{{ item.name }}</p>
        <p :id="getItemId(index)" class="number">{{ item.num }}</p>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.container {
  display: grid;
  grid-template-columns: repeat(3, 1fr); /* 两列等宽 */
  grid-template-rows: repeat(2, 1fr);
  overflow: hidden;
  grid-gap: 20px; /* 网格间距 */
  padding: 20px;
  margin-top: 20px;
}

.item {
  color: #fff;
  // background-color: #f0f0f0;
  border-radius: 5px;
  min-height: 130px;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
  .text {
    font-size: 14px;
    font-weight: 400;
    text-align: center;
    margin: 0 auto;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    .number {
      font-size: 24px;
      color: #61c6f0;
    }
  }
}
#tow-item {
  /* 第二个元素的样式 */
  color: #1bd135;
}
#third-item,
#sixth-item {
  /* 第三个元素的样式 */
  color: #fc1b1b;
}
</style>
