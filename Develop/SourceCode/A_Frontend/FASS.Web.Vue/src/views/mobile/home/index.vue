<script setup lang="ts">
import Header from "./components/header.vue";
import Main from "./components/main.vue";
</script>

<template>
  <div class="mobile-home">
    <div class="header">
      <Header />
    </div>
    <div style="padding-top: 70px" />
    <Main class="main-content" />
  </div>
</template>

<style scoped>
.mobile-home {
  /* position: relative; */
  height: 100%;
}
.main-content {
  margin: 0 !important;
  padding: 0;
  /* height: calc(100vh - 60px); 设置内容区域高度，减去头部高度 */
  /* height: 1rem;  */
  overflow: hidden;
  overflow-y: auto; /* 允许内容溢出时垂直滚动 */
}
.header {
  height: 5rem;
  width: 100%;
  position: absolute;
  top: 0px;
  z-index: 999;
}
</style>
