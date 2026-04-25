<script setup lang="ts">
import { onMounted, watchEffect } from "vue";

defineOptions({
  name: ""
});
const props = defineProps<{
  title: string;
  dataList: object;
}>();
let data = [
  {
    name: "涂布+",
    num: 1
  },
  {
    name: "涂布-",
    num: 1
  },
  {
    name: "辗压+",
    num: 1
  },
  {
    name: "辗压-",
    num: 1
  },
  {
    name: "模切+",
    num: 1
  },
  {
    name: "模切-",
    num: 1
  },
  {
    name: "卷绕+",
    num: 1
  },
  {
    name: "卷绕-",
    num: 1
  }
];
watchEffect(() => {
  const counts = props.dataList;
  Object.keys(counts).forEach((key, index) => {
    data[index].num = counts[key];
  });
});
onMounted(() => {});
</script>

<template>
  <div class="title">{{ props.title }}</div>
  <div class="container">
    <div v-for="(item, index) in data" :key="index" class="item">
      <div class="num">{{ item.num }}</div>
      <div>
        {{ item.name }}
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.title {
  color: #9da8d0;
  font-size: 18px;
  font-weight: 600;
}
.chartWrapper {
  // display: flex;
  // flex-wrap: wrap;
  // justify-content: space-around;
  // text-align: center;
  color: #fff;
  .chartWrapperItem {
    width: 140px;
    height: 80px;
  }
}
.container {
  display: grid;
  grid-template-columns: repeat(2, 1fr); /* 两列等宽 */
  // grid-template-rows: repeat(4, auto);
  grid-gap: 20px; /* 网格间距 */
  padding: 20px;
}

.item {
  color: #fff;
  // background-color: #f0f0f0;
  border-radius: 5px;
  .num {
    font-size: 1.8rem;
    font-weight: 400;
  }
}
</style>
