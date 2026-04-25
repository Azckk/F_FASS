<script setup lang="ts">
import Menu from "./menu/index.vue";
import SendTask from "./sendwork/index.vue";
import Panel from "./panel/index.vue";
import Header from "../home/components/header.vue";
import { useHook } from "./utils/hook";
import { reactive } from "vue";
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
const newFormInline = reactive({
  blockingType: ""
});
</script>

<template>
  <div style="height: 5rem"><Header class="header" /></div>
  <div class="common-layout">
    <re-col :value="24" :xs="12" :sm="12">
      <el-select
        v-model="newFormInline.blockingType"
        filterable
        class="!w-[100%]"
        :placeholder="$t('table.pleaseSelect')"
        @change="handleGetList"
      >
        <el-option value="all" label="全部">全部</el-option>
        <el-option
          v-for="item in areaList"
          :key="item.code"
          :label="item.name"
          :value="item.id"
        />
      </el-select>
    </re-col>
    <div class="flex justify-center mt-4">
      <div
        v-for="item in Object.values(tagList)"
        :key="item.color"
        class="flex items-center mx-2"
      >
        <div class="h-4 w-4 mr-1" :style="{ 'background-color': item.color }" />
        <span>{{ item.text }}</span>
      </div>
    </div>
    <el-container>
      <!-- <el-aside width="200px">
        <Menu :areaList="areaList" :handleGetList="handleGetList" />
        <div style="height: 50px" />
      </el-aside> -->

      <el-main>
        <Panel
          :dataList="dataList"
          :tagList="tagList"
          :tagListData="tagListData"
          :handleUpdate="handleUpdate"
        />
      </el-main>
    </el-container>
  </div>
</template>

<style lang="scss" scoped>
.common-layout {
  background-color: #fff;
  padding: 20px;
}
</style>
