<script lang="ts" setup>
import { getConfig } from "@/config";
import { ref, onMounted, watch, watchEffect, onBeforeUnmount } from "vue";
import { getAppName } from "@/api/user";

const TITLE = getConfig("Title");
const AppName = ref();

// 将 errMessage 变为响应式
// const errMessage = ref(1);

const getAppNameToShow = async () => {
  let { data } = await getAppName();
  AppName.value = data.appName;
};
// watch(
//   () => errMessage,
//   (newVal, oldVal) => {
//     console.log("new+++++++", newVal);

//     if (newVal == 60) {
//       console.log("错误！！！");

//       alert("错误！！！！");
//     }
//   },
//   { deep: true } // 深度监听对象的属性变化
// );
watchEffect(async () => {
  // console.log("props.data", props.data.code.text);
  // console.log("props.data.id", props.data.id);
  // if (errMessage.value % 20 == 0) {
  //   console.log("错误！！！");
  //   alert("错误！！！！");
  // }
});
// let timer: ReturnType<typeof setInterval>;
onMounted(() => {
  getAppNameToShow();
  // timer = setInterval(() => {
  //   errMessage.value += 1;
  //   console.log(errMessage);
  // }, 1000);
});
// 在组件卸载时清除定时器
onBeforeUnmount(() => {
  console.log("卸载了！！");

  // clearInterval(timer);
});
</script>

<template>
  <footer
    class="layout-footer text-[rgba(0,0,0,0.6)] dark:text-[rgba(220,220,242,0.8)]"
  >
    Copyright © 2024
    <a class="hover:text-primary" href="/"> &nbsp;{{ TITLE }} </a>
    <p>&emsp;{{ AppName }}</p>
  </footer>
</template>

<style lang="scss" scoped>
.layout-footer {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  padding: 0 0 8px;
  font-size: 14px;
}
</style>
