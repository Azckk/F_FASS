<script setup lang="ts">
import { ref, onMounted, onUnmounted } from "vue";
import Menu from "./components/menu/index.vue";
import Header from "./components/header/index.vue";
import Main from "./components/main/index.vue";

import {
  GetDictItemListToSelect,
  GetPage,
  AddWork
} from "@/api/mobile/padcall";

const isCollapse = ref(false);
const width = ref("200px");
const id = ref(0);
const currentMenuCode = ref("");

function handleChangeIsShow() {
  isCollapse.value = !isCollapse.value;
  if (isCollapse.value) {
    setTimeout(() => {
      width.value = "54px";
    }, 500);
  } else {
    width.value = "200px";
  }
}
const menuList = ref();
const tabsList = ref();
const kwList = ref();
// 获取菜单列表
async function getMenuList() {
  let param = {
    param: "pad",
    dictCode: "StorageType"
  };
  const res = await GetDictItemListToSelect(param);
  // 请求菜单列表
  menuList.value = res.data;
  if (menuList.value.length > 0) {
    currentMenuCode.value = menuList.value[0].code;
    getMenuId(menuList.value[0].code);
  }
}
let transformArrayData;
async function getMenuId(e) {
  //console.log("eeeee++", menuList.value[0].code);

  let res = await GetPage({ type: e });
  console.log("res.data++", res.data);
  tabsList.value = res.data;
  // 储存当前数据，并且进行分组
  transformArrayData = transformArray(tabsList.value);
  // 请求当前菜单下的库区
  if (tabsList.value.length > 0 && tabsList.value[0].length > 0) {
    getLableId(tabsList.value[0][0].areaName);
  } else {
    tabsList.value = [
      [
        {
          areaName: "此菜单下无库区"
        }
      ]
    ];
    kwList.value = [];
  }
  currentMenuCode.value = e;
}
function getLableId(areaName) {
  kwList.value = transformArrayData[areaName];
}
function transformArray(arr) {
  let result = {};

  arr.forEach(group => {
    if (group.length > 0) {
      // 获取每组中第0项的areaName
      const areaName = group[0].areaName;

      // 使用areaName作为新的key，将整组数据赋值给该key
      result[areaName] = group;
    }
  });

  return result;
}

onMounted(() => {
  getMenuList();
});

// let timer;
// onMounted(() => {
//   getMenuList();
//   timer = setInterval(() => {
//     if (menuList.value && menuList.value.length > 0) {
//       getMenuId(currentMenuCode.value);
//     }
//   }, 5000);
// });

// onUnmounted(() => {
//   clearInterval(timer);
// });
</script>

<template>
  <div class="common-layout">
    <el-container>
      <el-aside :width="width">
        <Menu
          :isCollapse="isCollapse"
          :handleChangeIsShow="handleChangeIsShow"
          :getMenuId="getMenuId"
          :dataList="menuList"
        />
      </el-aside>
      <el-container>
        <div style="padding: 0px 20px 5px 20px; text-align: right">
          <el-button style="width: 54px" @click="getMenuId(currentMenuCode)">{{
            "刷新"
          }}</el-button>
        </div>

        <el-header>
          <Header :getLableId="getLableId" :dataList="tabsList" />
        </el-header>
        <el-main><Main :dataList="kwList" :tabsList="tabsList" :transformArrayData="transformArrayData" /></el-main>
      </el-container>
    </el-container>
  </div>
</template>

<style lang="scss" scoped>
.common-layout {
  background-color: #fff;
  padding: 10px;
  height: calc(100vh - 180px);
}
.el-main {
  padding: 0 20px;
}
.el-header {
  --el-header-height: 50px;
}
</style>
