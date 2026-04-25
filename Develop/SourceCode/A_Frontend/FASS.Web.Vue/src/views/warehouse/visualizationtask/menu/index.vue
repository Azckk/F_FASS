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
import { ref } from "vue";

const radio1 = ref("all");
const { areaList, handleGetList } = defineProps(["areaList", "handleGetList"]);
// console.log(areaList);
</script>

<template>
  <div class="menuList">
    <el-row class="tac">
      <el-col :span="12">
        <div v-if="areaList.length == 0">
          <h5 class="mb-2">{{ $t("table.reservoirVisualization") }}</h5>
          <span>No Data</span>
        </div>
        <div v-else id="menu">
          <div>
            <h5 class="menu-title mb-2">
              {{ $t("table.reservoirVisualization") }}
            </h5>
          </div>

          <div>
            <el-radio-group v-model="radio1">
              <el-radio
                value="all"
                size="large"
                index="all"
                @click="handleGetList('all')"
                >全部</el-radio
              >
              <div class="el-radio-group-item">
                <el-radio
                  v-for="item in areaList"
                  :key="item.id"
                  :value="item.id"
                  :index="item.id"
                  size="large"
                  @click="handleGetList(item.id)"
                  >{{ item.name }}</el-radio
                >
              </div>
            </el-radio-group>
          </div>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<style lang="scss" scoped>
.menuList {
  // min-height: 200px;
}
#itemList:hover {
  background-color: rgb(90, 156, 248);
  border-radius: 4px;
}
#itemListFocus {
  background-color: rgb(90, 156, 248);
}
#menu {
  display: flex;
  align-items: center;
  .menu-title {
    padding-top: 8px;
    width: 100px;
  }
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
::v-deep(.el-radio-group) {
  flex-wrap: nowrap;
  width: auto;
}
.el-radio-group-item {
  display: flex;
  width: 100%;
  flex-wrap: nowrap;
}
</style>
