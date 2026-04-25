<script setup lang="ts">
import Menu from "./menu/index.vue";
import SendTask from "./sendwork/index.vue";
import Panel from "./panel/index.vue";
import { useHook } from "./utils/hook";
const {
  dataList,
  tagListData,
  areaList,
  handleGetList,
  handleSendWorkFn,
  handleUpdate
} = useHook();
defineOptions({
  name: ""
});
/// 先看state 有容器 有物料   满桶
/// 有容器 无物料   空桶
/// 无容器 有标签   只有盖子
/// 无容器 无标签   空库位
const tagList = {
  full: { text: "满桶", color: "#42c456" },
  empty: { text: "空桶", color: "#11aaea" },
  lid: { text: "只有盖子", color: "#b75cc4" },
  storage: { text: "空库位", color: "#cccccc" }
};
</script>

<template>
  <div class="common-layout">
    <el-container>
      <!-- <el-aside> -->
      <Menu :areaList="areaList" :handleGetList="handleGetList" />
      <!-- <div style="height: 50px" /> -->
      <!-- <SendTask :handleSendWorkFn="handleSendWorkFn" /> -->
      <!-- </el-aside> -->
      <el-main>
        <Panel
          :dataList="dataList"
          :tagList="tagList"
          :tagListData="tagListData"
          :handleUpdate="handleUpdate"
        />
        <div class="flex justify-center mt-4 testaaaaa">
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
}
::v-deep(.el-container) {
  display: block;
}
</style>
