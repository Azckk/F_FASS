<script lang="ts" setup>
import { ref, onMounted, onBeforeUnmount } from "vue";
import warning from "@/assets/svg/warning.svg?component";

const props = defineProps({
  info: {
    type: String,
    default: ""
  }
});

onMounted(() => {});
// 在组件卸载时清除定时器
onBeforeUnmount(() => {
  console.log("卸载了！！");
});
</script>

<template>
  <div style="text-align: center">
    <div style="display: flex; justify-content: center">
      <component :is="warning" style="text-align: center" />
    </div>
    <div style="color: #ff6060; font-size: 15px; margin: 10px">
      {{ props.info }}
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
