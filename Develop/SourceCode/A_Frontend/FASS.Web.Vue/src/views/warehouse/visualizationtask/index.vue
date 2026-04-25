<script setup lang="ts">
import { onMounted, provide, ref } from "vue";
import Menu from "./menu/index.vue";
import SendTask from "./sendwork/index.vue";
import Panel from "./panel/index.vue";
import { useHook } from "./utils/hook";

const {
  dataList,
  tagListData,
  areaList,
  handleGetList,
  // handleSendWorkFn,
  handleSendWorkFns,
  handleMaterial,
  handleContainer,
  handleSetStartPoin,
  deviceDetection
} = useHook();
defineOptions({
  name: ""
});
/// 先看state 有容器 有物料   满桶
/// 有容器 无物料   空桶
/// 无容器 有标签   只有盖子
/// 无容器 无标签   空库位
const tagList = {
  full: { text: "有容器有物料", color: "#42c457" },
  empty: { text: "有容器无物料", color: "#10AAEB" },
  lid: { text: "空库位", color: "#cccccc" },
  storage: { text: "任务中库位", color: "#B75DC4" }
};
</script>

<template>
  <div
    class="common-layout"
    :class="['flex', 'justify-between', deviceDetection() && 'flex-wrap']"
  >
    <el-container>
      <el-aside class="container">
        <SendTask
          :handleSendWorkFn="handleSendWorkFn"
          :handleSendWorkFns="handleSendWorkFns"
        />
      </el-aside>

      <el-main class="panel">
        <Panel
          :dataList="dataList"
          :tagList="tagList"
          :tagListData="tagListData"
          :handleContainer="handleContainer"
          :handleMaterial="handleMaterial"
          :handleSetStartPoin="handleSetStartPoin"
        />
        <div class="flex justify-center mt-10">
          <div
            v-for="item in Object.values(tagList)"
            :key="item.color"
            class="flex items-center mx-2"
          >
            <div
              class="h-4 w-4 mr-1"
              :style="{ 'background-color': item.color }"
            />
            <span>{{ item.text }}</span>
          </div>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<style lang="scss" scoped>
.common-layout {
  background-color: #fff;
  padding: 20px;
  // box-sizing: border-box;
}
.mt-10 {
  margin-top: 20px;
}
.panel {
  padding-top: 0;
}
.container {
  width: 120px;
  padding-top: 6px;
}
</style>
