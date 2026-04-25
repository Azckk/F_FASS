<script setup lang="ts">
import {
  Document,
  Menu as IconMenu,
  Location,
  Setting
} from "@element-plus/icons-vue";
// import { useHook } from "../utils/hook";
// const {  } = useHook();
defineOptions({
  name: ""
});
const { areaList, handleGetList } = defineProps(["areaList", "handleGetList"]);
// console.log(areaList);
</script>

<template>
  <div class="menuList">
    <el-row class="tac">
      <el-col :span="12">
        <h5 class="mb-2">{{ $t("table.reservoirVisualization") }}</h5>
        <div v-if="areaList.length == 0">
          <span>No Data</span>
        </div>
        <div v-else id="menu">
          <el-menu>
            <el-menu-item
              id="itemList"
              active-text-color="#ffd04b"
              index="all"
              @click="handleGetList"
            >
              <div>
                <el-icon><icon-menu /></el-icon>
                <span>全部</span>
              </div>
            </el-menu-item>
          </el-menu>
          <el-menu v-for="item in areaList" :key="item.id">
            <el-menu-item
              id="itemList"
              active-text-color="#ffd04b"
              :index="item.id"
              @click="handleGetList"
            >
              <div>
                <el-icon><icon-menu /></el-icon>
                <span>{{ item.name }}</span>
              </div>
            </el-menu-item>
          </el-menu>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<style lang="scss" scoped>
.menuList {
  min-height: 200px;
}
#itemList:hover {
  background-color: rgb(90, 156, 248);
  border-radius: 4px;
}
#itemListFocus {
  background-color: rgb(90, 156, 248);
}

.el-menu--vertical {
  // background-color: transparent;
  width: 200px;
  // border: none;
}
::v-deep(.el-menu--vertical .el-menu-item) {
  background-color: #fff !important;
}
::v-deep(#menu .el-menu-item:hover) {
  color: #938f8f !important;
}
::v-deep(#menu .el-menu-item:active) {
  color: #000000 !important;
}
::v-deep(#menu .el-menu--vertical .is-active) {
  color: #000000 !important;
}
// .el-menu--vertical:hover,
// .el-menu-item:hover {
//   background-color: #409eff;
// }
</style>
