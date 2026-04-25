<script setup lang="ts">
import { useHook } from "./utils/hook";
import { Calendar } from "@element-plus/icons-vue";

defineOptions({
  name: "safety-sign"
});

const { loading, activeName, tagList, dataList, deviceDetection, handleClick } =
  useHook();
</script>

<template>
  <div :class="['flex', 'justify-between', deviceDetection() && 'flex-wrap']">
    <div :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']">
      <el-card>
        <el-tabs
          v-model="activeName"
          type="card"
          class="demo-tabs"
          @tab-click="handleClick"
        >
          <!-- "One","Two","Three","Four","Five" -->
          <el-tab-pane
            v-for="item in tagList"
            :key="item.id"
            :label="item.code"
            :name="item.name"
          >
            <template #label>
              <span class="custom-tabs-label">
                <div class="itemTags">
                  <el-icon><calendar /></el-icon>
                  <span>{{ item.name }}</span>
                </div>
              </span>
            </template>
            <div v-loading="loading">
              <div v-for="(value, key) in dataList" :key="key">
                <div class="mt-8">
                  <p class="itemTitle">安全互锁信号/{{ key }}</p>
                  <el-row v-if="value.data.length" :gutter="10">
                    <el-col
                      v-for="signal in value.data"
                      :key="signal.name"
                      :xs="10"
                      :sm="6"
                      :md="4"
                      :lg="3"
                      :xl="3"
                    >
                      <div
                        :class="{
                          'bg-green-300': signal.value,
                          'bg-red-300': !signal.value,
                          'grid-content': true,
                          'ep-bg-purple': true,
                          'bg-opacity-50': true,
                          flex: true,
                          item: true
                        }"
                      >
                        <span class="mr-1">{{ signal.name }}</span>
                        <div
                          v-if="!signal.value"
                          class="w-6 h-6 bg-red-500 itemSignal"
                        />
                        <div v-else class="w-6 h-6 bg-green-500 itemSignal" />
                      </div>
                    </el-col>
                  </el-row>
                  <div v-else>无数据</div>
                </div>
              </div>
            </div>
          </el-tab-pane>
        </el-tabs>
      </el-card>
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
.el-col {
  border-radius: 4px;
}

.grid-content {
  border-radius: 4px;
  min-height: 36px;
}
.el-card {
  // min-height: 80vh;
  height: (calc(100vh - 160px));
  margin-bottom: 0;
}
.item {
  width: 144px;
  height: 14px;
  // background-color: #e6e6e6;
  display: flex;
  align-items: center;
  justify-content: space-evenly;
  border-radius: 12px;
  font-weight: 400;
  font-size: 14px;
  color: #333333;
  line-height: 41px;
}
.itemTitle {
  font-size: 16px;
  font-weight: bold;
  color: #333;
  line-height: 41px;
  // margin-bottom: 15px;
}
.itemSignal {
  // width: 30px;
  // height: 30px;
  border-radius: 50%;
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
</style>
