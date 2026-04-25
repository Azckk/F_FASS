<script setup lang="ts">
import { useHook } from "./utils/hook";
import { Calendar } from "@element-plus/icons-vue";
import { ref, watchEffect } from "vue";
import { CaretRight } from "@element-plus/icons-vue";

defineOptions({
  name: "safety-sign"
});

const {
  loading,
  activeName,
  StorageGetPageFun,
  tabList,
  dataList,
  deviceDetection,
  handleClick
} = useHook();
let collapseActiveNames = ref([0, 1, 2, 3, 4, 5]);
let btnText = ref("全部收起/展开");
const handleExpand = name => {
  if (collapseActiveNames.value.length == 0) {
    collapseActiveNames.value = [
      0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18
    ];
  } else {
    collapseActiveNames.value = [];
  }
};
</script>

<template>
  <div :class="['flex', 'justify-between', deviceDetection() && 'flex-wrap']">
    <div :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']">
      <div class="her">
        <el-card>
          <el-button
            style="position: absolute; right: 50px; top: 62px; z-index: 99"
            type="primary"
            @click="handleExpand"
            >{{ btnText }}</el-button
          >

          <el-tabs
            v-model="activeName"
            type="card"
            class="demo-tabs"
            @tab-click="handleClick"
          >
            <!-- "One","Two","Three","Four","Five" -->
            <el-tab-pane
              class="fixed-tabs"
              v-for="(item, index) in tabList"
              :key="item.DeviceName"
              :label="item.DeviceName"
              :name="index"
            >
              <template #label>
                <span class="custom-tabs-label">
                  <div class="itemTags">
                    <el-icon><calendar /></el-icon>
                    <span>{{ item.DeviceName }}</span>
                  </div>
                </span>
              </template>
            </el-tab-pane>
          </el-tabs>
        </el-card>
      </div>
      <div class="demo-collapse">
        <el-collapse v-model="collapseActiveNames">
          <el-collapse-item
            v-for="(value, key) in dataList"
            :key="key"
            :title="`${value.TypeName}【${value.TypeCode}】`"
            :name="key"
            :icon="CaretRight"
          >
            <template #icon="{ isActive }">
              <div class="icon-ele">
                <div
                  style="width: 20px; height: 20px"
                  :style="{
                    transform: isActive ? ' rotate(90deg)' : ''
                  }"
                >
                  <CaretRight />
                </div>
                <div>
                  {{ !isActive ? "展开" : "收起" }}
                </div>
              </div>
            </template>
            <div class="mt-8">
              <el-row v-if="value.Signals" :gutter="10">
                <el-col
                  v-for="signal in value.Signals"
                  :key="signal.Key"
                  :xs="10"
                  :sm="6"
                  :md="4"
                  :lg="3"
                  :xl="3"
                >
                  <div
                    :class="{
                      // 'bg-green-300': true,
                      // 'bg-red-300': signal.Value === 'false',
                      'grid-content': true,
                      'ep-bg-purple': true,
                      'bg-opacity-50': true,
                      flex: true,
                      item: true
                    }"
                    class="item-box"
                  >
                    <span class="mr-1">{{ signal.Key }}</span>
                    <div
                      v-if="signal.Value === 'false'"
                      class="w-6 h-6 bg-red-500 itemSignal"
                    />
                    <div
                      v-else-if="signal.Value === 'true'"
                      class="w-6 h-6 bg-green-500 itemSignal"
                    />
                    <div v-else class="w-6 others">
                      {{ signal.Value }}
                    </div>
                  </div>
                </el-col>
              </el-row>
              <div v-else>无数据</div>
            </div>
          </el-collapse-item>
        </el-collapse>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
.demo-tabs > .el-tabs__content {
  padding: 32px;
  color: #6b778c;
  font-size: 32px;
  font-weight: 600;
}
// :deep(.el-tabs__header, .is-top) {
//   position: absolute;
//   top: 30px;
//   left: 30px;
// }
// .fixed-tabs {
//   position: absolute;
//   top: 30px;
//   left: 30px;
// }
.her {
  position: absolute;
  width: calc(100% - 50px);
  background-color: #fff;
  z-index: 98;
}
.el-col {
  border-radius: 4px;
}

.grid-content {
  border-radius: 4px;
  min-height: 36px;
}
.el-card {
  // min-height: 80vh;
  // height: (calc(100vh - 160px));
  // min-height: 80vh;
  margin-bottom: 0;
  // overflow-y: auto;
}
.item {
  // width: 144px;
  height: 50px;
  // background-color: #e6e6e6;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-radius: 12px;
  font-weight: 400;
  font-size: 14px;
  color: #333333;
  padding: 0 5px;
  // line-height: 41px;
  margin-bottom: 10px;
}
.item-box {
  background-color: #e6e6e6;
}
.itemTitle {
  font-size: 16px;
  font-weight: bold;
  color: #333;
  line-height: 41px;
  // margin-bottom: 15px;
}
.itemSignal {
  width: 24px;
  height: 24px;
  border-radius: 50%;
}
.others {
  overflow: hidden;
  width: 30%;
}
.itemTags {
  height: 100%;
  width: 100%;
  display: flex;
  align-items: center;
}
:deep(.el-tabs__item) {
  transition: all 0.2s;
}
:deep(.el-tabs__item):active {
  transition: all 0.2s;
  color: #fff;
  background: #5a3092;
}
:deep(.is-active) {
  transition: all 0.2s;
  color: #fff;
  background: #5a3092;
  border-radius: 4px;
}
:deep(.el-collapse-item__header) {
  font-weight: 600;
  font-size: 16px;
}
:deep(.demo-collapse) {
  transition: all 0.2s;
  color: #000;
  background: #fff;
  border-radius: 4px;
  padding: 20px;
  margin-top: 100px;
  height: (calc(100vh - 260px));
  overflow-y: auto;
}
:deep(.demo-collapse .is-active) {
  transition: all 0.2s;
  color: #000;
  background: #fff;
  border-radius: 4px;
}
.icon-ele {
  // margin: 0 8px 0 auto;
  color: #5a3092;
  display: flex;
  font-size: 14px;
  font-weight: 300;
  align-items: center;
  width: 200px;
  height: 30px;
}
</style>
