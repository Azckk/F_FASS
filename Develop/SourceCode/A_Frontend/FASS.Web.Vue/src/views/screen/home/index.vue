<script setup lang="ts">
import Header from "./component/header.vue";
import Main from "../data/index.vue";
import { ref, onMounted, onUnmounted } from "vue";
import screenfull from "screenfull";
defineOptions({
  name: ""
});
const screen = ref(null);
// 是否全屏
const isFullscreen = ref(false);
const isFull = ref(false);
// 监听变化
const change = () => {
  isFull.value = screenfull.isFullscreen;
  isFullscreen.value = screenfull.isFullscreen;
};
// 切换事件
const toggleFullscreen = () => {
  // screenfull.toggle(screen.value);
  if (!screenfull.isEnabled) {
    return false;
  }
  const bodyNode = document.querySelector("body");
  //screenfull.toggle 此方法是执行全屏化操作。如果已是全屏状态，则退出全屏
  screenfull.toggle(bodyNode);
};
//数据大屏自适应函数
const handleScreenAuto = (): void => {
  const designDraftWidth = 1920; //设计稿的宽度
  const designDraftHeight = 1080; //设计稿的高度
  //根据屏幕的变化适配的比例
  const scale =
    document.documentElement.clientWidth /
      document.documentElement.clientHeight <
    designDraftWidth / designDraftHeight
      ? document.documentElement.clientWidth / designDraftWidth
      : document.documentElement.clientHeight / designDraftHeight;
  //缩放比例
  (document.querySelector("#screen") as any).style.transform =
    `scale(${scale}) translate(-50%)`;
};

onMounted(() => {
  screenfull.on("change", change);
  // //初始化自适应  ----在刚显示的时候就开始适配一次
  handleScreenAuto();
  //绑定自适应函数   ---防止浏览器栏变化后不再适配
  window.onresize = () => handleScreenAuto();
});

onUnmounted(() => {
  screenfull.off("change", change);
});
</script>

<template>
  <div ref="screen" class="screen-root" :class="{ full: isFull }">
    <div class="screen" id="screen">
      <Header
        class="header"
        :toggleFullscreen="toggleFullscreen"
        :isFullscreen="isFullscreen"
      />
      <Main class="main" />
    </div>
  </div>
</template>

<style lang="scss" scoped>
#screen {
  margin: 0;
  // width: 100%;
  height: 200%; // 适配非1k屏幕
  background: url("@/assets/screen/bg.png");
}
.screen-root {
  height: 100%;
  width: 100%;
}
.screen {
  // display: inline-block;
  width: 1920px; //设计稿的宽度
  height: 1080px; //设计稿的高度
  transform-origin: 0 0;
  position: absolute;
  left: 50%;
}
.full {
  position: fixed !important;
  z-index: 1100;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  width: 100% !important;
  height: 100% !important;
  margin: 0 !important;
}
.header {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
}
</style>
