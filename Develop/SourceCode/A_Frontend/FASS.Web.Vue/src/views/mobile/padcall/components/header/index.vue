<template>
  <el-tabs v-model="activeName" type="card" @tab-click="handleClick">
    <el-tab-pane
      v-for="item in dataList"
      id="tabList"
      :key="item[0].id"
      :label="item[0].areaName"
      :name="item[0].id"
    />
  </el-tabs>
</template>

<script lang="ts" setup>
import { ref, watchEffect } from "vue";
import type { TabsPaneContext } from "element-plus";

interface TabsInterface {
  areaName: string;
  id: string;
}

const props = defineProps({
  getLableId: {
    type: Function
  },
  dataList: {
    type: Object as () => TabsInterface[]
  }
});
const activeName = ref();
watchEffect(() => {
  if (props.dataList && props.dataList.length > 0) {
    activeName.value = props.dataList[0][0].id;
  }
});

const handleClick = (tab: TabsPaneContext, event: Event) => {
  // console.log(tab, event);
  // console.log(tab.props.label);
  // console.log(tab.props.name);
  props.getLableId(tab.props.label);
};
</script>

<style>
.el-tabs__item {
  /* width: 90px; */
  min-width: 90px;
  height: 40px;
}
.el-tabs__item.is-active {
  color: #fff;
  font-weight: 600;
  font-size: 14px;
  background: linear-gradient(0deg, #1098c5, #45c5f0);
}
</style>
