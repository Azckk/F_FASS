<template>
  <el-menu
    :default-active="active"
    class="el-menu-vertical-demo"
    :collapse="isCollapse"
  >
    <el-button style="width: 54px" @click="handleChangeIsShow()">{{
      isCollapse ? "展开" : "收起"
    }}</el-button>
    <el-menu-item
      v-for="item in dataList"
      id="el-menu-item"
      :key="item.id"
      :index="item.id"
      @click="getMenuId(item.code)"
    >
      <img
        :src="
          item.code === active
            ? imgUrlHover[item.code.toString()]
            : imgUrl[item.code.toString()]
        "
        alt=""
      />
      <template #title>{{ item.name }}</template>
    </el-menu-item>
  </el-menu>
</template>

<script lang="ts" setup>
import { ref, watchEffect } from "vue";
import Call from "@/assets/mobile/padCall/call.png";
import Put from "@/assets/mobile/padCall/Put.png";
import Fetch from "@/assets/mobile/padCall/Fetch.png";

import CallHover from "@/assets/mobile/padCall/callHover.png";
import PutHover from "@/assets/mobile/padCall/PutHover.png";
import FetchHover from "@/assets/mobile/padCall/FetchHover.png";

const active = ref("");
const imgActive = ref("");
const imgUrl = ref({
  Call: Call,
  Fetch: Fetch,
  Put: Put
});
const imgUrlHover = ref({
  Call: CallHover,
  Fetch: FetchHover,
  Put: PutHover
});
interface MenuInterface {
  name: string;
  id: string;
  code: string;
}
const props = defineProps({
  isCollapse: {
    type: Boolean,
    default: false
  },
  handleChangeIsShow: {
    type: Function
  },
  getMenuId: {
    type: Function
  },
  dataList: {
    type: Object as () => MenuInterface[]
  }
});

watchEffect(() => {
  if (props.dataList && props.dataList.length > 0) {
    active.value = props.dataList[0].id;
    imgActive.value = props.dataList[0].code;
  }
});
</script>

<style scoped>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
  height: calc(100vh - 300px);
}
#el-menu-item {
  /* background-color: #fff; */
  height: 80px;
  font-size: 20px;
  font-weight: 700;
}
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-menu-item,
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-sub-menu__title {
  color: #6d5890 !important;
}
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-menu-item,
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-sub-menu__title {
  background-color: #fff;
}
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .is-active {
  color: #fff !important;
  background-color: #10ade1;
}
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-menu-item:hover,
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-sub-menu__title:hover {
  color: #333 !important;
}
</style>
